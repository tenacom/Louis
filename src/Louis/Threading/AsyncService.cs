// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using Louis.Diagnostics;

namespace Louis.Threading;

/// <summary>
/// Base class for long-running services.
/// </summary>
/// <remarks>
/// <para>This class differs from the <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.backgroundservice">BackgroundService</see>
/// class found in <c>Microsoft.Extensions.Hosting</c> because it gives you more control over your service:</para>
/// <list type="bullet">
/// <item>you can either start your service in background by calling <see cref="StartAsync"/>,
/// or run it as a task by calling <see cref="RunAsync"/>;</item>
/// <item>you can, optionally, override the <see cref="OnStartServiceAsync"/> and <see cref="OnStopServiceAsync"/> methods
/// to separate setup and teardown from the core of your service;</item>
/// <item>you can use the <see cref="State"/> to know, at any time, if your service has been started, has finished starting,
/// is stopping, or has finished stopping;</item>
/// <item>you can synchronize other tasks with your service by calling <see cref="WaitUntilStartedAsync"/>
/// and <see cref="WaitUntilStoppedAsync"/>;</item>
/// <item>you can stop your service and let it complete in the background by calling <see cref="TryStop"/>,
/// or stop and wait for completion by calling <see cref="StopAsync"/>.</item>
/// </list>
/// </remarks>
public abstract class AsyncService : IAsyncDisposable, IDisposable
{
    private readonly CancellationTokenSource _stoppedTokenSource = new();
    private readonly TaskCompletionSource<bool> _startedCompletionSource = new();
    private readonly TaskCompletionSource<bool> _stoppedCompletionSource = new();
    private readonly object _stateSyncRoot = new();

    private InterlockedFlag _disposed;
    private AsyncServiceState _state = AsyncServiceState.Created;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncService"/> class.
    /// </summary>
    protected AsyncService()
    {
    }

    /// <summary>
    /// Gets the state of the service.
    /// </summary>
    /// <seealso cref="AsyncServiceState"/>
    public AsyncServiceState State
    {
        get
        {
            lock (_stateSyncRoot)
            {
                return _state;
            }
        }
        private set
        {
            lock (_stateSyncRoot)
            {
                _state = value;
            }
        }
    }

    /// <summary>
    /// Asynchronously run an asynchronous service.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the service.</param>
    /// <returns>
    /// A <see cref="Task"/> that will complete when the service has stopped.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The service has already been started, either by calling <see cref="RunAsync"/> or <see cref="StartAsync"/>.
    /// </exception>
    /// <remarks>
    /// <para>You may call one of <see cref="RunAsync"/> and <see cref="StartAsync"/> at most once.</para>
    /// </remarks>
    public Task RunAsync(CancellationToken cancellationToken) => RunAsyncCore(false, cancellationToken);

    /// <summary>
    /// Asynchronously start an asynchronous service, then return while it continues to run in the background.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the service.</param>
    /// <returns>
    /// A <see cref="Task{T}">Task&lt;bool&gt;</see> that will complete as soon as the service has started
    /// (in which case the result will be <see langword="true"/>), or it could not start, either because of an exception,
    /// or the cancellation of <paramref name="cancellationToken"/> (in which case the result will be <see langword="false"/>).
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The service has already been started, either by calling <see cref="RunAsync"/> or <see cref="StartAsync"/>.
    /// </exception>
    /// <remarks>
    /// <para>You may call one of <see cref="RunAsync"/> and <see cref="StartAsync"/> at most once.</para>
    /// <para>If your program needs to know the exact reason why a service stops or fails to start,
    /// do not use this method; call <see cref="RunAsync"/> instead.</para>
    /// </remarks>
    public Task<bool> StartAsync(CancellationToken cancellationToken)
    {
        _ = RunAsyncCore(true, cancellationToken);
        return WaitUntilStartedAsync();
    }

    /// <summary>
    /// Asynchronously stops an asynchronous service.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that will complete as soon as the service has finished stopping.
    /// If the service has already stopped, the returned task is already completed.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The service has not been started yet.
    /// </exception>
    public Task StopAsync() => TryStop() ? WaitUntilStoppedAsync() : Task.CompletedTask;

    /// <summary>
    /// Tries to stop an asynchronous service.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the service has been stopped;
    /// <see langword="false"/> if it had already stopped.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The service has not been started yet.
    /// </exception>
    public bool TryStop() => TryStopCore(out var exception) || (exception is null ? false : throw exception);

    /// <summary>
    /// Asynchronously wait until an asynchronous service has started.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}">Task&lt;bool&gt;</see> that will complete as soon as the service has started
    /// (in which case the result will be <see langword="true"/>), or it could not start
    /// (in which case the result will be <see langword="false"/>).
    /// </returns>
    /// <remarks>
    /// <para>If neither <see cref="RunAsync"/> nor <see cref="StartAsync"/> have been called,
    /// the returned task will not complete until one of them is called.</para>
    /// <para>If the service has already finished starting, the returned task is already completed.</para>
    /// </remarks>
    public Task<bool> WaitUntilStartedAsync() => _startedCompletionSource.Task;

    /// <summary>
    /// Asynchronously wait until an asynchronous service has stopped.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that will complete as soon as the service has started and then stopped,
    /// or until it has failed to start.</returns>
    /// <remarks>
    /// <para>If neither <see cref="RunAsync"/> nor <see cref="StartAsync"/> have been called,
    /// the returned task will not complete until one of them is called.</para>
    /// <para>If the service has already stopped, the returned task is already completed.</para>
    /// </remarks>
    public Task WaitUntilStoppedAsync() => _stoppedCompletionSource.Task;

    /// <inheritdoc/>
    /// <remarks>
    /// <para>This method stops the service and waits for completion if it has been started,
    /// then calls <see cref="OnDisposeServiceAsync"/> before releasing resources
    /// held by this class.</para>
    /// </remarks>
    public async ValueTask DisposeAsync()
    {
        AsyncServiceState previousState;
        lock (_stateSyncRoot)
        {
            if (!_disposed.TrySet())
            {
                return;
            }

            previousState = _state;
            if (previousState < AsyncServiceState.Starting)
            {
                _state = AsyncServiceState.Disposed;
            }
        }

        if (previousState < AsyncServiceState.Starting)
        {
            _ = _startedCompletionSource.TrySetResult(false);
            _ = _stoppedCompletionSource.TrySetResult(true);
        }
        else
        {
            if (TryStopCore(out var exception) && exception is null)
            {
                await WaitUntilStoppedAsync().ConfigureAwait(false);
            }

            State = AsyncServiceState.Disposed;
        }

        await OnDisposeServiceAsync().ConfigureAwait(false);
        _stoppedTokenSource.Dispose();
    }

    /// <inheritdoc />
    /// <remarks>
    /// <para>This method exists for compatibility with older libraries (e.g. DI containers) lacking support for
    /// <see cref="IAsyncDisposable"/>. It just calls <see cref="DisposeAsync"/> and waits for it synchronously.</para>
    /// <para>If possible, you should use <see cref="DisposeAsync"/> instead of this method.</para>
    /// </remarks>
#pragma warning disable CA2012 // Use ValueTasks correctly - This is correct usage because we consume the ValueTask only once.
    public void Dispose() => DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
#pragma warning restore CA2012 // Use ValueTasks correctly

    /// <summary>
    /// <para>Asynchronously releases managed resources owned by this instance.</para>
    /// <para>Note that an instance of a class derived from <see cref="AsyncService"/>
    /// cannot directly own unmanaged resources.</para>
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the ongoing operation.</returns>
    protected virtual ValueTask OnDisposeServiceAsync() => default;

    /// <summary>
    /// When overridden in a derived class, performs asynchronous operations
    /// related to starting the service.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the operation.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the ongoing operation.</returns>
    protected virtual ValueTask OnStartServiceAsync(CancellationToken cancellationToken) => default;

    /// <summary>
    /// When overridden in a derived class, performs asynchronous operations
    /// related to stopping the service.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the ongoing operation.</returns>
    /// <remarks>
    /// <para>This method is called if and only if the <see cref="Task"/> returned by <see cref="StartAsync"/>
    /// completes successfully, even if the service is prevented from starting afterwards
    /// (for example if the <see cref="CancellationToken"/> passed to <see cref="StartAsync"/>
    /// is canceled after <see cref="StartAsync"/> checks it for the last time).</para>
    /// </remarks>
    protected virtual ValueTask OnStopServiceAsync() => default;

    /// <summary>
    /// When overridden in a derived class, performs the actual operations the service is meant to carry out.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the service.</param>
    /// <returns>A <see cref="Task"/> representing the ongoing operation.</returns>
    protected abstract Task RunServiceAsync(CancellationToken cancellationToken);

    [DoesNotReturn]
    private static void ThrowOnObjectDisposed(string message)
        => throw new ObjectDisposedException(message);

    [DoesNotReturn]
    private static void ThrowMultipleExceptions(params Exception[] innerExceptions)
        => throw new AggregateException(innerExceptions);

    private bool TryStopCore(out Exception? exception)
    {
        lock (_stateSyncRoot)
        {
            switch (_state)
            {
                case < AsyncServiceState.Starting:
                    exception = new InvalidOperationException("The service has not been started yet.");
                    return false;
                case > AsyncServiceState.Running:
                    exception = null;
                    return false;
            }

            _state = AsyncServiceState.Stopping;
        }

        _stoppedTokenSource.Cancel();
        exception = null;
        return true;
    }

    private async Task RunAsyncCore(bool runInBackground, CancellationToken cancellationToken)
    {
        lock (_stateSyncRoot)
        {
            switch (_state)
            {
                case AsyncServiceState.Created:
                    _state = AsyncServiceState.Starting;
                    break;
                case AsyncServiceState.Disposed:
                    ThrowOnObjectDisposed("Trying to run a disposed async service.");
                    break;
                default:
                    ThrowHelper.ThrowInvalidOperationException("An async service cannot be started more than once.");
                    return;
            }
        }

        // Return immediately when called from StartAsync;
        // continue synchronously when called from RunAsync.
        if (runInBackground)
        {
            await Task.Yield();
        }

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_stoppedTokenSource.Token, cancellationToken);
        var onStartCalled = false;
        Exception? exception = null;
        try
        {
            try
            {
                // Perform start actions.
                await OnStartServiceAsync(cts.Token).ConfigureAwait(false);
                onStartCalled = true;

                // Check the cancellation token, in case cancellation has been requested
                // but StartServiceAsync has not honored the request.
                cts.Token.ThrowIfCancellationRequested();
            }
            catch
            {
                _startedCompletionSource.SetResult(false);
                throw;
            }

            State = AsyncServiceState.Running;
            _startedCompletionSource.SetResult(true);
            await RunServiceAsync(cts.Token).ConfigureAwait(false);
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            exception = e;
        }

        if (onStartCalled)
        {
            State = AsyncServiceState.Stopping;
            try
            {
                await OnStopServiceAsync().ConfigureAwait(false);
            }
            catch (Exception) when (exception is null)
            {
                throw;
            }
            catch (Exception e) when (!e.IsCriticalError())
            {
                ThrowMultipleExceptions(exception!, e);
            }
            finally
            {
                State = AsyncServiceState.Stopped;
            }
        }
        else
        {
            State = AsyncServiceState.Stopping;
        }

        _stoppedCompletionSource.SetResult(true);
        if (exception is not null)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}

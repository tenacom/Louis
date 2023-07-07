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
/// <item>you can, optionally, override the <see cref="SetupAsync"/> and <see cref="TeardownAsync"/> methods
/// to separate setup and teardown from the core of your service;</item>
/// <item>you can either start your service in background by calling <see cref="Start"/>,
/// start it in background and wait for preliminary operations to complete by calling <see cref="StartAsync"/>,
/// or run it as a task by calling <see cref="RunAsync"/>;</item>
/// <item>you can use the <see cref="State"/> property to know, at any time, if your service has been started, has finished starting,
/// is stopping, has finished stopping, or has been disposed;</item>
/// <item>you can use the <see cref="DoneToken"/> property to get a cancellation token that will be canceled
/// as soon as execution of the service stops, just before the teardown phase starts;</item>
/// <item>you can synchronize other tasks with your service by calling <see cref="WaitUntilStartedAsync"/>
/// and <see cref="WaitUntilStoppedAsync"/>;</item>
/// <item>you can stop your service and let it complete in the background by calling <see cref="Stop"/>,
/// or stop and wait for completion by calling <see cref="StopAsync"/>.</item>
/// </list>
/// </remarks>
public abstract class AsyncService : IAsyncDisposable, IDisposable
{
    private readonly CancellationTokenSource _stoppedTokenSource = new();
    private readonly CancellationTokenSource _doneTokenSource = new();
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
                UnsafeSetState(value);
            }
        }
    }

    /// <summary>
    /// Gets a <see cref="CancellationToken"/> that is canceled as soon as the service has finished executing
    /// (either successfully or with an exception) or has failed starting.
    /// </summary>
    public CancellationToken DoneToken => _doneTokenSource.Token;

    /// <summary>
    /// Asynchronously runs an asynchronous service.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the service.</param>
    /// <returns>
    /// A <see cref="Task"/> that will complete when the service has stopped.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The service has already been started, either by calling <see cref="RunAsync"/>, <see cref="Start"/>,
    /// or <see cref="StartAsync"/>.
    /// </exception>
    /// <remarks>
    /// <para>Only one of <see cref="RunAsync"/>, <see cref="Start"/>, and <see cref="StartAsync"/> may be called, at most once.</para>
    /// </remarks>
    public Task RunAsync(CancellationToken cancellationToken) => RunAsyncCore(false, cancellationToken);

    /// <summary>
    /// Starts an asynchronous service, then return while it continues to run in the background.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the service.</param>
    /// <exception cref="InvalidOperationException">
    /// The service has already been started, either by calling <see cref="RunAsync"/>, <see cref="Start"/>, or <see cref="StartAsync"/>.
    /// </exception>
    /// <remarks>
    /// <para>Only one of <see cref="RunAsync"/>, <see cref="Start"/>, and <see cref="StartAsync"/> may be called, at most once.</para>
    /// <para>If your program needs to know the exact reason why a service stops or fails to start,
    /// do not use this method; call <see cref="RunAsync"/> from a separate task instead.</para>
    /// </remarks>
    public void Start(CancellationToken cancellationToken) => _ = RunAsyncCore(true, cancellationToken);

    /// <summary>
    /// Asynchronously starts an asynchronous service and waits for its <see cref="SetupAsync"/> method to complete.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the service.</param>
    /// <returns>
    /// A <see cref="Task{T}">Task&lt;bool&gt;</see> that will complete as soon as the service has started
    /// (in which case the result will be <see langword="true"/>), or it could not start, either because of an exception,
    /// or the cancellation of <paramref name="cancellationToken"/> (in which case the result will be <see langword="false"/>).
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The service has already been started, either by calling <see cref="RunAsync"/>, <see cref="Start"/>,  or <see cref="StartAsync"/>.
    /// </exception>
    /// <remarks>
    /// <para>You may call one of <see cref="RunAsync"/> and <see cref="StartAsync"/> at most once.</para>
    /// <para>If your program needs to know the exact reason why a service stops or fails to start,
    /// do not use this method; call <see cref="RunAsync"/> from a separate task instead.</para>
    /// </remarks>
    public Task<bool> StartAsync(CancellationToken cancellationToken)
    {
        _ = RunAsyncCore(true, cancellationToken);
        return WaitUntilStartedAsync();
    }

    /// <summary>
    /// Stops an asynchronous service, without waiting for it to complete.
    /// If the service has not started yet, calling this method prevents it from starting.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the service was running and has been requested to stop;
    /// <see langword="false"/> if the service was not running.
    /// </returns>
    public bool Stop() => StopCore(false);

    /// <summary>
    /// Asynchronously stops an asynchronous service and waits for it to complete.
    /// If the service has not started yet, calling this method prevents it from starting.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}">Task&lt;bool&gt;</see> that will complete as soon as the service has finished stopping
    /// (in which case the result will be <see langword="true"/>), or immediately if the service was not running
    /// (in which case the result will be <see langword="false"/>).
    /// </returns>
    public async Task<bool> StopAsync()
    {
        if (!StopCore(false))
        {
            return false;
        }

        await WaitUntilStoppedAsync().ConfigureAwait(false);
        return true;
    }

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
    /// then calls <see cref="DisposeResourcesAsync"/> before releasing resources
    /// held by this class.</para>
    /// </remarks>
    public async ValueTask DisposeAsync()
    {
        bool hadStarted;
        lock (_stateSyncRoot)
        {
            if (!_disposed.TrySet())
            {
                return;
            }

            hadStarted = StopCore(true);
        }

        if (hadStarted)
        {
            await WaitUntilStoppedAsync().ConfigureAwait(false);
        }

        await DisposeResourcesAsync().ConfigureAwait(false);
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
    protected virtual ValueTask DisposeResourcesAsync() => default;

    /// <summary>
    /// When overridden in a derived class, performs asynchronous operations
    /// related to starting the service.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the operation.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the ongoing operation.</returns>
    protected virtual ValueTask SetupAsync(CancellationToken cancellationToken) => default;

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
    protected virtual ValueTask TeardownAsync() => default;

    /// <summary>
    /// When overridden in a derived class, performs the actual operations the service is meant to carry out.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the service.</param>
    /// <returns>A <see cref="Task"/> representing the ongoing operation.</returns>
    protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

    /// <summary>
    /// <para>Called whenever the <see cref="State"/> property changes.</para>
    /// <para>This method must return as early as possible, must not throw, and should be only used for logging purposes.</para>
    /// </summary>
    /// <param name="oldState">The old value of <see cref="State"/>.</param>
    /// <param name="newState">The new value of <see cref="State"/>.</param>
    protected virtual void LogStateChanged(AsyncServiceState oldState, AsyncServiceState newState)
    {
    }

    /// <summary>
    /// <para>Called if the <see cref="SetupAsync"/> method throws <see cref="OperationCanceledException"/>
    /// and the service has been requested to stop.</para>
    /// <para>This method must return as early as possible, must not throw, and should be only used for logging purposes.</para>
    /// </summary>
    protected virtual void LogSetupCanceled()
    {
    }

    /// <summary>
    /// <para>Called if the <see cref="SetupAsync"/> method throws an exception, except when such exception indicates
    /// that service execution has been canceled.</para>
    /// <para>This method must return as early as possible, must not throw, and should be only used for logging purposes.</para>
    /// </summary>
    /// <param name="exception">The exception thrown by <see cref="SetupAsync"/>.</param>
    protected virtual void LogSetupFailed(Exception exception)
    {
    }

    /// <summary>
    /// <para>Called if the <see cref="ExecuteAsync"/> method throws <see cref="OperationCanceledException"/>
    /// and the service has been requested to stop.</para>
    /// <para>This method must return as early as possible, must not throw, and should be only used for logging purposes.</para>
    /// </summary>
    protected virtual void LogExecuteCanceled()
    {
    }

    /// <summary>
    /// <para>Called if the <see cref="ExecuteAsync"/> method throws an exception, except when such exception indicates
    /// that service execution has been canceled.</para>
    /// <para>This method must return as early as possible, must not throw, and should be only used for logging purposes.</para>
    /// </summary>
    /// <param name="exception">The exception thrown by <see cref="ExecuteAsync"/>.</param>
    protected virtual void LogExecuteFailed(Exception exception)
    {
    }

    /// <summary>
    /// <para>Called if the <see cref="TeardownAsync"/> method throws an exception.</para>
    /// <para>This method must return as early as possible, must not throw, and should be only used for logging purposes.</para>
    /// </summary>
    /// <param name="exception">The exception thrown by <see cref="TeardownAsync"/>.</param>
    protected virtual void LogTeardownFailed(Exception exception)
    {
    }

    [DoesNotReturn]
    private static void ThrowOnObjectDisposed(string message)
        => throw new ObjectDisposedException(message);

    private static void AggregateAndThrowIfNeeded(Exception? exception1, Exception? exception2)
    {
        if (exception1 is not null)
        {
            if (exception2 is not null)
            {
                ThrowMultipleExceptions(exception1, exception2);
            }
            else
            {
                ExceptionDispatchInfo.Capture(exception1).Throw();
            }
        }
        else if (exception2 is not null)
        {
            ExceptionDispatchInfo.Capture(exception2).Throw();
        }
    }

    [DoesNotReturn]
    private static void ThrowMultipleExceptions(params Exception[] innerExceptions)
        => throw new AggregateException(innerExceptions);

    private bool StopCore(bool disposing)
    {
        lock (_stateSyncRoot)
        {
            switch (_state)
            {
                case < AsyncServiceState.Starting:
                    _ = _startedCompletionSource.TrySetResult(false);
                    _ = _stoppedCompletionSource.TrySetResult(true);
                    UnsafeSetState(disposing ? AsyncServiceState.Disposed : AsyncServiceState.Stopped);
                    return false;

                case AsyncServiceState.Disposed:
                    return false;

                case > AsyncServiceState.Running:
                    if (disposing)
                    {
                        UnsafeSetState(AsyncServiceState.Disposed);
                    }

                    return true;

                default:
                    UnsafeSetState(disposing ? AsyncServiceState.Disposed : AsyncServiceState.Stopping);
                    _stoppedTokenSource.Cancel();
                    return true;
            }
        }
    }

    private async Task RunAsyncCore(bool runInBackground, CancellationToken cancellationToken)
    {
        lock (_stateSyncRoot)
        {
            switch (_state)
            {
                case AsyncServiceState.Created:
                    UnsafeSetState(AsyncServiceState.Starting);
                    break;
                case AsyncServiceState.Disposed:
                    ThrowOnObjectDisposed("Trying to run a disposed async service.");
                    break;
                default:
                    ThrowHelper.ThrowInvalidOperationException("An async service cannot be started more than once.");
                    return;
            }
        }

        // Return immediately when called from Start or StartAsync;
        // continue synchronously when called from RunAsync.
        if (runInBackground)
        {
            await Task.Yield();
        }

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(_stoppedTokenSource.Token, cancellationToken);
        var setupCompleted = false;
        Exception? exception = null;
        try
        {
            // Perform start actions.
            await SetupAsync(cts.Token).ConfigureAwait(false);

            // Check the cancellation token, in case cancellation has been requested
            // but SetupAsync has not honored the request.
            cts.Token.ThrowIfCancellationRequested();

            setupCompleted = true;
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            LogSetupCanceled();
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            LogSetupFailed(e);
            exception = e;
        }

        if (!setupCompleted)
        {
            _startedCompletionSource.SetResult(false);
            _stoppedCompletionSource.SetResult(true);
            _doneTokenSource.Cancel();
            State = AsyncServiceState.Stopped;

            // Only propagate exceptions if there is a caller to propagate to.
            if (exception is not null && !runInBackground)
            {
                ExceptionDispatchInfo.Capture(exception).Throw();
            }

            return;
        }

        State = AsyncServiceState.Running;
        _startedCompletionSource.SetResult(true);
        try
        {
            await ExecuteAsync(cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            LogExecuteCanceled();
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            LogExecuteFailed(e);
            exception = e;
        }

        Exception? teardownException = null;
        _doneTokenSource.Cancel();
        State = AsyncServiceState.Stopping;
        try
        {
            await TeardownAsync().ConfigureAwait(false);
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            LogTeardownFailed(e);
            teardownException = e;
        }

        State = AsyncServiceState.Stopped;
        _stoppedCompletionSource.SetResult(true);
        AggregateAndThrowIfNeeded(exception, teardownException);
    }

    private void UnsafeSetState(AsyncServiceState value)
    {
        if (value == _state)
        {
            return;
        }

        var oldState = _state;
        _state = value;
        LogStateChanged(oldState, _state);
    }
}

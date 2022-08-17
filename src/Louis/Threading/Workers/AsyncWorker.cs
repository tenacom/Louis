// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Louis.Diagnostics;
using Louis.Threading.Workers.Internal;
using Microsoft.Extensions.Logging;

namespace Louis.Threading.Workers;

/// <inheritdoc cref="IAsyncWorker"/>
/// <summary>
/// Base class for the implementation of an asynchronous worker.
/// </summary>
public abstract class AsyncWorker : IAsyncWorker
{
    private readonly CancellationTokenSource _stoppedTokenSource = new();
    private readonly TaskCompletionSource<bool> _startedCompletionSource = new();
    private readonly TaskCompletionSource<bool> _stoppedCompletionSource = new();
    private readonly object _stateSyncRoot = new();

    private InterlockedFlag _disposed;
    private AsyncWorkerState _state = AsyncWorkerState.Created;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncWorker"/> class.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> interface used to log the worker's activity.</param>
    protected AsyncWorker(ILogger logger)
    {
        Logger = logger;
    }

    /// <inheritdoc />
    public AsyncWorkerState State
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
                if (value == _state)
                {
                    return;
                }

                _state = value;
                Logger.ServiceStateChanged(value);
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="ILogger"/> interface to use for logging the worker's activity.
    /// </summary>
    protected ILogger Logger { get; }

    /// <inheritdoc />
    public Task RunAsync(CancellationToken cancellationToken) => RunAsyncCore(false, cancellationToken);

    /// <inheritdoc />
    public Task<bool> StartAsync(CancellationToken cancellationToken)
    {
        _ = RunAsyncCore(true, cancellationToken);
        return WaitUntilStartedAsync();
    }

    /// <inheritdoc />
    public bool TryStop() => TryStopCore(out var exception) || (exception is null ? false : throw exception);

    /// <inheritdoc />
    public Task<bool> WaitUntilStartedAsync() => _startedCompletionSource.Task;

    /// <inheritdoc />
    public Task WaitUntilStoppedAsync() => _stoppedCompletionSource.Task;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        AsyncWorkerState previousState;
        lock (_stateSyncRoot)
        {
            if (!_disposed.TrySet())
            {
                return;
            }

            previousState = _state;
            if (previousState < AsyncWorkerState.Starting)
            {
                _state = AsyncWorkerState.Disposed;
            }
        }

        if (previousState < AsyncWorkerState.Starting)
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

            State = AsyncWorkerState.Disposed;
        }

        await DisposeAsyncCore().ConfigureAwait(false);
        _stoppedTokenSource.Dispose();
    }

    /// <inheritdoc />
    public void Dispose() => DisposeAsync().AsTask().ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// <para>Asynchronously releases managed resources owned by this instance.</para>
    /// <para>Note that an instance of a class derived from <see cref="AsyncWorker"/>
    /// cannot directly own unmanaged resources.</para>
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the ongoing operation.</returns>
    protected virtual ValueTask DisposeAsyncCore() => default;

    /// <summary>
    /// When overridden in a derived class, performs asynchronous operations
    /// related to starting the worker.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the operation.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the ongoing operation.</returns>
    protected abstract ValueTask StartWorkerAsync(CancellationToken cancellationToken);

    /// <summary>
    /// When overridden in a derived class, performs asynchronous operations
    /// related to stopping the worker.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the ongoing operation.</returns>
    /// <remarks>
    /// <para>This method is called if and only if the <see cref="Task"/> returned by <see cref="StartAsync"/>
    /// completes successfully, even if the worker is prevented from starting afterwards
    /// (for example if the <see cref="CancellationToken"/> passed to <see cref="StartAsync"/>
    /// is canceled after <see cref="StartAsync"/> checks it for the last time).</para>
    /// </remarks>
    protected abstract ValueTask StopWorkerAsync();

    /// <summary>
    /// When overridden in a derived class, performs the actual operations the worker is meant to carry out.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the worker.</param>
    /// <returns>A <see cref="Task"/> representing the ongoing operation.</returns>
    protected abstract Task RunAsyncInternal(CancellationToken cancellationToken);

    private bool TrySetStarting()
    {
        lock (_stateSyncRoot)
        {
            if (_state != AsyncWorkerState.Created)
            {
                return false;
            }

            _state = AsyncWorkerState.Starting;
            return true;
        }
    }

    private bool TryStopCore(out Exception? exception)
    {
        lock (_stateSyncRoot)
        {
            switch (_state)
            {
                case < AsyncWorkerState.Starting:
                    exception = new InvalidOperationException("The service has not been started.");
                    return false;
                case > AsyncWorkerState.Running:
                    exception = null;
                    return false;
            }

            _state = AsyncWorkerState.Stopping;
        }

        _stoppedTokenSource.Cancel();
        exception = null;
        return true;
    }

    // Note that State MUST be already set to Starting upon entering this method.
    private async Task RunAsyncCore(bool runInBackground, CancellationToken cancellationToken)
    {
        if (!TrySetStarting())
        {
            throw new InvalidOperationException("The service has already been started.");
        }

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
                await StartWorkerAsync(cts.Token).ConfigureAwait(false);
                onStartCalled = true;

                // Check the cancellation token, in case cancellation has been requested
                // but StartWorkerAsync has not honored the request.
                cts.Token.ThrowIfCancellationRequested();
            }
            catch
            {
                _startedCompletionSource.SetResult(false);
                throw;
            }

            State = AsyncWorkerState.Running;
            _startedCompletionSource.SetResult(true);
            await RunAsyncInternal(cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
        {
            Logger.OperationCanceled();
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            exception = e;
        }

        if (onStartCalled)
        {
            State = AsyncWorkerState.Stopping;
            try
            {
                await StopWorkerAsync().ConfigureAwait(false);
            }
            catch (Exception) when (exception is null)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new AggregateException(exception!, e);
            }
            finally
            {
                State = AsyncWorkerState.Stopped;
            }
        }
        else
        {
            State = AsyncWorkerState.Stopping;
        }

        _stoppedCompletionSource.SetResult(true);
        if (exception is not null)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
    }
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using Louis.Hosting.Internal;
using Louis.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Louis.Hosting;

/// <summary>
/// Subclasses <see cref="AsyncService"/>, providing support for logging and implementing <see cref="IHostedService"/>.
/// </summary>
public abstract partial class AsyncHostedService : AsyncService, IHostedService
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncHostedService"/> class.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger"/> to use for logging.</param>
    protected AsyncHostedService(ILogger logger)
    {
        Guard.IsNotNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        LogHostedServiceStarting();
        if (await StartAsync(cancellationToken).ConfigureAwait(false))
        {
            return;
        }

        // If AsyncService.StartAsync returns false, then either the service was canceled, or SetupAsync faulted.
        cancellationToken.ThrowIfCancellationRequested();
        ThrowHelper.ThrowInvalidOperationException("Service start faulted.");
    }

    /// <inheritdoc/>
    async Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        LogHostedServiceStopping();
        _ = await Task.WhenAny(StopAsync(), Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.StateChanged,
        level: LogLevel.Trace,
        message: "Service state {oldState} -> {newState}")]
    protected sealed override partial void LogStateChanged(AsyncServiceState oldState, AsyncServiceState newState);

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.BeforeSetup,
        level: LogLevel.Trace,
        message: "Starting setup phase")]
    protected sealed override partial void LogBeforeSetup();

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.SetupCompleted,
        level: LogLevel.Trace,
        message: "Setup phase completed")]
    protected sealed override partial void LogSetupCompleted();

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.SetupCanceled,
        level: LogLevel.Warning,
        message: "Service execution was canceled during setup phase")]
    protected sealed override partial void LogSetupCanceled();

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.SetupFailed,
        level: LogLevel.Error,
        message: "Service setup phase failed")]
    protected sealed override partial void LogSetupFailed(Exception exception);

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.BeforeExecute,
        level: LogLevel.Trace,
        message: "Starting service execution")]
    protected sealed override partial void LogBeforeExecute();

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.ExecuteCompleted,
        level: LogLevel.Trace,
        message: "Service execution completed")]
    protected sealed override partial void LogExecuteCompleted();

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.ExecuteCanceled,
        level: LogLevel.Warning,
        message: "Service execution was canceled")]
    protected sealed override partial void LogExecuteCanceled();

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.ExecuteFailed,
        level: LogLevel.Error,
        message: "Service execution failed")]
    protected sealed override partial void LogExecuteFailed(Exception exception);

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.BeforeTeardown,
        level: LogLevel.Trace,
        message: "Starting teardown phase")]
    protected sealed override partial void LogBeforeTeardown();

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.TeardownCompleted,
        level: LogLevel.Trace,
        message: "Teardown phase completed")]
    protected sealed override partial void LogTeardownCompleted();

    /// <inheritdoc/>
    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.TeardownFailed,
        level: LogLevel.Error,
        message: "Service teardown phase failed")]
    protected sealed override partial void LogTeardownFailed(Exception exception);

    /// <inheritdoc/>
    protected sealed override void LogStopRequested(AsyncServiceState previousState, AsyncServiceState currentState, bool result)
        => LogStopRequestedCore(previousState, result ? "running" : "not running");

    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.StopRequested,
        level: LogLevel.Information,
        message: "Stop requested while service {running} ({previousState})")]
    private partial void LogStopRequestedCore(AsyncServiceState previousState, string running);

    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.HostedServiceStarting,
        level: LogLevel.Trace,
        message: "Hosted service starting")]
    private partial void LogHostedServiceStarting();

    [LoggerMessage(
        eventId: EventIds.AsyncHostedService.HostedServiceStopping,
        level: LogLevel.Trace,
        message: "Hosted service stopping")]
    private partial void LogHostedServiceStopping();
}

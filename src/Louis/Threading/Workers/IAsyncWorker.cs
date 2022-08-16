// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Louis.Threading.Workers;

/// <summary>
/// <para>Represents an asynchronous worker, e.g. a web server.</para>
/// <para>The basic usage of an asynchronous worker is as follows:</para>
/// <list type="bullet">
/// <item><description>use appropriate properties and/or methods to configure the worker;</description></item>
/// <item><description>call <see cref="RunAsync"/> or <see cref="StartAsync"/> to start the worker;</description></item>
/// <item><description>use the <see cref="CancellationTokenSource"/> passed to <see cref="RunAsync"/> or <see cref="StartAsync"/>
/// to stop the worker;</description></item>
/// <item><description>dispose the worker when it has stopped.</description></item>
/// </list>
/// </summary>
/// <seealso cref="AsyncWorker"/>
public interface IAsyncWorker : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Gets the state of the service.
    /// </summary>
    /// <value>The state of the service.</value>
    /// <seealso cref="AsyncWorkerState"/>
    AsyncWorkerState State { get; }

    /// <summary>
    /// Asynchronously run an asynchronous worker.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the worker.</param>
    /// <returns>
    /// A <see cref="Task"/> that will complete when the worker has stopped.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The worker has already been started, either by calling <see cref="RunAsync"/> or <see cref="StartAsync"/>.
    /// </exception>
    /// <remarks>
    /// <para>You may call either <see cref="RunAsync"/> or <see cref="StartAsync"/> exactly once.</para>
    /// </remarks>
    Task RunAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously start an asynchronous worker, then return while it continues to run in the background.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to stop the service.</param>
    /// <returns>
    /// A <see cref="Task{T}">Task&lt;bool&gt;</see> that will complete as soon as the worker has started
    /// (in which case the result will be <see langword="true"/>), or it could not start, either because of an exception,
    /// or the cancellation of <paramref name="cancellationToken"/> (in which case the result will be <see langword="false"/>).
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The worker has already been started, either by calling <see cref="RunAsync"/> or <see cref="StartAsync"/>.
    /// </exception>
    /// <remarks>
    /// <para>You may call either <see cref="RunAsync"/> or <see cref="StartAsync"/> exactly once.</para>
    /// <para>If your program needs to know the exact reason why a service stops or fails to start,
    /// do not use this method; call <see cref="RunAsync"/> instead.</para>
    /// </remarks>
    Task<bool> StartAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Tries to stop an asynchronous worker.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the worker has been stopped;
    /// <see langword="false"/> if it had already stopped.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The worker has not been started yet.
    /// </exception>
    bool TryStop();

    /// <summary>
    /// Asynchronously wait until an asynchronous worker has started.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{T}">Task&lt;bool&gt;</see> that will complete as soon as the worker has started
    /// (in which case the result will be <see langword="true"/>), or it could not start
    /// (in which case the result will be <see langword="false"/>).
    /// </returns>
    /// <remarks>
    /// <para>If neither <see cref="RunAsync"/> nor <see cref="StartAsync"/> have been called,
    /// the returned task will not complete until one of them is called.</para>
    /// <para>If the worker has already started, the returned task will already be completed.</para>
    /// </remarks>
    Task<bool> WaitUntilStartedAsync();

    /// <summary>
    /// Asynchronously wait until an asynchronous worker has stopped.
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that will complete as soon as the worker has started and then stopped,
    /// or until it has failed to start.</returns>
    /// <remarks>
    /// <para>If neither <see cref="RunAsync"/> nor <see cref="StartAsync"/> have been called,
    /// the returned task will not complete until one of them is called.</para>
    /// <para>If the worker has already stopped, the returned task will already be completed.</para>
    /// </remarks>
    Task WaitUntilStoppedAsync();
}

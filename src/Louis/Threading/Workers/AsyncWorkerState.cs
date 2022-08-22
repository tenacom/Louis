// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

namespace Louis.Threading.Workers;

// NOTE TO CONTRIBUTORS:
// =====================
// Do not reorder fields or change their values.
// It is important that ServiceState values represent, in ascending order,
// the stages of a service's lifetime, so that comparisons can be made.
// For example,
//
// State < LongRunningServiceState.Running
//
// in the context of a web server means "not yet ready to accept requests".

/// <summary>
/// Represents the state of an <see cref="AsyncWorker"/>.
/// </summary>
public enum AsyncWorkerState
{
    /// <summary>
    /// The service has not started yet.
    /// </summary>
    Created,

    /// <summary>
    /// The service is starting but not functional yet.
    /// </summary>
    Starting,

    /// <summary>
    /// The service is running.
    /// </summary>
    Running,

    /// <summary>
    /// The service is stopping.
    /// </summary>
    Stopping,

    /// <summary>
    /// The service has stopped.
    /// </summary>
    Stopped,

    /// <summary>
    /// The service has been disposed.
    /// </summary>
    Disposed,
}

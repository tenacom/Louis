// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Louis.Threading;

/// <summary>
/// Represents the state of an <see cref="AsyncService"/>.
/// </summary>
public enum AsyncServiceState
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

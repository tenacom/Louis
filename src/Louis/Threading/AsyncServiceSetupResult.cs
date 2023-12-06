// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Louis.Threading;

/// <summary>
/// Represents the outcome of the setup phase of an <see cref="AsyncService"/>.
/// </summary>
public enum AsyncServiceSetupResult
{
    /// <summary>
    /// <para>Setup succeeded (the task returned by <see cref="AsyncService.SetupAsync"/> completed with <see langword="true"/>).</para>
    /// <para>Service execution continues normally.</para>
    /// </summary>
    Successful,

    /// <summary>
    /// <para>Setup was not started (the service was stopped without being started).</para>
    /// <para>Service execution is stopped.</para>
    /// </summary>
    NotStarted,

    /// <summary>
    /// <para>Setup failed (the task returned by <see cref="AsyncService.SetupAsync"/> completed with <see langword="false"/>).</para>
    /// <para>Service execution is stopped.</para>
    /// </summary>
    Unsuccessful,

    /// <summary>
    /// <para>Setup was canceled (the task returned by <see cref="AsyncService.SetupAsync"/> was canceled).</para>
    /// <para>Service execution is stopped.</para>
    /// </summary>
    Canceled,

    /// <summary>
    /// <para>Setup failed with an exception (the task returned by <see cref="AsyncService.SetupAsync"/> has faulted).</para>
    /// <para>Service execution is stopped.</para>
    /// </summary>
    Faulted,
}

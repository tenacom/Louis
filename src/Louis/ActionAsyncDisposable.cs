// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;

namespace Louis;

/// <summary>
/// Implements both <see cref="IAsyncDisposable"/> and <see cref="IDisposable"/> by invoking a given callback upon disposal.
/// </summary>
public class ActionAsyncDisposable : IAsyncDisposable, IDisposable
{
    private volatile Func<ValueTask>? _onDisposeAsync;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionAsyncDisposable"/> class.
    /// </summary>
    /// <param name="onDisposeAsync">A callback to invoke upon the first call to <see cref="Dispose"/> or <see cref="DisposeAsync"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="onDisposeAsync"/> is <see langword="null"/>.</exception>
    /// <seealso cref="ActionDisposable"/>
    /// <seealso cref="LocalActionDisposable"/>
    public ActionAsyncDisposable(Func<ValueTask> onDisposeAsync)
    {
        Guard.IsNotNull(onDisposeAsync);
        _onDisposeAsync = onDisposeAsync;
    }

    /// <summary>
    /// <para>When invoked for the first time, invokes the callback passed to the constructor
    /// and synchronously awaits its result.</para>
    /// <para>If / when invoked again, or if invoked after <see cref="DisposeAsync"/>, this method does nothing.</para>
    /// </summary>
    public void Dispose() => Interlocked.Exchange(ref _onDisposeAsync, null)?.Invoke().AsTask().GetAwaiter().GetResult();

    /// <summary>
    /// <para>When invoked for the first time, invokes the callback passed to the constructor.</para>
    /// <para>If / when invoked again, or if invoked after <see cref="Dispose"/>, this method does nothing.</para>
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the ongoing operation.</returns>
    public ValueTask DisposeAsync() => Interlocked.Exchange(ref _onDisposeAsync, null)?.Invoke() ?? default;
}

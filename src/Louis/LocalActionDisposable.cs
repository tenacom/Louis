// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using CommunityToolkit.Diagnostics;

namespace Louis;

/// <summary>
/// <para>Implements <see cref="IDisposable"/> by invoking a given <see cref="Action"/> upon disposal.</para>
/// <para>This type should be used instead of <see cref="ActionDisposable"/> when needed in a local scope,
/// as it avoids a heap allocation.</para>
/// </summary>
public ref struct LocalActionDisposable
{
    private volatile Action? _onDispose;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalActionDisposable"/> struct.
    /// </summary>
    /// <param name="onDispose">The <see cref="Action"/> to call upon the first call to <see cref="Dispose"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="onDispose"/> is <see langword="null"/>.</exception>
    /// <seealso cref="ActionDisposable"/>
    /// <seealso cref="ActionAsyncDisposable"/>
    public LocalActionDisposable(Action onDispose)
    {
        Guard.IsNotNull(onDispose);
        _onDispose = onDispose;
    }

    /// <summary>
    /// <para>When invoked for the first time, invokes the <see cref="Action"/> passed to the constructor.</para>
    /// <para>If / when invoked again, this method does nothing.</para>
    /// </summary>
    public void Dispose() => Interlocked.Exchange(ref _onDispose, null)?.Invoke();
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;

namespace Louis;

/// <summary>
/// Provides extension methods for objects implementing <see cref="IAsyncDisposable"/>.
/// </summary>
public static class AsyncDisposableExtensions
{
    /// <summary>
    /// Disposes an <see cref="IAsyncDisposable"/> synchronously by awaiting the result of <see cref="IAsyncDisposable.DisposeAsync"/>.
    /// </summary>
    /// <param name="this">The <see cref="IAsyncDisposable"/> interface on which this method is called.</param>
    /// <remarks>
    /// <para>The most common use case for this method is add an implementation of <see cref="IDisposable.Dispose"/>
    /// to a type that already implements <see cref="IAsyncDisposable.DisposeAsync"/>, without duplicating any code:</para>
    /// <code>
    ///     public void Dispose() => this.DisposeSynchronously();
    /// </code>
    /// </remarks>
    public static void DisposeSynchronously(this IAsyncDisposable @this)
    {
        Guard.IsNotNull(@this);

#pragma warning disable CA2012 // Use ValueTasks correctly - ValueTask gets consumed only once, hence this is correct usage.
        @this.DisposeAsync().GetAwaiter().GetResult();
#pragma warning restore CA2012 // Use ValueTasks correctly
    }
}

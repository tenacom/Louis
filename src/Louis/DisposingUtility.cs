// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Louis.Collections;

namespace Louis;

/// <summary>
/// Provides utility methods to handle disposing of objects with a mix of <see cref="IDisposable"/>
/// and <see cref="IAsyncDisposable"/> interfaces.
/// </summary>
public static class DisposingUtility
{
    /// <summary>
    /// Asynchronously or synchronously disposes the specified object.
    /// </summary>
    /// <param name="obj">The object to dispose.</param>
    /// <returns>A <see cref="ValueTask"/> representing the ongoing operation.</returns>
    /// <remarks>
    /// <para>If <paramref name="obj"/> implements <see cref="IAsyncDisposable"/>,
    /// it is disposed asynchronously and the returned <see cref="ValueTask"/> represents
    /// the ongoing call to <see cref="IAsyncDisposable.DisposeAsync"/>.</para>
    /// <para>Otherwise, if <paramref name="obj"/> implements <see cref="IDisposable"/>,
    /// it is disposed synchronously and this method returns a completed <see cref="ValueTask"/>.</para>
    /// <para>Otherwise, or if <paramref name="obj"/> is <see langword="null"/>,
    /// this method immediately returns a completed <see cref="ValueTask"/>.</para>
    /// </remarks>
    /// <seealso cref="Dispose"/>
    public static ValueTask DisposeAsync(object? obj)
    {
        switch (obj)
        {
            case IAsyncDisposable asyncDisposable:
                return asyncDisposable.DisposeAsync();
            case IDisposable disposable:
                disposable.Dispose();
                return default;
            default:
                return default;
        }
    }

    /// <summary>
    /// Synchronously disposes the specified object.
    /// </summary>
    /// <param name="obj">The object to dispose.</param>
    /// <remarks>
    /// <para>If <paramref name="obj"/> implements <see cref="IDisposable"/>,
    /// its <see cref="IDisposable.Dispose"/> method is called.</para>
    /// <para>Otherwise, if <paramref name="obj"/> implements <see cref="IAsyncDisposable"/>,
    /// its <see cref="IAsyncDisposable.DisposeAsync"/> method is called
    /// and the resulting <see cref="ValueTask"/> is awaited before this method returns.</para>
    /// <para>Otherwise, or if <paramref name="obj"/> is <see langword="null"/>,
    /// this method returns immediately.</para>
    /// </remarks>
    /// <seealso cref="DisposeAsync"/>
    public static void Dispose(object? obj)
    {
        switch (obj)
        {
            case IDisposable disposable:
                disposable.Dispose();
                break;
            case IAsyncDisposable asyncDisposable:
#pragma warning disable CA2012 // Use ValueTasks correctly - We are consuming it once, hence this is a correct, albeit unusual, use of ValueTask.
                asyncDisposable.DisposeAsync().ConfigureAwait(true).GetAwaiter().GetResult();
#pragma warning restore CA2012 // Use ValueTasks correctly
                break;
        }
    }

    /// <summary>
    /// Asynchronously dispose all specified disposable objects.
    /// </summary>
    /// <param name="items">The objects to dispose.</param>
    /// <returns>A <see cref="ValueTask"/> representing the ongoing operation.</returns>
    /// <remarks>This method works by calling <see cref="EnumerableExtensions.DisposeAllAsync"/>
    /// on the <paramref name="items"/> array.</remarks>
    /// <seealso cref="EnumerableExtensions.DisposeAllAsync"/>
    public static ValueTask DisposeAllAsync(params object?[] items) => items.DisposeAllAsync();

    /// <summary>
    /// Synchronously dispose all specified disposable objects.
    /// </summary>
    /// <param name="items">The objects to dispose.</param>
    /// <remarks>This method works by calling <see cref="EnumerableExtensions.DisposeAll"/>
    /// on the <paramref name="items"/> array.</remarks>
    /// <seealso cref="EnumerableExtensions.DisposeAll"/>
    public static void DisposeAll(params object?[] items) => items.DisposeAll();
}

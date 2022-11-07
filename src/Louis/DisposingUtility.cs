// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
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
    /// <para>This method will not dispose
    /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct">disposable ref structs</see>.</para>
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
    /// <para>This method will not dispose
    /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct">disposable ref structs</see>.</para>
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
                asyncDisposable.DisposeSynchronously();
                break;
        }
    }

    /// <summary>
    /// Asynchronously dispose all specified disposable objects.
    /// </summary>
    /// <param name="items">The objects to dispose.</param>
    /// <returns>A <see cref="ValueTask"/> representing the ongoing operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>This method works by calling <see cref="EnumerableExtensions.DisposeAllAsync"/>
    /// on the <paramref name="items"/> array.</para>
    /// <para>This method will not dispose
    /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct">disposable ref structs</see>.</para>
    /// </remarks>
    /// <seealso cref="EnumerableExtensions.DisposeAllAsync"/>
    public static ValueTask DisposeAllAsync(params object?[] items)
    {
        Guard.IsNotNull(items);

        return items.DisposeAllAsync();
    }

    /// <summary>
    /// Synchronously dispose all specified disposable objects.
    /// </summary>
    /// <param name="items">The objects to dispose.</param>
    /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>This method works by calling <see cref="EnumerableExtensions.DisposeAll"/>
    /// on the <paramref name="items"/> array.</para>
    /// <para>This method will not dispose
    /// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct">disposable ref structs</see>.</para>
    /// </remarks>
    /// <seealso cref="EnumerableExtensions.DisposeAll"/>
    public static void DisposeAll(params object?[] items)
    {
        Guard.IsNotNull(items);

        items.DisposeAll();
    }
}

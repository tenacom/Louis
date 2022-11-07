// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text;
using CommunityToolkit.Diagnostics;

namespace Louis.Diagnostics;

/// <summary>
/// Provides extension methods for <see cref="StringBuilder"/>.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Appends a text representation of an object to the end of this instance,
    /// using the same algorithm as <see cref="ExceptionHelper.FormatObject"/>.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="obj">The object to represent. Can be <see langword="null"/>.</param>
    /// <param name="format">An optional format string to apply if <paramref name="obj"/>
    /// implements <see cref="IFormattable"/>.</param>
    /// <returns>A reference to this instance after the append operation has completed..</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static StringBuilder AppendFormattedObject(this StringBuilder @this, object? obj, string? format = null)
    {
        Guard.IsNotNull(@this);

        return @this.Append(ExceptionHelper.FormatObject(obj, format));
    }
}

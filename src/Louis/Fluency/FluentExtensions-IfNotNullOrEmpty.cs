// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;

namespace Louis.Fluency;

partial class FluentExtensions
{
    /// <summary>
    /// Invokes an action on an object if a string is neither <see langword="null"/> nor the empty string, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="arg">The string to check and pass to <paramref name="then"/> if called.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> and <paramref name="arg"/>if <paramref name="arg "/> is not <see langword="null"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="then"/>, if called, returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T IfNotNullOrEmpty<T>(this T @this, string? arg, FluentAction<T, string> then)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return string.IsNullOrEmpty(arg) ? @this : then(@this, arg!);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Invokes an action on an object if a string is neither <see langword="null"/> nor the empty string, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="arg">The string to check and pass to <paramref name="then"/> if called.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> and <paramref name="arg"/>if <paramref name="arg "/> is not <see langword="null"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="then"/>, if called, returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T IfNotNullOrEmpty<T>(this T @this, string? arg, Action<T, string> then)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);

        if (!string.IsNullOrEmpty(arg))
        {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
            then(@this, arg!);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        return @this;
    }
}

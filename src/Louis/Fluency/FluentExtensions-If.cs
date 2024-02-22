// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;

namespace Louis.Fluency;

partial class FluentExtensions
{
    /// <summary>
    /// Invokes an action on an object if a condition is verified, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="condition">The condition to test.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="then"/>, if called, returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// </exception>
    /// <seealso cref="If{T}(T,bool,Action{T})"/>
    public static T If<T>(this T @this, bool condition, FluentAction<T> then)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return condition ? then(@this) : @this;
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Invokes an action on an object if a condition is verified, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="condition">The condition to test.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="then"/>, if called, returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// </exception>
    /// <seealso cref="If{T}(T,bool,FluentAction{T})"/>
    public static T If<T>(this T @this, bool condition, Action<T> then)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);

        if (condition)
        {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
            then(@this);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        return @this;
    }
}

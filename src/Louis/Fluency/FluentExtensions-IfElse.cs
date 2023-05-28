// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;

namespace Louis.Fluency;

partial class FluentExtensions
{
    /// <summary>
    /// Invokes one of two actions on an object according to a boolean condition, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="condition">The condition to test.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <param name="else">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="false"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after either <paramref name="then"/> or <paramref name="else"/> returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="else"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T IfElse<T>(this T @this, bool condition, FluentAction<T> then, FluentAction<T> @else)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);
        Guard.IsNotNull(@else);

        return condition ? then(@this) : @else(@this);
    }

    /// <summary>
    /// Invokes one of two actions on an object according to a boolean condition, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="condition">The condition to test.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <param name="else">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="false"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after either <paramref name="then"/> or <paramref name="else"/> returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="else"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T IfElse<T>(this T @this, bool condition, Action<T> then, Action<T> @else)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);
        Guard.IsNotNull(@else);

        if (condition)
        {
            then(@this);
        }
        else
        {
            @else(@this);
        }

        return @this;
    }
}

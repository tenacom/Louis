// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;

namespace Louis.Fluency;

partial class FluentExtensions
{
    /// <summary>
    /// Selects an action to invoke on an object by comparing a given value against a list of comparands, then returns the object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TValue">The type of the value being compared.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="cases">
    /// <para>An array of pairs, where each element consists of a value to compare against <paramref name="value"/>
    /// and an optional action to perform on <paramref name="this"/> if they are equal.</para>
    /// <para>If the action is <see langword="null"/>, no action is performed if the value is equal to <paramref name="value"/>;
    /// instead, the method returns immediately.</para>
    /// </param>
    /// <returns>A reference to <paramref name="this"/> after one of the actions in <paramref name="cases"/>, if any, returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="cases"/> is <see langword="null"/>.</para>
    /// </exception>
    /// <remarks>
    /// <para>If <paramref name="value"/> is not equal to any of the values in <paramref name="cases"/>, this method returns immediately.</para>
    /// </remarks>
    public static T Switch<T, TValue>(this T @this, TValue value, params (TValue Comparand, FluentAction<T>? Action)[] cases)
        where TValue : IEquatable<TValue>
        => Switch(@this, value, null, cases);

    /// <summary>
    /// Selects an action to invoke on an object by comparing a given value against a list of comparands, then returns the object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TValue">The type of the value being compared.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="default">
    /// <para>An optional action to perform on <paramref name="this"/> if <paramref name="value"/> is not equal
    /// to any of the values in <paramref name="cases"/>.</para>
    /// <para>If this parameter is <see langword="null"/> and <paramref name="value"/> is not equal
    /// to any of the values in <paramref name="cases"/>, this method returns immediately.</para>
    /// </param>
    /// <param name="cases">
    /// <para>An array of pairs, where each element consists of a value to compare against <paramref name="value"/>
    /// and an optional action to perform on <paramref name="this"/> if they are equal.</para>
    /// <para>If the action is <see langword="null"/>, no action is performed if the value is equal to <paramref name="value"/>;
    /// instead, the method returns immediately.</para>
    /// </param>
    /// <returns>A reference to <paramref name="this"/> after one of the actions in <paramref name="cases"/>, if any, returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="cases"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Switch<T, TValue>(this T @this, TValue value, FluentAction<T>? @default, params (TValue Comparand, FluentAction<T>? Action)[] cases)
        where TValue : IEquatable<TValue>
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(cases);

        foreach (var (comparand, action) in cases)
        {
            if (value.Equals(comparand))
            {
                return action == null ? @this : action(@this);
            }
        }

        return @default == null ? @this : @default(@this);
    }
}

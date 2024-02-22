// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Louis;

partial class RangeCheck
{
    /// <summary>
    /// Returns <paramref name="value"/> clamped to the inclusive range
    /// of <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being clamped.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> &lt; <paramref name="min"/>, <paramref name="min"/>.</para>
    /// <para>If <paramref name="value"/> &gt; <paramref name="max"/>, <paramref name="max"/>.</para>
    /// <para>Otherwise, <paramref name="value"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static T Clamp<T>(T value, T min, T max)
        where T : IComparable<T>
    {
        EnsureValidRange(min, max);
        return value.CompareTo(min) < 0 ? min
                : value.CompareTo(max) > 0 ? max
                : value;
    }

    /// <summary>
    /// Returns <paramref name="value"/> clamped to the inclusive range
    /// of <paramref name="min"/> and <paramref name="max"/>,
    /// using the specified <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being clamped.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="comparer">An <see cref="IComparer{T}">IComparer&lt;T&gt;</see>
    /// to use for comparisons.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> &lt; <paramref name="min"/>, <paramref name="min"/>.</para>
    /// <para>If <paramref name="value"/> &gt; <paramref name="max"/>, <paramref name="max"/>.</para>
    /// <para>Otherwise, <paramref name="value"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static T Clamp<T>(T value, T min, T max, IComparer<T> comparer)
        where T : notnull
    {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        EnsureValidComparerAndRange(min, max, comparer);
#pragma warning restore CA1062 // Validate arguments of public methods
        return comparer.Compare(value, min) < 0 ? min
                : comparer.Compare(value, max) > 0 ? max
                : value;
    }
}

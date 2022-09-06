// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Louis.ArgumentValidation;

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
        EnsureValidComparerAndRange(min, max, comparer);
        return comparer.Compare(value, min) < 0 ? min
                : comparer.Compare(value, max) > 0 ? max
                : value;
    }
}

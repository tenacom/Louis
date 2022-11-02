// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Louis;

partial class RangeCheck
{
    /// <summary>
    /// Returns <paramref name="value"/>, if it is not <see langword="null"/>,
    /// clamped to the inclusive range of <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being clamped.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is <see langword="null"/>, <see langword="null"/>.</para>
    /// <para>If <paramref name="value"/> &lt; <paramref name="min"/>, <paramref name="min"/>.</para>
    /// <para>If <paramref name="value"/> &gt; <paramref name="max"/>, <paramref name="max"/>.</para>
    /// <para>Otherwise, <paramref name="value"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    [return:NotNullIfNotNull("value")]
    public static T? ClampOrNull<T>(T? value, T min, T max)
        where T : class, IComparable<T>
        => value == null ? null : Clamp(value, min, max);

    /// <summary>
    /// Returns <paramref name="value"/>, if it is not <see langword="null"/>,
    /// clamped to the inclusive range of <paramref name="min"/> and <paramref name="max"/>,
    /// using the specified <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being clamped.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="comparer">An <see cref="IComparer{T}">IComparer&lt;T&gt;</see>
    /// to use for comparisons.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is <see langword="null"/>, <see langword="null"/>.</para>
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
    [return:NotNullIfNotNull("value")]
    public static T? ClampOrNull<T>(T? value, T min, T max, IComparer<T> comparer)
        where T : class
        => value == null ? null : Clamp(value, min, max, comparer);

    /// <summary>
    /// Returns <paramref name="value"/>, if it is not <see langword="null"/>,
    /// clamped to the inclusive range of <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being clamped.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is <see langword="null"/>, <see langword="null"/>.</para>
    /// <para>If <paramref name="value"/> &lt; <paramref name="min"/>, <paramref name="min"/>.</para>
    /// <para>If <paramref name="value"/> &gt; <paramref name="max"/>, <paramref name="max"/>.</para>
    /// <para>Otherwise, <paramref name="value"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    [return:NotNullIfNotNull("value")]
    public static T? ClampOrNull<T>(T? value, T min, T max)
        where T : struct, IComparable<T>
        => value.HasValue ? Clamp(value.Value, min, max) : null;

    /// <summary>
    /// Returns <paramref name="value"/>, if it is not <see langword="null"/>,
    /// clamped to the inclusive range of <paramref name="min"/> and <paramref name="max"/>,
    /// using the specified <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being clamped.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="comparer">An <see cref="IComparer{T}">IComparer&lt;T&gt;</see>
    /// to use for comparisons.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is <see langword="null"/>, <see langword="null"/>.</para>
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
    [return:NotNullIfNotNull("value")]
    public static T? ClampOrNull<T>(T? value, T min, T max, IComparer<T> comparer)
        where T : struct
        => value.HasValue ? Clamp(value.Value, min, max, comparer) : null;
}

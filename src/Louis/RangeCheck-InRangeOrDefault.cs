// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Louis;

partial class RangeCheck
{
    /// <summary>
    /// Returns a value if it is within an inclusive range, or a default value otherwise.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="defaultValue">The value to return if <paramref name="value"/>
    /// is not within the specified range.</param>
    /// <returns>
    /// If <paramref name="value"/> is within <paramref name="min"/> and <paramref name="max"/>, including boundaries, <paramref name="value"/>;
    /// otherwise, <paramref name="defaultValue"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static T InRangeOrDefault<T>(T value, T min, T max, T defaultValue)
        where T : IComparable<T>
        => Verify(value, min, max) ? value : defaultValue;

    /// <summary>
    /// Returns a value if it is within an inclusive range, or a default value otherwise,
    /// using a specified <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="defaultValue">The value to return if <paramref name="value"/>
    /// is not within the specified range.</param>
    /// <param name="comparer">The comparer to use for comparisons.</param>
    /// <returns>
    /// If <paramref name="value"/> is within <paramref name="min"/> and <paramref name="max"/>, including boundaries, <paramref name="value"/>;
    /// otherwise, <paramref name="defaultValue"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static T InRangeOrDefault<T>(T value, T min, T max, T defaultValue, IComparer<T> comparer)
        where T : notnull
        => Verify(value, min, max, comparer) ? value : defaultValue;

    /// <summary>
    /// Returns a value if it is within an inclusive range, or a default value otherwise.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="defaultValue">The value to return if <paramref name="value"/>
    /// is not within the specified range.</param>
    /// <param name="acceptNull"><see langword="true"/> to consider <see langword="null"/> values
    /// as if within range; <see langword="false"/> otherwise.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is <see langword="null"/> and <paramref name="acceptNull"/> is <see langword="true"/>, <see langword="null"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/> and <paramref name="acceptNull"/> is <see langword="false"/>, <paramref name="defaultValue"/>.</para>
    /// <para>If <paramref name="value"/> is within <paramref name="min"/> and <paramref name="max"/>, including boundaries, <paramref name="value"/>.</para>
    /// <para>Otherwise, <paramref name="defaultValue"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    [return:NotNullIfNotNull("value")]
    public static T? InRangeOrDefault<T>(T? value, T min, T max, T? defaultValue, bool acceptNull)
        where T : class, IComparable<T>
        => value == null
            ? (acceptNull ? null : defaultValue)
            : (Verify(value, min, max) ? value : defaultValue);

    /// <summary>
    /// Returns a value if it is within an inclusive range, or a default value otherwise,
    /// using a specified <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="defaultValue">The value to return if <paramref name="value"/>
    /// is not within the specified range.</param>
    /// <param name="acceptNull"><see langword="true"/> to consider <see langword="null"/> values
    /// as if within range; <see langword="false"/> otherwise.</param>
    /// <param name="comparer">The comparer to use for comparisons.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is <see langword="null"/> and <paramref name="acceptNull"/> is <see langword="true"/>, <see langword="null"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/> and <paramref name="acceptNull"/> is <see langword="false"/>, <paramref name="defaultValue"/>.</para>
    /// <para>If <paramref name="value"/> is within <paramref name="min"/> and <paramref name="max"/>, including boundaries, <paramref name="value"/>.</para>
    /// <para>Otherwise, <paramref name="defaultValue"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    [return:NotNullIfNotNull("value")]
    public static T? InRangeOrDefault<T>(T? value, T min, T max, T? defaultValue, bool acceptNull, IComparer<T> comparer)
        where T : class
        => value == null
            ? (acceptNull ? null : defaultValue)
            : (Verify(value, min, max, comparer) ? value : defaultValue);

    /// <summary>
    /// Returns a value if it is within an inclusive range, or a default value otherwise.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="defaultValue">The value to return if <paramref name="value"/>
    /// is not within the specified range.</param>
    /// <param name="acceptNull"><see langword="true"/> to consider <see langword="null"/> values
    /// as if within range; <see langword="false"/> otherwise.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is <see langword="null"/> and <paramref name="acceptNull"/> is <see langword="true"/>, <see langword="null"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/> and <paramref name="acceptNull"/> is <see langword="false"/>, <paramref name="defaultValue"/>.</para>
    /// <para>If <paramref name="value"/> is within <paramref name="min"/> and <paramref name="max"/>, including boundaries, <paramref name="value"/>.</para>
    /// <para>Otherwise, <paramref name="defaultValue"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    [return:NotNullIfNotNull("value")]
    public static T? InRangeOrDefault<T>(T? value, T min, T max, T? defaultValue, bool acceptNull)
        where T : struct, IComparable<T>
        => value.HasValue
            ? (Verify(value.Value, min, max) ? defaultValue : value.Value)
            : (acceptNull ? null : defaultValue);

    /// <summary>
    /// Returns a value if it is within an inclusive range, or a default value otherwise,
    /// using a specified <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="defaultValue">The value to return if <paramref name="value"/>
    /// is not within the specified range.</param>
    /// <param name="acceptNull"><see langword="true"/> to consider <see langword="null"/> values
    /// as if within range; <see langword="false"/> otherwise.</param>
    /// <param name="comparer">The comparer to use for comparisons.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is <see langword="null"/> and <paramref name="acceptNull"/> is <see langword="true"/>, <see langword="null"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/> and <paramref name="acceptNull"/> is <see langword="false"/>, <paramref name="defaultValue"/>.</para>
    /// <para>If <paramref name="value"/> is within <paramref name="min"/> and <paramref name="max"/>, including boundaries, <paramref name="value"/>.</para>
    /// <para>Otherwise, <paramref name="defaultValue"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    [return:NotNullIfNotNull("value")]
    public static T? InRangeOrDefault<T>(T? value, T min, T max, T? defaultValue, bool acceptNull, IComparer<T> comparer)
        where T : struct
        => value.HasValue
            ? (Verify(value.Value, min, max, comparer) ? defaultValue : value.Value)
            : (acceptNull ? null : defaultValue);
}

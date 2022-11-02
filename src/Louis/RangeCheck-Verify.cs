// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Louis.ArgumentValidation;

namespace Louis;

partial class RangeCheck
{
    /// <summary>
    /// Checks whether a value is within an inclusive range.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is within <paramref name="min"/>
    /// and <paramref name="max"/>, including boundaries; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static bool Verify<T>(T value, T min, T max)
        where T : IComparable<T>
    {
        EnsureValidRange(min, max);
        return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }

    /// <summary>
    /// Checks whether a value is within an inclusive range,
    /// using the specified <paramref name="comparer"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="comparer">The comparer to use for comparisons.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is within <paramref name="min"/>
    /// and <paramref name="max"/>, including boundaries; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static bool Verify<T>(T value, T min, T max, IComparer<T> comparer)
        where T : notnull
    {
        EnsureValidComparerAndRange(min, max, comparer);
        return comparer.Compare(value, min) >= 0 && comparer.Compare(value, max) <= 0;
    }

    /// <summary>
    /// Checks whether a nullable value is within an inclusive range,
    /// accepting or rejecting <see langword="null"/> values according to the
    /// value of the <paramref name="acceptNull"/> parameter.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="acceptNull">The Boolean value to return when <paramref name="value"/> is <see langword="null"/>.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is not <see langword="null"/>:
    /// <see langword="true"/> if <paramref name="value"/> is within <paramref name="min"/>
    /// and <paramref name="max"/>, including boundaries; otherwise, <see langword="false"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/>:
    /// the value passed in <paramref name="acceptNull"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static bool Verify<T>(T? value, T min, T max, bool acceptNull)
        where T : class, IComparable<T>
        => value is null ? acceptNull : Verify(value, min, max);

    /// <summary>
    /// Checks whether a nullable value is within an inclusive range,
    /// using the specified <paramref name="comparer"/>, and
    /// accepting or rejecting <see langword="null"/> values according to the
    /// value of the <paramref name="acceptNull"/> parameter.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="acceptNull">The Boolean value to return when <paramref name="value"/> is <see langword="null"/>.</param>
    /// <param name="comparer">The comparer to use for comparisons.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is not <see langword="null"/>:
    /// <see langword="true"/> if <paramref name="value"/> is within <paramref name="min"/>
    /// and <paramref name="max"/>, including boundaries; otherwise, <see langword="false"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/>:
    /// the value passed in <paramref name="acceptNull"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static bool Verify<T>(T? value, T min, T max, bool acceptNull, IComparer<T> comparer)
        where T : class
        => value is null ? acceptNull : Verify(value, min, max, comparer);

    /// <summary>
    /// Checks whether a nullable value is within an inclusive range,
    /// accepting or rejecting <see langword="null"/> values according to the
    /// value of the <paramref name="acceptNull"/> parameter.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="acceptNull">The Boolean value to return when <paramref name="value"/> is <see langword="null"/>.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is not <see langword="null"/>:
    /// <see langword="true"/> if <paramref name="value"/> is within <paramref name="min"/>
    /// and <paramref name="max"/>, including boundaries; otherwise, <see langword="false"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/>:
    /// the value passed in <paramref name="acceptNull"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static bool Verify<T>(T? value, T min, T max, bool acceptNull)
        where T : struct, IComparable<T>
        => value.HasValue ? Verify(value.Value, min, max) : acceptNull;

    /// <summary>
    /// Checks whether a nullable value is within an inclusive range,
    /// using the specified <paramref name="comparer"/>, and
    /// accepting or rejecting <see langword="null"/> values according to the
    /// value of the <paramref name="acceptNull"/> parameter.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value being checked.</param>
    /// <param name="min">The inclusive lower bound of the range.</param>
    /// <param name="max">The inclusive upper bound of the range.</param>
    /// <param name="acceptNull">The Boolean value to return when <paramref name="value"/> is <see langword="null"/>.</param>
    /// <param name="comparer">The comparer to use for comparisons.</param>
    /// <returns>
    /// <para>If <paramref name="value"/> is not <see langword="null"/>:
    /// <see langword="true"/> if <paramref name="value"/> is within <paramref name="min"/>
    /// and <paramref name="max"/>, including boundaries; otherwise, <see langword="false"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/>:
    /// the value passed in <paramref name="acceptNull"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/>.
    /// </exception>
    public static bool Verify<T>(T? value, T min, T max, bool acceptNull, IComparer<T> comparer)
        where T : struct
        => value.HasValue ? Verify(value.Value, min, max, comparer) : acceptNull;
}

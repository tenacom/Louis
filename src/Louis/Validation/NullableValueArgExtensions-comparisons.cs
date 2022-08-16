// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;

namespace Louis.ArgumentValidation;

[StackTraceHidden]
partial class NullableValueArgExtensions
{
    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="NullableValueArg{T}"/> object
    /// is greater than a specified value according to <see cref="IComparable{T}"/> semantics.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T">The underlying type of the argument.</typeparam>
    /// <param name="this">The <see cref="NullableValueArg{T}"/> on which this method is called.</param>
    /// <param name="threshold">The value against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static NullableValueArg<T> GreaterThan<T>(this NullableValueArg<T> @this, T threshold, string? message = null)
        where T : struct, IComparable<T>
        => @this.UnlessNull(ValueArgExtensions.GreaterThan, threshold, message);

    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="NullableValueArg{T}"/> object
    /// is greater than or equal to a specified value according to <see cref="IComparable{T}"/> semantics.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T">The underlying type of the argument.</typeparam>
    /// <param name="this">The <see cref="NullableValueArg{T}"/> on which this method is called.</param>
    /// <param name="threshold">The value against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static NullableValueArg<T> GreaterThanOrEqualTo<T>(this NullableValueArg<T> @this, T threshold, string? message = null)
        where T : struct, IComparable<T>
        => @this.UnlessNull(ValueArgExtensions.GreaterThanOrEqualTo, threshold, message);

    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="NullableValueArg{T}"/> object
    /// is less than a specified value according to <see cref="IComparable{T}"/> semantics.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T">The underlying type of the argument.</typeparam>
    /// <param name="this">The <see cref="NullableValueArg{T}"/> on which this method is called.</param>
    /// <param name="threshold">The value against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static NullableValueArg<T> LessThan<T>(this NullableValueArg<T> @this, T threshold, string? message = null)
        where T : struct, IComparable<T>
        => @this.UnlessNull(ValueArgExtensions.LessThan, threshold, message);

    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="NullableValueArg{T}"/> object
    /// is less than or equal to a specified value according to <see cref="IComparable{T}"/> semantics.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T">The underlying type of the argument.</typeparam>
    /// <param name="this">The <see cref="NullableValueArg{T}"/> on which this method is called.</param>
    /// <param name="threshold">The value against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static NullableValueArg<T> LessThanOrEqualTo<T>(this NullableValueArg<T> @this, T threshold, string? message = null)
        where T : struct, IComparable<T>
        => @this.UnlessNull(ValueArgExtensions.LessThanOrEqualTo, threshold, message);

    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="NullableValueArg{T}"/> object
    /// is in a specified open range according to <see cref="IComparable{T}"/> semantics.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T">The underlying type of the argument.</typeparam>
    /// <param name="this">The <see cref="NullableValueArg{T}"/> on which this method is called.</param>
    /// <param name="minValue">The lower bound of the range against which the argument is tested.</param>
    /// <param name="maxValue">The upper bound of the range against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static NullableValueArg<T> InRange<T>(this NullableValueArg<T> @this, T minValue, T maxValue, string? message = null)
        where T : struct, IComparable<T>
        => @this.UnlessNull(ValueArgExtensions.InRange, minValue, maxValue, message);

    /// <summary>
    /// <para>Ensures that the argument (assumed to be a numeric value) represented by a <see cref="NullableValueArg{T}"/> object
    /// is greater than zero according to <see cref="IComparable{T}"/> semantics.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T">The underlying type of the argument.</typeparam>
    /// <param name="this">The <see cref="NullableValueArg{T}"/> on which this method is called.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static NullableValueArg<T> GreaterThanZero<T>(this NullableValueArg<T> @this, string? message = null)
        where T : unmanaged, IComparable<T>
        => @this.UnlessNull(ValueArgExtensions.GreaterThanZero, message);
}

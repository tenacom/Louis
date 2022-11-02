// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace Louis.ArgumentValidation;

[StackTraceHidden]
partial class ValueArgExtensions
{
    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="ValueArg{T}"/> object
    /// is greater than a specified value according to <see cref="IComparable{T}"/> semantics.</para>
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="this">The <see cref="ValueArg{T}"/> on which this method is called.</param>
    /// <param name="threshold">The value against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static ValueArg<T> GreaterThan<T>(this ValueArg<T> @this, T threshold, string? message = null)
        where T : struct, IComparable<T>
        => @this.Value.CompareTo(threshold) <= 0
            ? ArgHelper.ThrowArgumentOutOfRangeException(@this, message ?? $"Argument must be greater than {threshold}.")
            : @this;

    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="ValueArg{T}"/> object
    /// is greater than or equal to a specified value according to <see cref="IComparable{T}"/> semantics.</para>
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="this">The <see cref="ValueArg{T}"/> on which this method is called.</param>
    /// <param name="threshold">The value against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static ValueArg<T> GreaterThanOrEqualTo<T>(this ValueArg<T> @this, T threshold, string? message = null)
        where T : struct, IComparable<T>
        => @this.Value.CompareTo(threshold) < 0
            ? ArgHelper.ThrowArgumentOutOfRangeException(@this, message ?? $"Argument must be greater than or equal to {threshold}.")
            : @this;

    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="ValueArg{T}"/> object
    /// is less than a specified value according to <see cref="IComparable{T}"/> semantics.</para>
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="this">The <see cref="ValueArg{T}"/> on which this method is called.</param>
    /// <param name="threshold">The value against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static ValueArg<T> LessThan<T>(this ValueArg<T> @this, T threshold, string? message = null)
        where T : struct, IComparable<T>
        => @this.Value.CompareTo(threshold) >= 0
            ? ArgHelper.ThrowArgumentOutOfRangeException(@this, message ?? $"Argument must be less than {threshold}.")
            : @this;

    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="ValueArg{T}"/> object
    /// is less than or equal to a specified value according to <see cref="IComparable{T}"/> semantics.</para>
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="this">The <see cref="ValueArg{T}"/> on which this method is called.</param>
    /// <param name="threshold">The value against which the argument is tested.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static ValueArg<T> LessThanOrEqualTo<T>(this ValueArg<T> @this, T threshold, string? message = null)
        where T : struct, IComparable<T>
        => @this.Value.CompareTo(threshold) > 0
            ? ArgHelper.ThrowArgumentOutOfRangeException(@this, message ?? $"Argument cannot be greater than {threshold}.")
            : @this;

    /// <summary>
    /// <para>Ensures that the argument represented by a <see cref="ValueArg{T}"/> object
    /// is in a specified open range according to <see cref="IComparable{T}"/> semantics.</para>
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="this">The <see cref="ValueArg{T}"/> on which this method is called.</param>
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
    public static ValueArg<T> InRange<T>(this ValueArg<T> @this, T minValue, T maxValue, string? message = null)
        where T : struct, IComparable<T>
        => @this.Value.CompareTo(minValue) < 0 || @this.Value.CompareTo(maxValue) > 0
            ? ArgHelper.ThrowArgumentOutOfRangeException(@this, message ?? $"Argument must be between {minValue} and {maxValue}.")
            : @this;

    /// <summary>
    /// <para>Ensures that the argument (assumed to be a numeric value) represented by a <see cref="ValueArg{T}"/> object
    /// is greater than zero according to <see cref="IComparable{T}"/> semantics.</para>
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="this">The <see cref="ValueArg{T}"/> on which this method is called.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentOutOfRangeException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The argument does not satisfy the condition expressed by this method.</exception>
    public static ValueArg<T> GreaterThanZero<T>(this ValueArg<T> @this, string? message = null)
        where T : unmanaged, IComparable<T>
        => @this.Value.CompareTo(default) <= 0
            ? ArgHelper.ThrowArgumentOutOfRangeException(@this, message ?? "Argument must be greater than zero.")
            : @this;
}

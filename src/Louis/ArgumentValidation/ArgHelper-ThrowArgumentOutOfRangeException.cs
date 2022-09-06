// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Louis.ArgumentValidation;

partial class ArgHelper
{
    private const string DefaultArgumentOutOfRangExceptionFormat = "{@} is out of the allowed range.";

    /// <summary>
    /// Throws a new <see cref="ArgumentOutOfRangeException"/> with a message formatted
    /// using <see cref="FormatArgumentExceptionMessage"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument being validated.</typeparam>
    /// <param name="arg">The <see cref="Arg{T}"/> struct being used for evaluation.</param>
    /// <param name="format">The format of the exception message, as accepted by <see cref="FormatArgumentExceptionMessage"/>.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Always thrown with the formatted message
    /// and the <see cref="Arg{T}.Name">Name</see> of <paramref name="arg"/>.</exception>
    [DoesNotReturn]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static Arg<T> ThrowArgumentOutOfRangeException<T>(Arg<T> arg, string? format)
        where T : class
        => throw new ArgumentOutOfRangeException(
            arg.Name,
            FormatArgumentExceptionMessage(format ?? DefaultArgumentOutOfRangExceptionFormat, arg.Value));

    /// <summary>
    /// Throws a new <see cref="ArgumentOutOfRangeException"/> with a message formatted
    /// using <see cref="FormatArgumentExceptionMessage"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument being validated.</typeparam>
    /// <param name="arg">The <see cref="NullableArg{T}"/> struct being used for evaluation.</param>
    /// <param name="format">The format of the exception message, as accepted by <see cref="FormatArgumentExceptionMessage"/>.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Always thrown with the formatted message
    /// and the <see cref="NullableArg{T}.Name">Name</see> of <paramref name="arg"/>.</exception>
    [DoesNotReturn]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static NullableArg<T> ThrowArgumentOutOfRangeException<T>(NullableArg<T> arg, string? format)
        where T : class
        => throw new ArgumentOutOfRangeException(
            arg.Name,
            FormatArgumentExceptionMessage(format ?? DefaultArgumentOutOfRangExceptionFormat, arg.Value));

    /// <summary>
    /// Throws a new <see cref="ArgumentOutOfRangeException"/> with a message formatted
    /// using <see cref="FormatArgumentExceptionMessage"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument being validated.</typeparam>
    /// <param name="arg">The <see cref="ValueArg{T}"/> struct being used for evaluation.</param>
    /// <param name="format">The format of the exception message, as accepted by <see cref="FormatArgumentExceptionMessage"/>.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Always thrown with the formatted message
    /// and the <see cref="ValueArg{T}.Name">Name</see> of <paramref name="arg"/>.</exception>
    [DoesNotReturn]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static ValueArg<T> ThrowArgumentOutOfRangeException<T>(ValueArg<T> arg, string? format)
        where T : struct
        => throw new ArgumentOutOfRangeException(
            arg.Name,
            FormatArgumentExceptionMessage(format ?? DefaultArgumentOutOfRangExceptionFormat, arg.Value));

    /// <summary>
    /// Throws a new <see cref="ArgumentOutOfRangeException"/> with a message formatted
    /// using <see cref="FormatArgumentExceptionMessage"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument being validated.</typeparam>
    /// <param name="arg">The <see cref="NullableValueArg{T}"/> struct being used for evaluation.</param>
    /// <param name="format">The format of the exception message, as accepted by <see cref="FormatArgumentExceptionMessage"/>.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Always thrown with the formatted message
    /// and the <see cref="NullableValueArg{T}.Name">Name</see> of <paramref name="arg"/>.</exception>
    [DoesNotReturn]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static NullableValueArg<T> ThrowArgumentOutOfRangeException<T>(NullableValueArg<T> arg, string? format)
        where T : struct
        => throw new ArgumentOutOfRangeException(
            arg.Name,
            FormatArgumentExceptionMessage(format ?? DefaultArgumentOutOfRangExceptionFormat, arg.Value));
}

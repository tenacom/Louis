// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Louis.ArgumentValidation.Internal;
using Louis.Diagnostics;

namespace Louis.ArgumentValidation;

/// <summary>
/// Represents an argument to check, whose type is a value type (<see langword="struct"/>).
/// </summary>
/// <typeparam name="T">The type of the argument.</typeparam>
[StackTraceHidden]
public readonly ref struct ValueArg<T>
    where T : struct
{
    internal ValueArg(string name, T value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Gets the name of the argument represented by the current <see cref="Arg{T}"/> object.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// <para>Gets the argument represented by the current <see cref="ValueArg{T}"/> object.</para>
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Defines an implicit conversion of a <see cref="ValueArg{T}"/> to its <see cref="Value"/>.
    /// </summary>
    /// <param name="arg">The <see cref="ValueArg{T}"/> to convert.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CA2225 // Operator overloads have named alternates - A "ToT" method wouldn't make much sense.
    public static implicit operator T(ValueArg<T> arg) => arg.Value;
#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Checks that the argument represented by the current <see cref="ValueArg{T}"/> object
    /// verifies a given predicate, and throws <see cref="ArgumentException"/> if it does not.
    /// </summary>
    /// <param name="predicate">A function to test the argument for a condition.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the <see cref="ArgumentException"/>
    /// thrown if <paramref name="predicate"/> returns <see langword="false"/>.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns>The current <see cref="ValueArg{T}"/> object.</returns>
    /// <exception cref="InternalErrorException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="predicate"/> returned <see langword="false"/>.</exception>
    public ValueArg<T> Check(Predicate<T> predicate, string? message = null)
        => predicate is null ? throw ExceptionHelper.PredicateCannotBeNull()
            : predicate(Value) ? this
            : throw ArgHelper.MakeArgumentException(Name, Value, message);

    /// <summary>
    /// Checks that the argument represented by the current <see cref="ValueArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback,
    /// and throws <see cref="ArgumentException"/> if it does not.
    /// </summary>
    /// <param name="func">A function to test whether the argument satisfies one or more conditions
    /// and generate an appropriate exception message if it does not.</param>
    /// <returns>The current <see cref="ValueArg{T}"/> object.</returns>
    /// <exception cref="InternalErrorException"><paramref name="func"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="func"/> returned <see langword="false"/>.</exception>
    public ValueArg<T> Check(ArgumentCheckFunc<T> func)
        => func is null ? throw ExceptionHelper.CallbackCannotBeNull()
            : func(Value, out var message) ? this
            : throw ArgHelper.MakeArgumentException(Name, Value, message);
}

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

namespace Louis.ArgumentValidation;

/// <summary>
/// Represents an argument to check, whose value is a nullable value type (<see cref="Nullable{T}"/>).
/// </summary>
/// <typeparam name="T">The underlying type of the argument (e.g. <c>int</c> if the type of the argument is <c>int?</c>).</typeparam>
[StackTraceHidden]
public readonly ref partial struct NullableValueArg<T>
    where T : struct
{
    private readonly ValueArg<T> _valueArg;

    /// <summary>
    /// Initializes a new instance of the <see cref="NullableValueArg{T}"/> struct, representing an argument
    /// of a <see cref="Nullable{T}">nullable value type</see>.
    /// </summary>
    /// <param name="name">The name of the argument.
    /// This parameter should always be passed as <c><see langword="nameof"/>(argument)</c>.</param>
    /// <param name="value">The value of the argument.</param>
    public NullableValueArg(string name, T? value)
    {
        HasValue = value.HasValue;
        _valueArg = new(name, value.GetValueOrDefault());
    }

    /// <summary>
    /// Gets the name of the argument represented by the current <see cref="NullableValueArg{T}"/> object.
    /// </summary>
    public string Name => _valueArg.Name;

    /// <summary>
    /// Gets a value indicating whether the argument represented by the current <see cref="NullableValueArg{T}"/> object
    /// has a value.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the argument has a value; otherwise, <see langword="false"/>.
    /// </value>
    public bool HasValue { get; }

    /// <summary>
    /// Gets the <see cref="Nullable{T}">nullable</see> argument represented by the current <see cref="NullableValueArg{T}"/> object.
    /// </summary>
    public T? Value => HasValue ? _valueArg.Value : null;

    /// <summary>
    /// Defines an implicit conversion of a <see cref="NullableValueArg{T}"/> to its <see cref="Value"/>.
    /// </summary>
    /// <param name="arg">The <see cref="NullableValueArg{T}"/> to convert.</param>
#pragma warning disable CA2225 // Operator overloads have named alternates - A "ToT" method wouldn't make much sense.
    public static implicit operator T?(NullableValueArg<T> arg) => arg.Value;
#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Tries to retrieve the value of the argument represented by the current <see cref="NullableValueArg{T}"/> object
    /// and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="value">When this method returns <see langword="true"/>, contains the value of the argument.
    /// This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the argument has a value; otherwise, <see langword="false"/>.</returns>
    public bool TryGetValue(out T value)
    {
        value = _valueArg.Value;
        return HasValue;
    }

    /// <summary>
    /// <para>Retrieves the value of the argument represented by the current <see cref="NullableValueArg{T}"/> object,
    /// or <c>default(<typeparamref name="T"/>)</c> if the argument has no value.</para>
    /// </summary>
    /// <returns>
    /// If the argument has a value, the value; otherwise, <c>default(<typeparamref name="T"/>)</c>.
    /// </returns>
    public T GetValueOrDefault() => HasValue ? _valueArg.Value : default;

    /// <summary>
    /// <para>Retrieves the argument represented by the current <see cref="NullableValueArg{T}"/> object,
    /// or the specified default value if the argument is <see langword="null"/>.</para>
    /// </summary>
    /// <param name="defaultValue">The default value to return if the argument is <see langword="null"/>.</param>
    /// <returns>
    /// The argument, or <paramref name="defaultValue"/> if the argument is <see langword="null"/>.
    /// </returns>
    public T GetValueOrDefault(T defaultValue) => HasValue ? _valueArg.Value : defaultValue;
}

// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Louis.ArgumentValidation;

/// <summary>
/// Represents an argument to check, whose type is a nullable reference type.
/// </summary>
/// <typeparam name="T">The type of the argument.</typeparam>
[StackTraceHidden]
public readonly ref partial struct NullableArg<T>
    where T : class
{
    private readonly Arg<T> _arg;

    internal NullableArg(string name, T? value)
    {
        _arg = new(name, value!);
    }

    /// <summary>
    /// Gets the name of the argument represented by the current <see cref="NullableArg{T}"/> object.
    /// </summary>
    public string Name => _arg.Name;

    /// <summary>
    /// <para>Gets the argument represented by the current <see cref="NullableArg{T}"/> object.</para>
    /// </summary>
    public T? Value => (T?)_arg.Value;

    /// <summary>
    /// Defines an implicit conversion of a <see cref="NullableArg{T}"/> to its <see cref="Value"/>.
    /// </summary>
    /// <param name="arg">The <see cref="NullableArg{T}"/> to convert.</param>
#pragma warning disable CA2225 // Operator overloads have named alternates - A "ToT" method wouldn't make sense here.
    public static implicit operator T?(NullableArg<T> arg) => arg.Value;
#pragma warning restore CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Tries to retrieve the argument represented by the current <see cref="NullableArg{T}"/> object
    /// and returns a value that indicates whether the operation succeeded.
    /// </summary>
    /// <param name="value">When this method returns <see langword="true"/>, contains the argument.
    /// This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if the argument is not <see langword="null"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        value = (T?)_arg.Value;
        return value is not null;
    }
}

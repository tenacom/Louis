// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using Louis.Diagnostics;

namespace Louis.ArgumentValidation;

/// <summary>
/// <para>Provides helper methods for argument validation.</para>
/// <para>This class is intended for use by extension methods for the <see cref="Arg{T}"/>, <see cref="NullableArg{T}"/>,
/// <see cref="ValueArg{T}"/>, and <see cref="NullableValueArg{T}"/> structs. Using the methods in this class,
/// instead of throwing exceptions directly, makes validation extension methods smaller, faster when it counts
/// (i.e. when argument values are valid), and more likely to be inlined by the JIT.</para>
/// </summary>
/// <remarks>
/// <para>Validation extension methods usually return the same struct they are called on; this would be a
/// typical use case for the generic methods in the <see cref="Throw"/> class. Unfortunately, though,
/// <c>ref struct</c>s cannot be used as generic type parameters; hence the need for dedicated helper methods.</para>
/// </remarks>
public static partial class ArgHelper
{
}

// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace Louis.ArgumentValidation;

/// <summary>
/// Represents the method that defines a set of criteria, determines whether the specified value
/// meets those criteria, and if not sets a message specifying the reason why.
/// </summary>
/// <typeparam name="T">
/// <para>The type of the value to compare.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <param name="value">The value to compare against the criteria defined within the method
/// represented by this delegate.</param>
/// <param name="message">When this method returns <see langword="false"/>, contains the message to use to throw an <see cref="ArgumentException"/>.
/// This parameter is passed uninitialized.</param>
/// <returns><see langword="true"/> if <paramref name="value"/> meets the criteria defined within the method represented by this delegate;
/// otherwise, <see langword="false"/>.</returns>
/// <seealso cref="Arg{T}.Check(ArgumentCheckFunc{T})"/>
public delegate bool ArgumentCheckFunc<in T>(T value, [MaybeNullWhen(true)] out string message);

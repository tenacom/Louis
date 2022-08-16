// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

namespace Louis.Fluency;

#pragma warning disable SA1402 // File may only contain a single type

/// <summary>
/// Encapsulates a method that has one parameter and returns a value of the same type as the parameter.
/// </summary>
/// <typeparam name="T">The type of both the parameter and the result of the method that this delegate encapsulates.</typeparam>
/// <param name="obj">The parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed to the method.</returns>
public delegate T FluentAction<T>(T obj);

/// <summary>
/// Encapsulates a method that has two parameters and returns a value of the same type as the first parameter.
/// </summary>
/// <typeparam name="T">The type of both the first parameter and the result of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="T1">
/// <para>The type of the second parameter of the method that this delegate encapsulates.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
/// For more information about covariance and contravariance, see
/// <see href="https://docs.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance">Covariance and Contravariance in Generics</see>.</para>
/// </typeparam>
/// <param name="obj">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg">The second parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed as the first parameter to the method.</returns>
public delegate T FluentAction<T, T1>(T obj, in T1 arg);

/// <summary>
/// Encapsulates a method that has three parameter and returns a value of the same type as the first parameter.
/// </summary>
/// <typeparam name="T">The type of both the first parameter and the result of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="T1">
/// <para>The type of the second parameter of the method that this delegate encapsulates.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
/// For more information about covariance and contravariance, see
/// <see href="https://docs.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance">Covariance and Contravariance in Generics</see>.</para>
/// </typeparam>
/// <typeparam name="T2">
/// <para>The type of the third parameter of the method that this delegate encapsulates.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
/// For more information about covariance and contravariance, see
/// <see href="https://docs.microsoft.com/en-us/dotnet/standard/generics/covariance-and-contravariance">Covariance and Contravariance in Generics</see>.</para>
/// </typeparam>
/// <param name="obj">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg1">The second parameter of the method that this delegate encapsulates.</param>
/// <param name="arg2">The third parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed as the first parameter to the method.</returns>
public delegate T FluentAction<T, T1, T2>(T obj, in T1 arg1, in T2 arg2);

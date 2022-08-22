// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

namespace Louis.ArgumentValidation;

#pragma warning disable SA1402 // File may only contain a single type - https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3468

/// <summary>
/// Encapsulates a method that performs one or more checks on a <see cref="Arg{T}"/> and returns it.
/// </summary>
/// <typeparam name="T">The type of the argument represented by <paramref name="this"/>.</typeparam>
/// <param name="this">The <see cref="Arg{T}"/> on which the method is called.</param>
/// <returns><paramref name="this"/>, for chaining calls.</returns>
public delegate Arg<T> ArgCheckFunc<T>(Arg<T> @this)
    where T : class;

/// <summary>
/// Encapsulates a method that performs one or more checks on a <see cref="Arg{T}"/> and returns it.
/// </summary>
/// <typeparam name="T">The type of the argument represented by <paramref name="this"/>.</typeparam>
/// <typeparam name="T1">
/// <para>The type of the additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <param name="this">The <see cref="Arg{T}"/> on which the method is called.</param>
/// <param name="arg1">An additional parameter.</param>
/// <returns><paramref name="this"/>, for chaining calls.</returns>
public delegate Arg<T> ArgCheckFunc<T, in T1>(Arg<T> @this, T1 arg1)
    where T : class;

/// <summary>
/// Encapsulates a method that performs one or more checks on a <see cref="Arg{T}"/> and returns it.
/// </summary>
/// <typeparam name="T">The type of the argument represented by <paramref name="this"/>.</typeparam>
/// <typeparam name="T1">
/// <para>The type of the first additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <typeparam name="T2">
/// <para>The type of the second additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <param name="this">The <see cref="Arg{T}"/> on which the method is called.</param>
/// <param name="arg1">The first additional parameter.</param>
/// <param name="arg2">The second additional parameter.</param>
/// <returns><paramref name="this"/>, for chaining calls.</returns>
public delegate Arg<T> ArgCheckFunc<T, in T1, in T2>(Arg<T> @this, T1 arg1, T2 arg2)
    where T : class;

/// <summary>
/// Encapsulates a method that performs one or more checks on a <see cref="Arg{T}"/> and returns it.
/// </summary>
/// <typeparam name="T">The type of the argument represented by <paramref name="this"/>.</typeparam>
/// <typeparam name="T1">
/// <para>The type of the first additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <typeparam name="T2">
/// <para>The type of the second additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <typeparam name="T3">
/// <para>The type of the third additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <param name="this">The <see cref="Arg{T}"/> on which the method is called.</param>
/// <param name="arg1">The first additional parameter.</param>
/// <param name="arg2">The second additional parameter.</param>
/// <param name="arg3">The third additional parameter.</param>
/// <returns><paramref name="this"/>, for chaining calls.</returns>
public delegate Arg<T> ArgCheckFunc<T, in T1, in T2, in T3>(Arg<T> @this, T1 arg1, T2 arg2, T3 arg3)
    where T : class;

/// <summary>
/// Encapsulates a method that performs one or more checks on a <see cref="Arg{T}"/> and returns it.
/// </summary>
/// <typeparam name="T">The type of the argument represented by <paramref name="this"/>.</typeparam>
/// <typeparam name="T1">
/// <para>The type of the first additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <typeparam name="T2">
/// <para>The type of the second additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <typeparam name="T3">
/// <para>The type of the third additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <typeparam name="T4">
/// <para>The type of the fourth additional parameter to the method.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <param name="this">The <see cref="Arg{T}"/> on which the method is called.</param>
/// <param name="arg1">The first additional parameter.</param>
/// <param name="arg2">The second additional parameter.</param>
/// <param name="arg3">The third additional parameter.</param>
/// <param name="arg4">The fourth additional parameter.</param>
/// <returns><paramref name="this"/>, for chaining calls.</returns>
public delegate Arg<T> ArgCheckFunc<T, in T1, in T2, in T3, in T4>(Arg<T> @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    where T : class;

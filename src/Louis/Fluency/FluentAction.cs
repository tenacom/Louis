// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

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
/// <typeparam name="TArg">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
/// <param name="obj">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg">The second parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed as the first parameter to the method.</returns>
public delegate T FluentAction<T, TArg>(T obj, TArg arg);

/// <summary>
/// Encapsulates a method that has three parameter and returns a value of the same type as the first parameter.
/// </summary>
/// <typeparam name="T">The type of both the first parameter and the result of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg1">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg2">The type of the third parameter of the method that this delegate encapsulates.</typeparam>
/// <param name="obj">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg1">The second parameter of the method that this delegate encapsulates.</param>
/// <param name="arg2">The third parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed as the first parameter to the method.</returns>
public delegate T FluentAction<T, TArg1, TArg2>(T obj, TArg1 arg1, TArg2 arg2);

/// <summary>
/// Encapsulates a method that has four parameter and returns a value of the same type as the first parameter.
/// </summary>
/// <typeparam name="T">The type of both the first parameter and the result of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg1">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg2">The type of the third parameter of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg3">The type of the fourth parameter of the method that this delegate encapsulates.</typeparam>
/// <param name="obj">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg1">The second parameter of the method that this delegate encapsulates.</param>
/// <param name="arg2">The third parameter of the method that this delegate encapsulates.</param>
/// <param name="arg3">The fourth parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed as the first parameter to the method.</returns>
public delegate T FluentAction<T, TArg1, TArg2, TArg3>(T obj, TArg1 arg1, TArg2 arg2, TArg3 arg3);

/// <summary>
/// Encapsulates a method that has four parameter and returns a value of the same type as the first parameter.
/// </summary>
/// <typeparam name="T">The type of both the first parameter and the result of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg1">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg2">The type of the third parameter of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg3">The type of the fourth parameter of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="TArg4">The type of the fifth parameter of the method that this delegate encapsulates.</typeparam>
/// <param name="obj">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg1">The second parameter of the method that this delegate encapsulates.</param>
/// <param name="arg2">The third parameter of the method that this delegate encapsulates.</param>
/// <param name="arg3">The fourth parameter of the method that this delegate encapsulates.</param>
/// <param name="arg4">The fifth parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed as the first parameter to the method.</returns>
public delegate T FluentAction<T, TArg1, TArg2, TArg3, TArg4>(T obj, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4);

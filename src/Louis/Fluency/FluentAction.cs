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
/// <typeparam name="T1">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
/// <param name="obj">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg">The second parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed as the first parameter to the method.</returns>
public delegate T FluentAction<T, T1>(T obj, T1 arg);

/// <summary>
/// Encapsulates a method that has three parameter and returns a value of the same type as the first parameter.
/// </summary>
/// <typeparam name="T">The type of both the first parameter and the result of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="T1">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
/// <typeparam name="T2">The type of the third parameter of the method that this delegate encapsulates.</typeparam>
/// <param name="obj">The first parameter of the method that this delegate encapsulates.</param>
/// <param name="arg1">The second parameter of the method that this delegate encapsulates.</param>
/// <param name="arg2">The third parameter of the method that this delegate encapsulates.</param>
/// <returns>The return value of the method that this delegate encapsulates.
/// This is usually (but not obligatorily), at least is <typeparamref name="T"/> is a reference type,
/// the same reference that was passed as the first parameter to the method.</returns>
public delegate T FluentAction<T, T1, T2>(T obj, T1 arg1, T2 arg2);

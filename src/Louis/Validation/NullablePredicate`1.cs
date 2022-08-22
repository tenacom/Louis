// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

namespace Louis.ArgumentValidation;

/// <summary>
/// Represents the method that defines a set of criteria and determines whether
/// the specified, potentially <see langword="null"/>, value meets those criteria.
/// </summary>
/// <typeparam name="T">
/// <para>The type of the value to compare.</para>
/// <para>This type parameter is contravariant. That is, you can use either the type you specified
/// or any type that is less derived.</para>
/// </typeparam>
/// <param name="hasValue"><see langword="true"/> if <paramref name="value"/> is not <see langword="null"/>;
/// otherwise, <see langword="false"/>.</param>
/// <param name="value">The value to compare against the criteria defined within the method
/// represented by this delegate.</param>
/// <returns><see langword="true"/> if the combination of <paramref name="hasValue"/> and <paramref name="value"/>
/// meets the criteria defined within the method represented by this delegate;
/// otherwise, <see langword="false"/>.</returns>
/// <seealso cref="NullableArg{T}.Check(NullablePredicate{T},string?)"/>
public delegate bool NullablePredicate<in T>(bool hasValue, T value);

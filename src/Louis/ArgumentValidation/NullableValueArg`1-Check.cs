// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using Louis.Diagnostics;

namespace Louis.ArgumentValidation;

partial struct NullableValueArg<T>
{
    /// <summary>
    /// <para>Checks that the argument represented by the current <see cref="NullableValueArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback,
    /// and throws <see cref="ArgumentException"/> if it does not.</para>
    /// <para>Unlike <see cref="CheckUnlessNull(Predicate{T},string?)"/>, this method will check
    /// <see langword="null"/> as well as non-<see langword="null"/> arguments.</para>
    /// </summary>
    /// <param name="predicate">A function to test the argument for a condition.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentException"/> thrown if the argument is not <see langword="null"/>
    /// and <paramref name="predicate"/> returns <see langword="false"/>.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns>The current <see cref="NullableValueArg{T}"/> object.</returns>
    /// <exception cref="InternalErrorException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="predicate"/> returned <see langword="false"/>.</exception>
    public NullableValueArg<T> Check(NullablePredicate<T> predicate, string? message = null)
        => predicate is null ? ArgHelper.ThrowPredicateCannotBeNull(this)
            : predicate(HasValue, _valueArg.Value) ? this
            : ArgHelper.ThrowArgumentException(this, message);

    /// <summary>
    /// <para>Checks that the argument represented by the current <see cref="NullableValueArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback,
    /// and throws <see cref="ArgumentException"/> if it does not.</para>
    /// <para>Unlike <see cref="CheckUnlessNull(ArgumentCheckFunc{T})"/>, this method will check
    /// <see langword="null"/> as well as non-<see langword="null"/> arguments.</para>
    /// </summary>
    /// <param name="callback">A function to test whether the argument satisfies one or more conditions
    /// and generate an appropriate exception message if it does not.</param>
    /// <returns>The current <see cref="NullableValueArg{T}"/> object.</returns>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="callback"/> returned <see langword="false"/>.</exception>
    public NullableValueArg<T> Check(NullableArgumentCheckFunc<T> callback)
        => callback is null ? ArgHelper.ThrowCallbackCannotBeNull(this)
            : callback(HasValue, _valueArg.Value, out var message) ? this
            : ArgHelper.ThrowArgumentException(this, message);
}

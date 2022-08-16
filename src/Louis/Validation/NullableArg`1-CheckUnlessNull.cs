// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using Louis.ArgumentValidation.Internal;
using Louis.Diagnostics;

namespace Louis.ArgumentValidation;

partial struct NullableArg<T>
{
    /// <summary>
    /// <para>Checks that the argument represented by the current <see cref="NullableArg{T}"/> object verifies a given predicate,
    /// and throws <see cref="ArgumentException"/> if it does not.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <param name="predicate">A function to test the argument for a condition.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the
    /// <see cref="ArgumentException"/> thrown if the argument is not <see langword="null"/>
    /// and <paramref name="predicate"/> returns <see langword="false"/>.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns>The current <see cref="NullableArg{T}"/> object.</returns>
    /// <exception cref="InternalErrorException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The argument is not <see langword="null"/>
    /// and <paramref name="predicate"/> returned <see langword="false"/>.</exception>
    public NullableArg<T> CheckUnlessNull(Predicate<T> predicate, string? message = null)
        => predicate is null ? throw ExceptionHelper.PredicateCannotBeNull()
            : Value is null ? this
            : predicate(_arg.Value) ? this
            : throw ArgHelper.MakeArgumentException(_arg.Name, _arg.Value, message);

    /// <summary>
    /// <para>Checks that the argument represented by the current <see cref="NullableArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback,
    /// and throws <see cref="ArgumentException"/> if it does not.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <param name="callback">A function to test whether the argument satisfies one or more conditions
    /// and generate an appropriate exception message if it does not.</param>
    /// <returns>The current <see cref="NullableArg{T}"/> object.</returns>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">The argument is not <see langword="null"/>
    /// and <paramref name="callback"/> returned <see langword="false"/>.</exception>
    public NullableArg<T> CheckUnlessNull(ArgumentCheckFunc<T> callback)
        => callback is null ? throw ExceptionHelper.CallbackCannotBeNull()
            : Value is null ? this
            : callback(_arg.Value, out var message) ? this
            : throw ArgHelper.MakeArgumentException(_arg.Name, _arg.Value, message);
}

﻿// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using Louis.ArgumentValidation.Internal;
using Louis.Diagnostics;

namespace Louis.ArgumentValidation;

partial struct NullableArg<T>
{
    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <param name="callback">A function that ensures that the argument satisfies one or more conditions.</param>
    /// <returns>The current <see cref="NullableArg{T}"/> object.</returns>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableArg<T> UnlessNull(ArgCheckFunc<T> callback)
    {
        if (callback is null)
        {
            throw ExceptionHelper.CallbackCannotBeNull();
        }

        if (Value is null)
        {
            _ = callback(_arg);
        }

        return this;
    }

    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T1">
    /// <para>The type of the additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <param name="callback">A function that ensures that the argument satisfies one or more conditions.</param>
    /// <returns>The current <see cref="NullableArg{T}"/> object.</returns>
    /// <param name="arg1">An additional parameter passed to <paramref name="callback"/>.</param>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableArg<T> UnlessNull<T1>(ArgCheckFunc<T, T1> callback, T1 arg1)
    {
        if (callback is null)
        {
            throw ExceptionHelper.CallbackCannotBeNull();
        }

        if (Value is null)
        {
            _ = callback(_arg, arg1);
        }

        return this;
    }

    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T1">
    /// <para>The type of the first additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <typeparam name="T2">
    /// <para>The type of the second additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <param name="callback">A function that ensures that the argument satisfies one or more conditions.</param>
    /// <returns>The current <see cref="NullableArg{T}"/> object.</returns>
    /// <param name="arg1">The first additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg2">The second additional parameter passed to <paramref name="callback"/>.</param>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableArg<T> UnlessNull<T1, T2>(ArgCheckFunc<T, T1, T2> callback, T1 arg1, T2 arg2)
    {
        if (callback is null)
        {
            throw ExceptionHelper.CallbackCannotBeNull();
        }

        if (Value is null)
        {
            _ = callback(_arg, arg1, arg2);
        }

        return this;
    }

    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T1">
    /// <para>The type of the first additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <typeparam name="T2">
    /// <para>The type of the second additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <typeparam name="T3">
    /// <para>The type of the third additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <param name="callback">A function that ensures that the argument satisfies one or more conditions.</param>
    /// <returns>The current <see cref="NullableArg{T}"/> object.</returns>
    /// <param name="arg1">The first additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg2">The second additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg3">The third additional parameter passed to <paramref name="callback"/>.</param>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableArg<T> UnlessNull<T1, T2, T3>(ArgCheckFunc<T, T1, T2, T3> callback, T1 arg1, T2 arg2, T3 arg3)
    {
        if (callback is null)
        {
            throw ExceptionHelper.CallbackCannotBeNull();
        }

        if (Value is null)
        {
            _ = callback(_arg, arg1, arg2, arg3);
        }

        return this;
    }

    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <typeparam name="T1">
    /// <para>The type of the first additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <typeparam name="T2">
    /// <para>The type of the second additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <typeparam name="T3">
    /// <para>The type of the third additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <typeparam name="T4">
    /// <para>The type of the fourth additional parameter passed to <paramref name="callback"/>.</para>
    /// <para>This type parameter is contravariant. That is, you can use either the type you specified
    /// or any type that is less derived.</para>
    /// </typeparam>
    /// <param name="callback">A function that ensures that the argument satisfies one or more conditions.</param>
    /// <returns>The current <see cref="NullableArg{T}"/> object.</returns>
    /// <param name="arg1">The first additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg2">The second additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg3">The third additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg4">The fourth additional parameter passed to <paramref name="callback"/>.</param>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableArg<T> UnlessNull<T1, T2, T3, T4>(ArgCheckFunc<T, T1, T2, T3, T4> callback, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (callback is null)
        {
            throw ExceptionHelper.CallbackCannotBeNull();
        }

        if (Value is null)
        {
            _ = callback(_arg, arg1, arg2, arg3, arg4);
        }

        return this;
    }
}

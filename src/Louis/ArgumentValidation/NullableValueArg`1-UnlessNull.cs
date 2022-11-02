// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using Louis.Diagnostics;

namespace Louis.ArgumentValidation;

partial struct NullableValueArg<T>
{
    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableValueArg{T}"/> object
    /// verifies one ore more conditions as determined by a given callback.</para>
    /// <para>This method does not check the argument if it is <see langword="null"/>;
    /// instead, it returns immediately.</para>
    /// </summary>
    /// <param name="callback">A function that ensures that the argument satisfies one or more conditions.</param>
    /// <returns>The current <see cref="NullableValueArg{T}"/> object.</returns>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableValueArg<T> UnlessNull(ValueArgCheckFunc<T> callback)
    {
        if (callback is null)
        {
            return ArgHelper.ThrowCallbackCannotBeNull(this);
        }

        if (HasValue)
        {
            _ = callback(_valueArg);
        }

        return this;
    }

    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableValueArg{T}"/> object
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
    /// <returns>The current <see cref="NullableValueArg{T}"/> object.</returns>
    /// <param name="arg1">An additional parameter passed to <paramref name="callback"/>.</param>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableValueArg<T> UnlessNull<T1>(ValueArgCheckFunc<T, T1> callback, T1 arg1)
    {
        if (callback is null)
        {
            return ArgHelper.ThrowCallbackCannotBeNull(this);
        }

        if (HasValue)
        {
            _ = callback(_valueArg, arg1);
        }

        return this;
    }

    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableValueArg{T}"/> object
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
    /// <returns>The current <see cref="NullableValueArg{T}"/> object.</returns>
    /// <param name="arg1">The first additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg2">The second additional parameter passed to <paramref name="callback"/>.</param>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableValueArg<T> UnlessNull<T1, T2>(ValueArgCheckFunc<T, T1, T2> callback, T1 arg1, T2 arg2)
    {
        if (callback is null)
        {
            return ArgHelper.ThrowCallbackCannotBeNull(this);
        }

        if (HasValue)
        {
            _ = callback(_valueArg, arg1, arg2);
        }

        return this;
    }

    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableValueArg{T}"/> object
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
    /// <returns>The current <see cref="NullableValueArg{T}"/> object.</returns>
    /// <param name="arg1">The first additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg2">The second additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg3">The third additional parameter passed to <paramref name="callback"/>.</param>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableValueArg<T> UnlessNull<T1, T2, T3>(ValueArgCheckFunc<T, T1, T2, T3> callback, T1 arg1, T2 arg2, T3 arg3)
    {
        if (callback is null)
        {
            return ArgHelper.ThrowCallbackCannotBeNull(this);
        }

        if (HasValue)
        {
            _ = callback(_valueArg, arg1, arg2, arg3);
        }

        return this;
    }

    /// <summary>
    /// <para>Ensures that the argument represented by the current <see cref="NullableValueArg{T}"/> object
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
    /// <returns>The current <see cref="NullableValueArg{T}"/> object.</returns>
    /// <param name="arg1">The first additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg2">The second additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg3">The third additional parameter passed to <paramref name="callback"/>.</param>
    /// <param name="arg4">The fourth additional parameter passed to <paramref name="callback"/>.</param>
    /// <exception cref="InternalErrorException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public NullableValueArg<T> UnlessNull<T1, T2, T3, T4>(ValueArgCheckFunc<T, T1, T2, T3, T4> callback, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (callback is null)
        {
            return ArgHelper.ThrowCallbackCannotBeNull(this);
        }

        if (HasValue)
        {
            _ = callback(_valueArg, arg1, arg2, arg3, arg4);
        }

        return this;
    }
}

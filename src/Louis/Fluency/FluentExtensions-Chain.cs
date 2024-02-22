// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;

namespace Louis.Fluency;

partial class FluentExtensions
{
    /// <summary>
    /// Invokes a chainable method on an object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="func">The chainable method to invoke on <paramref name="this"/>.</param>
    /// <returns>The reference to <paramref name="this"/> returned by <paramref name="func"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="func"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Chain<T>(this T @this, FluentAction<T> func)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(func);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return func(@this);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Invokes a chainable method on an object and an additional argument.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TArg">The type of the additional parameter to <paramref name="func"/>.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="arg">The additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="func">The chainable method to invoke on <paramref name="this"/>.</param>
    /// <returns>The reference to <paramref name="this"/> returned by <paramref name="func"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="func"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Chain<T, TArg>(this T @this, TArg arg, FluentAction<T, TArg> func)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(func);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return func(@this, arg);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Invokes a chainable method on an object and additional arguments.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TArg1">The type of the first additional parameter to <paramref name="func"/>.</typeparam>
    /// <typeparam name="TArg2">The type of the second additional parameter to <paramref name="func"/>.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="arg1">The First additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="arg2">The second additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="func">The chainable method to invoke on <paramref name="this"/>.</param>
    /// <returns>The reference to <paramref name="this"/> returned by <paramref name="func"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="func"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Chain<T, TArg1, TArg2>(this T @this, TArg1 arg1, TArg2 arg2, FluentAction<T, TArg1, TArg2> func)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(func);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return func(@this, arg1, arg2);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Invokes a chainable method on an object and additional arguments.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TArg1">The type of the first additional parameter to <paramref name="func"/>.</typeparam>
    /// <typeparam name="TArg2">The type of the second additional parameter to <paramref name="func"/>.</typeparam>
    /// <typeparam name="TArg3">The type of the third additional parameter to <paramref name="func"/>.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="arg1">The First additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="arg2">The second additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="arg3">The third additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="func">The chainable method to invoke on <paramref name="this"/>.</param>
    /// <returns>The reference to <paramref name="this"/> returned by <paramref name="func"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="func"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Chain<T, TArg1, TArg2, TArg3>(this T @this, TArg1 arg1, TArg2 arg2, TArg3 arg3, FluentAction<T, TArg1, TArg2, TArg3> func)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(func);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return func(@this, arg1, arg2, arg3);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Invokes a chainable method on an object and additional arguments.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TArg1">The type of the first additional parameter to <paramref name="func"/>.</typeparam>
    /// <typeparam name="TArg2">The type of the second additional parameter to <paramref name="func"/>.</typeparam>
    /// <typeparam name="TArg3">The type of the third additional parameter to <paramref name="func"/>.</typeparam>
    /// <typeparam name="TArg4">The type of the fourth additional parameter to <paramref name="func"/>.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="arg1">The First additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="arg2">The second additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="arg3">The third additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="arg4">The fourth additional argument to pass to <paramref name="func"/>.</param>
    /// <param name="func">The chainable method to invoke on <paramref name="this"/>.</param>
    /// <returns>The reference to <paramref name="this"/> returned by <paramref name="func"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="func"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Chain<T, TArg1, TArg2, TArg3, TArg4>(this T @this, TArg1 arg1, TArg2 arg2, TArg3 arg3, TArg4 arg4, FluentAction<T, TArg1, TArg2, TArg3, TArg4> func)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(func);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return func(@this, arg1, arg2, arg3, arg4);
#pragma warning restore CA1062 // Validate arguments of public methods
    }
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;

namespace Louis.Fluency;

partial class FluentExtensions
{
    /// <summary>
    /// Invokes an action on an object if a nullable value is not <see langword="null"/>, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="T1">The type of the additional argument to pass to <paramref name="then"/>.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="arg">The additional argument to pass to <paramref name="then"/>.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> and <paramref name="arg"/>if <paramref name="arg "/> is not <see langword="null"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="then"/>, if called, returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T IfNotNull<T, T1>(this T @this, T1? arg, FluentAction<T, T1> then)
        where T1 : struct
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return arg is null ? @this : then(@this, arg.Value);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Invokes an action on an object if a nullable value is not <see langword="null"/>, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="T1">The type of the additional argument to pass to <paramref name="then"/>.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="arg">The additional argument to pass to <paramref name="then"/>.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> and <paramref name="arg"/>if <paramref name="arg "/> is not <see langword="null"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="then"/>, if called, returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T IfNotNull<T, T1>(this T @this, T1? arg, Action<T, T1> then)
        where T1 : struct
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);

        if (arg is not null)
        {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
            then(@this, arg.Value);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        return @this;
    }
}

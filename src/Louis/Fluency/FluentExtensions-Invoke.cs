// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;

namespace Louis.Fluency;

partial class FluentExtensions
{
    /// <summary>
    /// Invokes an action on an object and returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="action">The action to perform on <paramref name="this"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="action"/> returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="action"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Invoke<T>(this T @this, Action<T> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);

        action(@this);
        return @this;
    }
}

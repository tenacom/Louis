// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text;
using CommunityToolkit.Diagnostics;

namespace Louis.Text;

partial class StringBuilderExtensions
{
    /// <summary>
    /// Appends the result of a method to the end of this builder.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="provider">The method to call.</param>
    /// <returns>A reference to <paramref name="this"/> after the operation is completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="provider"/> is <see langword="null"/>.</para>
    /// </exception>
    public static StringBuilder AppendResult(this StringBuilder @this, Func<string?> provider)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(provider);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return @this.Append(provider());
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Appends the result of a method to the end of this builder.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="func">The method to call.</param>
    /// <returns>A reference to <paramref name="this"/> after the operation is completed.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="func"/> is <see langword="null"/>.</para>
    /// </exception>
    public static StringBuilder AppendResult(this StringBuilder @this, ReadOnlySpanFunc<char> func)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(func);

#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
        return @this.Append(func());
#else
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return @this.Append(func().ToString());
#pragma warning restore CA1062 // Validate arguments of public methods
#endif
    }
}

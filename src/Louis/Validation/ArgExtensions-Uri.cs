// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;

namespace Louis.ArgumentValidation;

partial class ArgExtensions
{
    /// <summary>
    /// <para>Ensures that the argument represented by an <see cref="Arg{T}">Arg&lt;Uri&gt;</see> object
    /// is an absolute URI.</para>
    /// </summary>
    /// <param name="this">The <see cref="Arg{T}">Arg&lt;Uri&gt;</see> on which this method is called.</param>
    /// <param name="message">
    /// <para>An optional message that will be used to construct the <see cref="ArgumentException"/> thrown if the argument
    /// does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentException">
    /// <para>The argument represented by <paramref name="this"/> is not an absolute URI.</para>
    /// </exception>
    public static Arg<Uri> AbsoluteUri(this Arg<Uri> @this, string? message = null)
        => @this.Value.IsAbsoluteUri ? @this
            : throw ArgHelper.MakeArgumentException(@this.Name, @this.Value, message ?? "Argument should be an absolute URI.");

    /// <summary>
    /// <para>Ensures that the argument represented by an <see cref="Arg{T}">Arg&lt;Uri&gt;</see> object
    /// is an URI whose scheme is either <c>http</c> or <c>https</c>.</para>
    /// </summary>
    /// <param name="this">The <see cref="Arg{T}">Arg&lt;Uri&gt;</see> on which this method is called.</param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentException">
    /// <para>The argument represented by <paramref name="this"/> has a scheme that is
    /// neither <c>http</c> nor <c>https</c>.</para>
    /// </exception>
    public static Arg<Uri> HttpOrHttpsUri(this Arg<Uri> @this)
        => @this.Value.Scheme == Uri.UriSchemeHttp || @this.Value.Scheme == Uri.UriSchemeHttps ? @this
            : throw new ArgumentException("Argument should be an HTTP or HTTPS URI.", @this.Name);
}

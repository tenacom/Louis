// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
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
    /// <param name="format">
    /// <para>An optional format used to construct the message of the <see cref="ArgumentException"/>
    /// thrown if the argument does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is omitted or <see langword="null"/>, a default format will be used.</para>
    /// <para>The actual exception message will be constructed using <see cref="ArgHelper.FormatArgumentExceptionMessage"/>.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentException">
    /// <para>The argument represented by <paramref name="this"/> is not an absolute URI.</para>
    /// </exception>
    public static Arg<Uri> AbsoluteUri(this Arg<Uri> @this, string? format = null)
        => @this.Value.IsAbsoluteUri ? @this
            : ArgHelper.ThrowArgumentException(@this, format ?? "{@} is not an absolute URI.");

    /// <summary>
    /// <para>Ensures that the argument represented by an <see cref="Arg{T}">Arg&lt;Uri&gt;</see> object
    /// is an URI whose scheme is either <c>http</c> or <c>https</c>.</para>
    /// </summary>
    /// <param name="this">The <see cref="Arg{T}">Arg&lt;Uri&gt;</see> on which this method is called.</param>
    /// <param name="format">
    /// <para>An optional format used to construct the message of the <see cref="ArgumentException"/>
    /// thrown if the argument does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is omitted or <see langword="null"/>, a default format will be used.</para>
    /// <para>The actual exception message will be constructed using <see cref="ArgHelper.FormatArgumentExceptionMessage"/>.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentException">
    /// <para>The argument represented by <paramref name="this"/> has a scheme that is
    /// neither <c>http</c> nor <c>https</c>.</para>
    /// </exception>
    public static Arg<Uri> HttpOrHttpsUri(this Arg<Uri> @this, string? format = null)
        => @this.Value.Scheme == Uri.UriSchemeHttp || @this.Value.Scheme == Uri.UriSchemeHttps ? @this
            : ArgHelper.ThrowArgumentException(@this, format ?? "{@} is not an HTTP or HTTPS URI.");
}

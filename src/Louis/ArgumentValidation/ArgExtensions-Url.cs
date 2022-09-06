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

partial class ArgExtensions
{
    /// <summary>
    /// Ensures that the argument represented by an <see cref="Arg{T}">Arg&lt;string&gt;</see> object
    /// is an absolute URL and converts it to an <see cref="Uri"/> object.
    /// </summary>
    /// <param name="this">The <see cref="Arg{T}">Arg&lt;string&gt;</see> on which this method is called.</param>
    /// <param name="uri">When this method returns, contains a newly-created <see cref="Uri"/>.
    /// This parameter is passed uninitialized.</param>
    /// <param name="format">
    /// <para>An optional format used to construct the message of the <see cref="ArgumentException"/>
    /// thrown if the argument does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is omitted or <see langword="null"/>, a default format will be used.</para>
    /// <para>The actual exception message will be constructed using <see cref="ArgHelper.FormatArgumentExceptionMessage"/>.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentException">The argument represented by <paramref name="this"/> is not a valid URL.</exception>
    public static Arg<string> Url(this Arg<string> @this, out Uri uri, string? format = null) => Url(@this, out uri, false, format);

    /// <summary>
    /// Ensures that the argument represented by an <see cref="Arg{T}">Arg&lt;string&gt;</see> object
    /// is an absolute URL and converts it to an <see cref="Uri"/> object.
    /// </summary>
    /// <param name="this">The <see cref="Arg{T}">Arg&lt;string&gt;</see> on which this method is called.</param>
    /// <param name="uri">When this method returns, contains a newly-created <see cref="Uri"/>.
    /// This parameter is passed uninitialized.</param>
    /// <param name="enforceHttp"><see langword="true"/> to ensure that, if the argument represented by <paramref name="this"/> is an absolute URL,
    /// its scheme is either <c>http</c> or <c>https</c>; <see langword="false"/> to not check the scheme.</param>
    /// <param name="format">
    /// <para>An optional format used to construct the message of the <see cref="ArgumentException"/>
    /// thrown if the argument does not satisfy the condition expressed by this method.</para>
    /// <para>If this parameter is omitted or <see langword="null"/>, a default format will be used.</para>
    /// <para>The actual exception message will be constructed using <see cref="ArgHelper.FormatArgumentExceptionMessage"/>.</para>
    /// </param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentException">
    /// <para>The argument represented by <paramref name="this"/> is not a valid URL.</para>
    /// <para>- or -</para>
    /// <para><paramref name="enforceHttp"/> is <see langword="true"/>,
    /// and the argument represented by <paramref name="this"/> has a scheme that is neither <c>http</c> nor <c>https</c>.</para>
    /// </exception>
    public static Arg<string> Url(this Arg<string> @this, out Uri uri, bool enforceHttp, string? format = null)
    {
#pragma warning disable CS8601 // Possible null reference assignment. - If Uri.TryCreate fails, this method throws; the caller will never see uri == null.
        if (!Uri.TryCreate(@this.Value, UriKind.RelativeOrAbsolute, out uri))
        {
            return ArgHelper.ThrowArgumentException(@this, "{@} is not a valid URL.");
        }
#pragma warning restore CS8601 // Possible null reference assignment.

        if (enforceHttp && uri.IsAbsoluteUri && uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            return ArgHelper.ThrowArgumentException(@this, "{@} is not a valid HTTP or HTTPS URL.");
        }

        return @this;
    }

    /// <summary>
    /// Ensures that the argument represented by an <see cref="Arg{T}">Arg&lt;string&gt;</see> object
    /// is either an absolute URL, or a URL relative to the given <paramref name="baseUri"/>,
    /// and converts it to an <see cref="Uri"/> object.
    /// </summary>
    /// <param name="this">The <see cref="Arg{T}">Arg&lt;string&gt;</see> on which this method is called.</param>
    /// <param name="baseUri">The base URI for relative URLs.</param>
    /// <param name="uri">When this method returns, contains a newly-created <see cref="Uri"/>.
    /// This parameter is passed uninitialized.</param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentException">The argument represented by <paramref name="this"/> is not a valid URL.</exception>
    public static Arg<string> Url(this Arg<string> @this, Uri baseUri, out Uri uri) => Url(@this, baseUri, out uri, false);

    /// <summary>
    /// Ensures that the argument represented by an <see cref="Arg{T}">Arg&lt;string&gt;</see> object
    /// is either an absolute URL, or a URL relative to the given <paramref name="baseUri"/>,
    /// and converts it to an <see cref="Uri"/> object.
    /// </summary>
    /// <param name="this">The <see cref="Arg{T}">Arg&lt;string&gt;</see> on which this method is called.</param>
    /// <param name="baseUri">The base URI for relative URLs.</param>
    /// <param name="uri">When this method returns, contains a newly-created <see cref="Uri"/>.
    /// This parameter is passed uninitialized.</param>
    /// <param name="enforceHttp"><see langword="true"/> to ensure that the scheme of the resulting URI scheme is either <c>http</c> or <c>https</c>;
    /// <see langword="false"/> to not check the scheme.</param>
    /// <returns><paramref name="this"/>, for chaining calls.</returns>
    /// <exception cref="ArgumentException">
    /// <para>The argument represented by <paramref name="this"/> is not a valid URL.</para>
    /// <para>- or -</para>
    /// <para><paramref name="enforceHttp"/> is <see langword="true"/>,
    /// and the combination of <paramref name="baseUri"/> and the argument represented by <paramref name="this"/>
    /// has a scheme that is neither <c>http</c> nor <c>https</c>.</para>
    /// </exception>
    public static Arg<string> Url(this Arg<string> @this, Uri baseUri, out Uri uri, bool enforceHttp)
    {
        if (baseUri is null || !baseUri.IsAbsoluteUri)
        {
            throw SelfCheck.Failure("A base URI must be an absolute URI.");
        }

        if (!Uri.TryCreate(@this.Value, UriKind.RelativeOrAbsolute, out var relativeUri))
        {
            throw new ArgumentException("Argument should be a valid URL.", @this.Name);
        }

        if (!Uri.TryCreate(baseUri, relativeUri, out var createdUri))
        {
            throw new ArgumentException("Argument should be a valid URL.", @this.Name);
        }

        if (enforceHttp && createdUri.IsAbsoluteUri && createdUri.Scheme != Uri.UriSchemeHttp && createdUri.Scheme != Uri.UriSchemeHttps)
        {
            throw new ArgumentException("Argument should be a valid HTTP or HTTPS URL.", @this.Name);
        }

        uri = createdUri;
        return @this;
    }
}

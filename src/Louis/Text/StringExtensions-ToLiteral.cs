// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Text;
using Louis.Text.Internal;

namespace Louis.Text;

partial class StringExtensions
{
    /// <summary>
    /// Builds and returns a string representing a given string as a C#
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#quoted-string-literals">quoted string literal</see>.
    /// </summary>
    /// <param name="this">The <see langword="string"/> on which this method is called.</param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <seealso cref="ToVerbatimLiteral"/>
    /// <seealso cref="ToLiteral"/>
    public static string ToQuotedLiteral(this string? @this)
        => @this is null ? InternalConstants.QuotedNull : new StringBuilder(@this.Length + 2).AppendQuotedLiteral(@this.AsSpan()).ToString();

    /// <summary>
    /// Builds and returns a string representing a given string as a C#
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#verbatim-string-literals">verbatim string literal</see>.
    /// </summary>
    /// <param name="this">The <see langword="string"/> on which this method is called.</param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <seealso cref="ToQuotedLiteral"/>
    /// <seealso cref="ToLiteral"/>
    public static string ToVerbatimLiteral(this string? @this)
        => @this is null ? InternalConstants.QuotedNull : new StringBuilder(@this.Length + 3).AppendVerbatimLiteral(@this.AsSpan()).ToString();

    /// <summary>
    /// Builds and returns a string representing a given string as a C# string literal.
    /// </summary>
    /// <param name="this">The <see langword="string"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to build.</param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <seealso cref="ToQuotedLiteral"/>
    /// <seealso cref="ToVerbatimLiteral"/>
    public static string ToLiteral(this string? @this, StringLiteralKind literalKind)
        => @this is null ? InternalConstants.QuotedNull : new StringBuilder(@this.Length + 2).AppendLiteral(literalKind, @this.AsSpan()).ToString();

    /// <summary>
    /// Gets the length of the <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#quoted-string-literals">quoted literal</see>
    /// representation of a string.
    /// </summary>
    /// <param name="this">The <see langword="string"/> on which this method is called.</param>
    /// <returns>The length of the string that would result from calling
    /// <see cref="ToQuotedLiteral"/>on <paramref name="this"/>.</returns>
    public static int GetQuotedLiteralLength(this string? @this)
        => @this is null ? InternalConstants.QuotedNullLength : @this.AsSpan().GetQuotedLiteralLength();

    /// <summary>
    /// Gets the length of the <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#verbatim-string-literals">verbatim literal</see>
    /// representation of a string.
    /// </summary>
    /// <param name="this">The <see langword="string"/> on which this method is called.</param>
    /// <returns>The length of the string that would result from calling
    /// <see cref="ToVerbatimLiteral"/>on <paramref name="this"/>.</returns>
    public static int GetVerbatimLiteralLength(this string? @this)
        => @this is null ? InternalConstants.QuotedNullLength : @this.AsSpan().GetVerbatimLiteralLength();

    /// <summary>
    /// Gets the length of the representation of a string as a string literal.
    /// </summary>
    /// <param name="this">The <see langword="string"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying
    /// the kind of string literal that would be built.</param>
    /// <returns>The length of the string that would result from calling
    /// <see cref="ToLiteral"/>on <paramref name="this"/>
    /// with <paramref name="literalKind"/> as last parameter.</returns>
    public static int GetLiteralLength(this string? @this, StringLiteralKind literalKind)
        => @this is null ? InternalConstants.QuotedNullLength : @this.AsSpan().GetLiteralLength(literalKind);
}

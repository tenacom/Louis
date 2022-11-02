// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Louis.Text.Internal;

namespace Louis.Text;

partial class StringExtensions
{
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

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

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
        => @this is null
            ? InternalConstants.QuotedNull
            : new StringBuilder(@this.Length + 2)
                .UnsafeAppendQuotedLiteral(@this.AsSpan())
                .ToString();

    /// <summary>
    /// Builds and returns a string representing a given string as a C#
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#verbatim-string-literals">verbatim string literal</see>.
    /// </summary>
    /// <param name="this">The <see langword="string"/> on which this method is called.</param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <seealso cref="ToQuotedLiteral"/>
    /// <seealso cref="ToLiteral"/>
    public static string ToVerbatimLiteral(this string? @this)
        => @this is null
            ? InternalConstants.QuotedNull
            : new StringBuilder(@this.Length + 3)
                .UnsafeAppendVerbatimLiteral(@this.AsSpan())
                .ToString();

    /// <summary>
    /// Builds and returns a string representing a given string as a C# string literal.
    /// </summary>
    /// <param name="this">The <see langword="string"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to build.</param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="literalKind"/> is neither <see cref="StringLiteralKind.Quoted"/>
    /// nor <see cref="StringLiteralKind.Verbatim"/>.</exception>
    /// <seealso cref="ToQuotedLiteral"/>
    /// <seealso cref="ToVerbatimLiteral"/>
    public static string ToLiteral(this string? @this, StringLiteralKind literalKind)
    {
        InternalGuard.IsDefinedStringLiteralKind(literalKind);

        return @this is null
            ? InternalConstants.QuotedNull
            : new StringBuilder(@this.Length + 2)
                .UnsafeAppendLiteral(literalKind, @this.AsSpan())
                .ToString();
    }
}

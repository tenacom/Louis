// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using Louis.Diagnostics;

namespace Louis.Text;

/// <summary>
/// Provides extension methods for read-only spans of characters.
/// </summary>
public static class CharReadOnlySpanExtensions
{
    /// <summary>
    /// Builds and returns a string representing a given span of characters as a C#
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#quoted-string-literals">quoted string literal</see>.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <seealso cref="ToVerbatimLiteral"/>
    /// <seealso cref="ToLiteral"/>
    public static string ToQuotedLiteral(this ReadOnlySpan<char> @this)
        => new StringBuilder(@this.Length + 2).AppendQuotedLiteral(@this).ToString();

    /// <summary>
    /// Builds and returns a string representing a given span of characters as a C#
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#verbatim-string-literals">verbatim string literal</see>.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <seealso cref="ToQuotedLiteral"/>
    /// <seealso cref="ToLiteral"/>
    public static string ToVerbatimLiteral(this ReadOnlySpan<char> @this)
        => new StringBuilder(@this.Length + 3).AppendVerbatimLiteral(@this).ToString();

    /// <summary>
    /// Builds and returns a string representing a given span of characters as a C# string literal.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to build.</param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <seealso cref="ToQuotedLiteral"/>
    /// <seealso cref="ToVerbatimLiteral"/>
    public static string ToLiteral(this ReadOnlySpan<char> @this, StringLiteralKind literalKind)
        => new StringBuilder(@this.Length + 2).AppendLiteral(literalKind, @this).ToString();

    /// <summary>
    /// Gets the length of the <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#quoted-string-literals">quoted literal</see>
    /// representation of a span of characters.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <returns>The length of the string that would result from calling
    /// <see cref="ToQuotedLiteral"/>on <paramref name="this"/>.</returns>
    public static int GetQuotedLiteralLength(this ReadOnlySpan<char> @this)
    {
        var result = 2;
        foreach (var c in @this)
        {
            result += c switch {
                '\x00' or '\x07' or '\x08' or '\x09' or '\x0A'or '\x0B' or '\x0C' or '\x0D' or '"' or '\\' => 2,
                '\x7F' or (>= '\x01' and <= '\x1F') => 4,
                _ => char.GetUnicodeCategory(c) switch {
                    UnicodeCategory.NonSpacingMark
                        or UnicodeCategory.SpacingCombiningMark
                        or UnicodeCategory.EnclosingMark
                        or UnicodeCategory.SpaceSeparator
                        or UnicodeCategory.LineSeparator
                        or UnicodeCategory.ParagraphSeparator
                        or UnicodeCategory.Control
                        or UnicodeCategory.Format
                        or UnicodeCategory.Surrogate
                        or UnicodeCategory.PrivateUse
                        or UnicodeCategory.ConnectorPunctuation
                        or UnicodeCategory.ModifierSymbol
                        or UnicodeCategory.OtherNotAssigned => 6,
                    _ => 1,
                },
            };
        }

        return result;
    }

    /// <summary>
    /// Gets the length of the <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#verbatim-string-literals">verbatim literal</see>
    /// representation of a span of characters.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <returns>The length of the string that would result from calling
    /// <see cref="ToVerbatimLiteral"/>on <paramref name="this"/>.</returns>
    public static int GetVerbatimLiteralLength(this ReadOnlySpan<char> @this)
    {
        var result = 3 + @this.Length;
        foreach (var c in @this)
        {
            if (c == '"')
            {
                result++;
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the length of the representation of a span of characters as a string literal.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying
    /// the kind of string literal that would be built.</param>
    /// <returns>The length of the string that would result from calling
    /// <see cref="ToLiteral"/>on <paramref name="this"/>
    /// with <paramref name="literalKind"/> as last parameter.</returns>
    public static int GetLiteralLength(this ReadOnlySpan<char> @this, StringLiteralKind literalKind)
        => literalKind switch {
            StringLiteralKind.Quoted => GetQuotedLiteralLength(@this),
            StringLiteralKind.Verbatim => GetVerbatimLiteralLength(@this),
            _ => Throw.Argument<int>($"{literalKind} is not a valid {nameof(StringLiteralKind)}.", nameof(literalKind)),
        };
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Louis.Diagnostics;

namespace Louis.Text;

partial class CharReadOnlySpanExtensions
{
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
                '\x00' or '\x07' or '\x08' or '\x09' or '\x0A' or '\x0B' or '\x0C' or '\x0D' or '"' or '\\' => 2,
                '\x7F' => 4,
                < '\x7F' => c < '\x20' ? 4 : 1,
                _ => char.GetUnicodeCategory(c) switch {
                    UnicodeCategory.UppercaseLetter
                        or UnicodeCategory.LowercaseLetter
                        or UnicodeCategory.TitlecaseLetter
                        or UnicodeCategory.ModifierLetter
                        or UnicodeCategory.OtherLetter
                        or UnicodeCategory.DecimalDigitNumber
                        or UnicodeCategory.LetterNumber
                        or UnicodeCategory.OtherNumber
                        or UnicodeCategory.DashPunctuation
                        or UnicodeCategory.OpenPunctuation
                        or UnicodeCategory.ClosePunctuation
                        or UnicodeCategory.InitialQuotePunctuation
                        or UnicodeCategory.FinalQuotePunctuation
                        or UnicodeCategory.OtherPunctuation
                        or UnicodeCategory.MathSymbol
                        or UnicodeCategory.CurrencySymbol
                        or UnicodeCategory.OtherSymbol => 1,
                    _ => 6,
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

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text;

namespace Louis.Text;

partial class StringBuilderExtensions
{
    private const string HexDigits = "0123456789ABCDEF";

    private static StringBuilder AppendQuotedLiteralCore(this StringBuilder @this, ReadOnlySpan<char> chars)
    {
        foreach (var c in chars)
        {
            _ = @this.AppendQuoteChar(c);
        }

        return @this;
    }

    private static StringBuilder AppendVerbatimLiteralCore(this StringBuilder @this, ReadOnlySpan<char> chars)
    {
        foreach (var c in chars)
        {
            _ = c == '"' ? @this.Append("\"\"") : @this.Append(c);
        }

        return @this;
    }

    private static StringBuilder AppendQuoteChar(this StringBuilder @this, char c)
        => c switch {
            '\x00' => @this.Append(@"\0"),
            '\x07' => @this.Append(@"\a"),
            '\x08' => @this.Append(@"\b"),
            '\x09' => @this.Append(@"\t"),
            '\x0A' => @this.Append(@"\n"),
            '\x0B' => @this.Append(@"\v"),
            '\x0C' => @this.Append(@"\f"),
            '\x0D' => @this.Append(@"\r"),
            '"' => @this.Append(@"\"""),
            '\\' => @this.Append(@"\\"),
            '\x7F' => @this.AppendAsciiEscape(c),
            < '\x7F' => c < '\x20' ? @this.AppendAsciiEscape(c) : @this.Append(c),
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
                    or UnicodeCategory.OtherSymbol => @this.Append(c),
                _ => @this.AppendUnicodeEscape(c),
            },
        };

    private static StringBuilder AppendAsciiEscape(this StringBuilder @this, char c)
    {
        var n = (int)c;
        return @this.Append(@"\x")
                    .Append(HexDigits[(n >> 4) & 0xF])
                    .Append(HexDigits[n & 0xF]);
    }

    private static StringBuilder AppendUnicodeEscape(this StringBuilder @this, char c)
    {
        var n = (int)c;
        return @this.Append(@"\u")
                    .Append(HexDigits[(n >> 12) & 0xF])
                    .Append(HexDigits[(n >> 8) & 0xF])
                    .Append(HexDigits[(n >> 4) & 0xF])
                    .Append(HexDigits[n & 0xF]);
    }
}

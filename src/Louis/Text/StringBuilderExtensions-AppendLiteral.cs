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
using Louis.Text.Internal;

namespace Louis.Text;

partial class StringBuilderExtensions
{
    private static readonly ReadOnlyMemory<char> HexDigits = "0123456789ABCDEF".AsMemory();

    /// <summary>
    /// Appends the specified string, expressed as a C# quoted string literal, to the end
    /// of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="str">The string to append.</param>
    /// <returns>A reference to this instance after the append operation has completed..</returns>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendVerbatimLiteral(StringBuilder,string?)"/>
    /// <seealso cref="AppendLiteral(StringBuilder,StringLiteralKind,string?)"/>
    public static StringBuilder AppendQuotedLiteral(this StringBuilder @this, string? str)
        => str is null ? @this.Append(InternalConstants.QuotedNull) : AppendQuotedLiteral(@this, str.AsSpan());

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# quoted string literal,
    /// to the end of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="chars">The characters to append.</param>
    /// <returns>A reference to this instance after the append operation has completed..</returns>
    /// <seealso cref="AppendVerbatimLiteral(StringBuilder,ReadOnlySpan{char})"/>
    /// <seealso cref="AppendLiteral(StringBuilder,StringLiteralKind,ReadOnlySpan{char})"/>
    public static StringBuilder AppendQuotedLiteral(this StringBuilder @this, ReadOnlySpan<char> chars)
    {
        _ = @this.Append('"');
        foreach (var c in chars)
        {
            var quotedChar = QuoteChar(c);
            _ = quotedChar is null ? @this.Append(c) : @this.Append(quotedChar);
        }

        return @this.Append('"');
    }

    /// <summary>
    /// Appends the specified string, expressed as a C# verbatim string literal, to the end
    /// of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="str">The string to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendQuotedLiteral(StringBuilder,string?)"/>
    /// <seealso cref="AppendLiteral(StringBuilder,StringLiteralKind,string?)"/>
    public static StringBuilder AppendVerbatimLiteral(this StringBuilder @this, string? str)
        => str is null ? @this.Append(InternalConstants.QuotedNull) : AppendVerbatimLiteral(@this, str.AsSpan());

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# verbatim string literal,
    /// to the end of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="chars">The characters to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <seealso cref="AppendQuotedLiteral(StringBuilder,ReadOnlySpan{char})"/>
    /// <seealso cref="AppendLiteral(StringBuilder,StringLiteralKind,ReadOnlySpan{char})"/>
    public static StringBuilder AppendVerbatimLiteral(this StringBuilder @this, ReadOnlySpan<char> chars)
    {
        _ = @this.Append('@').Append('"');
        foreach (var c in chars)
        {
            _ = c == '"' ? @this.Append(c).Append(c) : @this.Append(c);
        }

        return @this.Append('"');
    }

    /// <summary>
    /// Appends the specified string, expressed as a C# string literal, to the end of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to append.</param>
    /// <param name="str">The string to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendQuotedLiteral(System.Text.StringBuilder,string?)"/>
    /// <seealso cref="AppendVerbatimLiteral(System.Text.StringBuilder,string?)"/>
    /// <seealso cref="StringLiteralKind"/>
    public static StringBuilder AppendLiteral(this StringBuilder @this, StringLiteralKind literalKind, string? str)
        => str is null ? @this.Append(InternalConstants.QuotedNull) : AppendLiteral(@this, literalKind, str.AsSpan());

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# string literal, to the end of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to append.</param>
    /// <param name="chars">The characters to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <seealso cref="AppendQuotedLiteral(System.Text.StringBuilder,System.ReadOnlySpan{char})"/>
    /// <seealso cref="AppendVerbatimLiteral(System.Text.StringBuilder,System.ReadOnlySpan{char})"/>
    /// <seealso cref="StringLiteralKind"/>
    public static StringBuilder AppendLiteral(this StringBuilder @this, StringLiteralKind literalKind, ReadOnlySpan<char> chars)
        => literalKind switch {
            StringLiteralKind.Quoted => AppendQuotedLiteral(@this, chars),
            StringLiteralKind.Verbatim => AppendVerbatimLiteral(@this, chars),
            _ => Throw.Argument<StringBuilder>($"{literalKind} is not a valid {nameof(StringLiteralKind)}.", nameof(literalKind)),
        };

    private static string? QuoteChar(char c)
        => c switch {
            '\x00' => @"\0",
            '\x07' => @"\a",
            '\x08' => @"\b",
            '\x09' => @"\t",
            '\x0A' => @"\n",
            '\x0B' => @"\v",
            '\x0C' => @"\f",
            '\x0D' => @"\r",
            '"' => @"\""",
            '\\' => @"\\",
            '\x7F' or (>= '\x01' and <= '\x1F') => AsciiEscape(c),
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
                    or UnicodeCategory.OtherNotAssigned => UnicodeEscape(c),
                _ => null,
            },
        };

    private static string AsciiEscape(char c)
    {
        var digits = HexDigits.Span;
        var n = (int)c;
        Span<char> result = stackalloc char[4];
        result[0] = '\\';
        result[1] = 'x';
        result[2] = digits[(n >> 4) & 0xF];
        result[3] = digits[n & 0xF];
        return result.ToString();
    }

    private static string UnicodeEscape(char c)
    {
        var digits = HexDigits.Span;
        var n = (int)c;
        Span<char> result = stackalloc char[6];
        result[0] = '\\';
        result[1] = 'u';
        result[5] = digits[n & 0xF];
        result[4] = digits[(n >>= 4) & 0xF];
        result[3] = digits[(n >>= 4) & 0xF];
        result[2] = digits[(n >>= 4) & 0xF];
        return result.ToString();
    }
}

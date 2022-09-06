// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Text;
using Louis.Diagnostics;
using Louis.Text.Internal;

namespace Louis.Text;

partial class StringBuilderExtensions
{
    /// <summary>
    /// Appends the specified string, expressed as a C# quoted string literal, to the end
    /// of this instance. If the string is longer than the sum of <paramref name="headLength"/>,
    /// <paramref name="tailLength"/>, and the length of an ellipsis (1 if <paramref name="useUnicodeEllipsis"/>
    /// is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first <paramref name="headLength"/>
    /// characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or three dots <c>...</c>)
    /// and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="str">The string to append.</param>
    /// <param name="headLength">The number of characters to leave before clipping. Negative values will be treated as 0.</param>
    /// <param name="tailLength">The number of characters to leave after clipping. Negative values will be treated as 0.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>.</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A reference to this instance after the append operation has completed..</returns>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendClippedVerbatimLiteral(StringBuilder,string?,int,int,bool)"/>
    /// <seealso cref="AppendClippedLiteral(StringBuilder,StringLiteralKind,string?,int,int,bool)"/>
    public static StringBuilder AppendClippedQuotedLiteral(
        this StringBuilder @this,
        string? str,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
        => str is null
            ? @this.Append(InternalConstants.QuotedNull)
            : AppendClippedQuotedLiteral(@this, str.AsSpan(), headLength, tailLength, useUnicodeEllipsis);

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# quoted string literal, to the end
    /// of this instance. If the span is longer than the sum of <paramref name="headLength"/>,
    /// <paramref name="tailLength"/>, and the length of an ellipsis (1 if <paramref name="useUnicodeEllipsis"/>
    /// is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first <paramref name="headLength"/>
    /// characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or three dots <c>...</c>)
    /// and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="chars">The characters to append.</param>
    /// <param name="headLength">The number of characters to leave before clipping. Negative values will be treated as 0.</param>
    /// <param name="tailLength">The number of characters to leave after clipping. Negative values will be treated as 0.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>...</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A reference to this instance after the append operation has completed..</returns>
    /// <seealso cref="AppendClippedVerbatimLiteral(StringBuilder,ReadOnlySpan{char},int,int,bool)"/>
    /// <seealso cref="AppendClippedLiteral(StringBuilder,StringLiteralKind,ReadOnlySpan{char},int,int,bool)"/>
    public static StringBuilder AppendClippedQuotedLiteral(
        this StringBuilder @this,
        ReadOnlySpan<char> chars,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
    {
        if (headLength < 0)
        {
            headLength = 0;
        }

        if (tailLength < 0)
        {
            tailLength = 0;
        }

        var unsignedLength = (uint)chars.Length;
        var unclippedLength = (uint)headLength + (uint)tailLength;
        if (unclippedLength >= int.MaxValue)
        {
            return @this.AppendQuotedLiteral(chars);
        }

        var ellipsisLength = useUnicodeEllipsis ? 1U : 3U;
        if (unsignedLength <= unclippedLength + ellipsisLength)
        {
            return @this.AppendQuotedLiteral(chars);
        }

        var tailStart = chars.Length - tailLength;
        return @this.Append('"')
                    .AppendQuotedLiteralCore(chars[0..headLength])
                    .AppendEllipsis(useUnicodeEllipsis)
                    .AppendQuotedLiteralCore(chars[tailStart..])
                    .Append('"');
    }

    /// <summary>
    /// Appends the specified string, expressed as a C# verbatim string literal, to the end
    /// of this instance. If the string is longer than the sum of <paramref name="headLength"/>,
    /// <paramref name="tailLength"/>, and the length of an ellipsis (1 if <paramref name="useUnicodeEllipsis"/>
    /// is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first <paramref name="headLength"/>
    /// characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or three dots <c>...</c>)
    /// and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="str">The string to append.</param>
    /// <param name="headLength">The number of characters to leave before clipping. Negative values will be treated as 0.</param>
    /// <param name="tailLength">The number of characters to leave after clipping. Negative values will be treated as 0.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>.</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendClippedQuotedLiteral(StringBuilder,string?,int,int,bool)"/>
    /// <seealso cref="AppendClippedLiteral(StringBuilder,StringLiteralKind,string?,int,int,bool)"/>
    public static StringBuilder AppendClippedVerbatimLiteral(
        this StringBuilder @this,
        string? str,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
        => str is null
            ? @this.Append(InternalConstants.QuotedNull)
            : AppendClippedVerbatimLiteral(@this, str.AsSpan(), headLength, tailLength, useUnicodeEllipsis);

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# verbatim string literal, to the end
    /// of this instance. If the span is longer than the sum of <paramref name="headLength"/>,
    /// <paramref name="tailLength"/>, and the length of an ellipsis (1 if <paramref name="useUnicodeEllipsis"/>
    /// is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first <paramref name="headLength"/>
    /// characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or three dots <c>...</c>)
    /// and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="chars">The characters to append.</param>
    /// <param name="headLength">The number of characters to leave before clipping. Negative values will be treated as 0.</param>
    /// <param name="tailLength">The number of characters to leave after clipping. Negative values will be treated as 0.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>.</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <seealso cref="AppendClippedQuotedLiteral(StringBuilder,ReadOnlySpan{char},int,int,bool)"/>
    /// <seealso cref="AppendClippedLiteral(StringBuilder,StringLiteralKind,ReadOnlySpan{char},int,int,bool)"/>
    public static StringBuilder AppendClippedVerbatimLiteral(
        this StringBuilder @this,
        ReadOnlySpan<char> chars,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
    {
        if (headLength < 0)
        {
            headLength = 0;
        }

        if (tailLength < 0)
        {
            tailLength = 0;
        }

        var unsignedLength = (uint)chars.Length;
        var unclippedLength = (uint)headLength + (uint)tailLength;
        if (unclippedLength >= int.MaxValue)
        {
            return @this.AppendVerbatimLiteral(chars);
        }

        var ellipsisLength = useUnicodeEllipsis ? 1U : 3U;
        if (unsignedLength <= unclippedLength + ellipsisLength)
        {
            return @this.AppendVerbatimLiteral(chars);
        }

        var tailStart = chars.Length - tailLength;
        return @this.Append(@"@""")
                    .AppendVerbatimLiteralCore(chars[0..headLength])
                    .AppendEllipsis(useUnicodeEllipsis)
                    .AppendVerbatimLiteralCore(chars[tailStart..])
                    .Append('"');
    }

    /// <summary>
    /// Appends the specified string, expressed as a C# string literal, to the end
    /// of this instance. If the string is longer than the sum of <paramref name="headLength"/>,
    /// <paramref name="tailLength"/>, and the length of an ellipsis (1 if <paramref name="useUnicodeEllipsis"/>
    /// is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first <paramref name="headLength"/>
    /// characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or three dots <c>...</c>)
    /// and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to append.</param>
    /// <param name="str">The string to append.</param>
    /// <param name="headLength">The number of characters to leave before clipping. Negative values will be treated as 0.</param>
    /// <param name="tailLength">The number of characters to leave after clipping. Negative values will be treated as 0.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>.</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendClippedQuotedLiteral(System.Text.StringBuilder,string?,int,int,bool)"/>
    /// <seealso cref="AppendClippedVerbatimLiteral(System.Text.StringBuilder,string?,int,int,bool)"/>
    /// <seealso cref="StringLiteralKind"/>
    public static StringBuilder AppendClippedLiteral(
        this StringBuilder @this,
        StringLiteralKind literalKind,
        string? str,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
        => str is null
            ? @this.Append(InternalConstants.QuotedNull)
            : AppendClippedLiteral(@this, literalKind, str.AsSpan(), headLength, tailLength, useUnicodeEllipsis);

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# string literal, to the end
    /// of this instance. If the span is longer than the sum of <paramref name="headLength"/>,
    /// <paramref name="tailLength"/>, and the length of an ellipsis (1 if <paramref name="useUnicodeEllipsis"/>
    /// is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first <paramref name="headLength"/>
    /// characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or three dots <c>...</c>)
    /// and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to append.</param>
    /// <param name="chars">The characters to append.</param>
    /// <param name="headLength">The number of characters to leave before clipping. Negative values will be treated as 0.</param>
    /// <param name="tailLength">The number of characters to leave after clipping. Negative values will be treated as 0.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>.</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <seealso cref="AppendClippedQuotedLiteral(System.Text.StringBuilder,System.ReadOnlySpan{char},int,int,bool)"/>
    /// <seealso cref="AppendClippedVerbatimLiteral(System.Text.StringBuilder,System.ReadOnlySpan{char},int,int,bool)"/>
    /// <seealso cref="StringLiteralKind"/>
    public static StringBuilder AppendClippedLiteral(
        this StringBuilder @this,
        StringLiteralKind literalKind,
        ReadOnlySpan<char> chars,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
        => literalKind switch {
            StringLiteralKind.Quoted => AppendClippedQuotedLiteral(@this, chars, headLength, tailLength, useUnicodeEllipsis),
            StringLiteralKind.Verbatim => AppendClippedVerbatimLiteral(@this, chars, headLength, tailLength, useUnicodeEllipsis),
            _ => Throw.Argument<StringBuilder>($"{literalKind} is not a valid {nameof(StringLiteralKind)}.", nameof(literalKind)),
        };

    private static StringBuilder AppendEllipsis(this StringBuilder @this, bool useUnicodeEllipsis)
        => useUnicodeEllipsis ? @this.Append('\x2026') : @this.Append("...");
}

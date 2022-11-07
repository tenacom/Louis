// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text;
using CommunityToolkit.Diagnostics;
using Louis.Text.Internal;

namespace Louis.Text;

partial class CharReadOnlySpanExtensions
{
    /// <summary>
    /// Builds and returns a string representing a given span of characters as a C#
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#quoted-string-literals">quoted string literal</see>.
    /// If the string is longer than the sum of <paramref name="headLength"/>, <paramref name="tailLength"/>, and the length of an ellipsis
    /// (1 if <paramref name="useUnicodeEllipsis"/> is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first
    /// <paramref name="headLength"/> characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or
    /// three dots <c>...</c>) and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <param name="headLength">The number of characters to leave before clipping.</param>
    /// <param name="tailLength">The number of characters to leave after clipping.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>.</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="headLength"/> is less than 0.</para>
    /// <para>- or -</para>
    /// <para><paramref name="tailLength"/> is less than 0.</para>
    /// </exception>
    /// <seealso cref="ToClippedVerbatimLiteral"/>
    /// <seealso cref="ToClippedLiteral"/>
    public static string ToClippedQuotedLiteral(
        this ReadOnlySpan<char> @this,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
    {
        Guard.IsGreaterThanOrEqualTo(headLength, 0);
        Guard.IsGreaterThanOrEqualTo(tailLength, 0);

        return new StringBuilder(@this.Length + 2)
              .AppendClippedQuotedLiteral(@this, headLength, tailLength, useUnicodeEllipsis)
              .ToString();
    }

    /// <summary>
    /// Builds and returns a string representing a given span of characters as a C#
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#verbatim-string-literals">verbatim string literal</see>.
    /// If the string is longer than the sum of <paramref name="headLength"/>, <paramref name="tailLength"/>, and the length of an ellipsis
    /// (1 if <paramref name="useUnicodeEllipsis"/> is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first
    /// <paramref name="headLength"/> characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or
    /// three dots <c>...</c>) and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <param name="headLength">The number of characters to leave before clipping.</param>
    /// <param name="tailLength">The number of characters to leave after clipping.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>.</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="headLength"/> is less than 0.</para>
    /// <para>- or -</para>
    /// <para><paramref name="tailLength"/> is less than 0.</para>
    /// </exception>
    /// <seealso cref="ToClippedQuotedLiteral"/>
    /// <seealso cref="ToClippedLiteral"/>
    public static string ToClippedVerbatimLiteral(
        this ReadOnlySpan<char> @this,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
    {
        Guard.IsGreaterThanOrEqualTo(headLength, 0);
        Guard.IsGreaterThanOrEqualTo(tailLength, 0);

        return new StringBuilder(@this.Length + 3)
              .AppendClippedVerbatimLiteral(@this, headLength, tailLength, useUnicodeEllipsis)
              .ToString();
    }

    /// <summary>
    /// Builds and returns a string representing a given span of characters as a C# string literal.
    /// If the string is longer than the sum of <paramref name="headLength"/>, <paramref name="tailLength"/>, and the length of an ellipsis
    /// (1 if <paramref name="useUnicodeEllipsis"/> is <see langword="true"/>, 3 otherwise), then it is clipped, taking only the first
    /// <paramref name="headLength"/> characters, followed by an ellipsis (either the Unicode character <c>'\x2026'</c> (<c>…</c>), or
    /// three dots <c>...</c>) and the last <paramref name="tailLength"/> characters.
    /// </summary>
    /// <param name="this">The <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;char&gt;</see>
    /// on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to build.</param>
    /// <param name="headLength">The number of characters to leave before clipping.</param>
    /// <param name="tailLength">The number of characters to leave after clipping.</param>
    /// <param name="useUnicodeEllipsis">
    /// <see langword="true"/> to use a single Unicode character Ellipsis (<c>'\x2026'</c>, <c>…</c>) to replace the clipped part;
    /// <see langword="false"/> to use three dots (<c>.</c>) instead. Default is <see langword="false"/>.
    /// </param>
    /// <returns>A newly-constructed <see langword="string"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="literalKind"/> is neither <see cref="StringLiteralKind.Quoted"/>
    /// nor <see cref="StringLiteralKind.Verbatim"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <para><paramref name="headLength"/> is less than 0.</para>
    /// <para>- or -</para>
    /// <para><paramref name="tailLength"/> is less than 0.</para>
    /// </exception>
    /// <seealso cref="ToClippedQuotedLiteral"/>
    /// <seealso cref="ToClippedVerbatimLiteral"/>
    public static string ToClippedLiteral(
        this ReadOnlySpan<char> @this,
        StringLiteralKind literalKind,
        int headLength,
        int tailLength,
        bool useUnicodeEllipsis = false)
    {
        InternalGuard.IsDefinedStringLiteralKind(literalKind);
        Guard.IsGreaterThanOrEqualTo(headLength, 0);
        Guard.IsGreaterThanOrEqualTo(tailLength, 0);

        return new StringBuilder(@this.Length + 2)
              .AppendClippedLiteral(literalKind, @this, headLength, tailLength, useUnicodeEllipsis)
              .ToString();
    }
}

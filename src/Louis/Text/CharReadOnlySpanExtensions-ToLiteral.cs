// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text;

namespace Louis.Text;

partial class CharReadOnlySpanExtensions
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
}

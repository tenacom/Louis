// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text;
using CommunityToolkit.Diagnostics;
using Louis.Text.Internal;

namespace Louis.Text;

partial class StringBuilderExtensions
{
    /// <summary>
    /// Appends the specified string, expressed as a C# quoted string literal, to the end
    /// of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="str">The string to append.</param>
    /// <returns>A reference to this instance after the append operation has completed..</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendVerbatimLiteral(StringBuilder,string?)"/>
    /// <seealso cref="AppendLiteral(StringBuilder,StringLiteralKind,string?)"/>
    public static StringBuilder AppendQuotedLiteral(this StringBuilder @this, string? str)
    {
        Guard.IsNotNull(@this);

        return str is null
            ? @this.Append(InternalConstants.QuotedNull)
            : UnsafeAppendQuotedLiteral(@this, str.AsSpan());
    }

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# quoted string literal,
    /// to the end of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="chars">The characters to append.</param>
    /// <returns>A reference to this instance after the append operation has completed..</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    /// <seealso cref="AppendVerbatimLiteral(StringBuilder,ReadOnlySpan{char})"/>
    /// <seealso cref="AppendLiteral(StringBuilder,StringLiteralKind,ReadOnlySpan{char})"/>
    public static StringBuilder AppendQuotedLiteral(this StringBuilder @this, ReadOnlySpan<char> chars)
    {
        Guard.IsNotNull(@this);

        return UnsafeAppendQuotedLiteral(@this, chars);
    }

    /// <summary>
    /// Appends the specified string, expressed as a C# verbatim string literal, to the end
    /// of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="str">The string to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendQuotedLiteral(StringBuilder,string?)"/>
    /// <seealso cref="AppendLiteral(StringBuilder,StringLiteralKind,string?)"/>
    public static StringBuilder AppendVerbatimLiteral(this StringBuilder @this, string? str)
    {
        Guard.IsNotNull(@this);

        return str is null
            ? @this.Append(InternalConstants.QuotedNull)
            : UnsafeAppendVerbatimLiteral(@this, str.AsSpan());
    }

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# verbatim string literal,
    /// to the end of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="chars">The characters to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    /// <seealso cref="AppendQuotedLiteral(StringBuilder,ReadOnlySpan{char})"/>
    /// <seealso cref="AppendLiteral(StringBuilder,StringLiteralKind,ReadOnlySpan{char})"/>
    public static StringBuilder AppendVerbatimLiteral(this StringBuilder @this, ReadOnlySpan<char> chars)
    {
        Guard.IsNotNull(@this);

        return UnsafeAppendVerbatimLiteral(@this, chars);
    }

    /// <summary>
    /// Appends the specified string, expressed as a C# string literal, to the end of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to append.</param>
    /// <param name="str">The string to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="literalKind"/> is neither <see cref="StringLiteralKind.Quoted"/>
    /// nor <see cref="StringLiteralKind.Verbatim"/>.</exception>
    /// <remarks>
    /// <para>If <paramref name="str"/> is <see langword="null"/>, the string <c>null</c>
    /// (without surrounding quotes) will be appended to <paramref name="this"/>.</para>
    /// </remarks>
    /// <seealso cref="AppendQuotedLiteral(StringBuilder,string?)"/>
    /// <seealso cref="AppendVerbatimLiteral(StringBuilder,string?)"/>
    /// <seealso cref="StringLiteralKind"/>
    public static StringBuilder AppendLiteral(this StringBuilder @this, StringLiteralKind literalKind, string? str)
    {
        Guard.IsNotNull(@this);
        InternalGuard.IsDefinedStringLiteralKind(literalKind);

        return str is null
            ? @this.Append(InternalConstants.QuotedNull)
            : UnsafeAppendLiteral(@this, literalKind, str.AsSpan());
    }

    /// <summary>
    /// Appends the specified span of characters, expressed as a C# string literal, to the end of this instance.
    /// </summary>
    /// <param name="this">The <see cref="StringBuilder"/> on which this method is called.</param>
    /// <param name="literalKind">A <see cref="StringLiteralKind"/> constant specifying the kind of string literal
    /// to append.</param>
    /// <param name="chars">The characters to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="literalKind"/> is neither <see cref="StringLiteralKind.Quoted"/>
    /// nor <see cref="StringLiteralKind.Verbatim"/>.</exception>
    /// <seealso cref="AppendQuotedLiteral(StringBuilder,ReadOnlySpan{char})"/>
    /// <seealso cref="AppendVerbatimLiteral(StringBuilder,ReadOnlySpan{char})"/>
    /// <seealso cref="StringLiteralKind"/>
    public static StringBuilder AppendLiteral(this StringBuilder @this, StringLiteralKind literalKind, ReadOnlySpan<char> chars)
    {
        Guard.IsNotNull(@this);
        InternalGuard.IsDefinedStringLiteralKind(literalKind);

        return UnsafeAppendLiteral(@this, literalKind, chars);
    }

    internal static StringBuilder UnsafeAppendQuotedLiteral(this StringBuilder @this, ReadOnlySpan<char> chars)
    {
        @this.EnsureCapacity(@this.Length + 2 + chars.Length);
        return @this.Append('"')
                    .AppendQuotedLiteralCore(chars)
                    .Append('"');
    }

    internal static StringBuilder UnsafeAppendVerbatimLiteral(this StringBuilder @this, ReadOnlySpan<char> chars)
    {
        @this.EnsureCapacity(@this.Length + 3 + chars.Length);
        return @this.Append(@"@""")
                    .AppendVerbatimLiteralCore(chars)
                    .Append('"');
    }

    internal static StringBuilder UnsafeAppendLiteral(this StringBuilder @this, StringLiteralKind literalKind, ReadOnlySpan<char> chars)
        => literalKind switch {
            StringLiteralKind.Quoted => UnsafeAppendQuotedLiteral(@this, chars),
            StringLiteralKind.Verbatim => UnsafeAppendVerbatimLiteral(@this, chars),
            _ => @this,
        };
}

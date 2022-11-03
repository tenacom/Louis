// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Text;
using CommunityToolkit.Diagnostics;

namespace Louis.Text;

/// <summary>
/// Provides useful methods to work with UTF-8 encoding.
/// </summary>
public static partial class Utf8Utility
{
    /// <summary>
    /// <para>An <see cref="Encoding"/> that can be used to encode UTF-8 text without a byte order mark (BOM).</para>
    /// </summary>
    public static readonly Encoding Utf8NoBomEncoding = new UTF8Encoding(false);

    /// <summary>
    /// Computes the maximum number of characters from a given string
    /// that will fit in a given number of bytes when encoded using UTF-8.
    /// </summary>
    /// <param name="str">The string to encode.</param>
    /// <param name="maxBytes">The maximum number of bytes available for the encoded text.</param>
    /// <returns>The maximum number of characters, starting from the beginning of <paramref name="str"/>,
    /// whose UTF-8 representation will fit in a maximum of <paramref name="maxBytes"/> bytes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="str"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxBytes"/> is a negative number.</exception>
    /// <remarks>
    /// <para>This method does not use any functionality from the <c>System.Text</c> namespace;
    /// its computations are based solely on the UTF-8 standard.</para>
    /// <para>This method assumes that any unrepresentable character will be encoded as the Unicode replacement character
    /// (code point <c>0xFFFD</c>). This is the default for both <see cref="Encoding.UTF8"/>
    /// and <see cref="Utf8NoBomEncoding"/>.</para>
    /// <para>This method does not take into account the UTF-8 preamble (BOM), which would add 3 additional bytes
    /// at the start of the encoded string. If you plan to use the preamble, you should subtract 3 from the number
    /// of available bytes when calling this method.</para>
    /// </remarks>
    public static int GetMaxCharsInBytes(string str, int maxBytes)
    {
        Guard.IsNotNull(str);
        Guard.IsGreaterThanOrEqualTo(maxBytes, 0);

        return MaxCharsInBytesHelper.GetMaxCharsInBytes(str.AsSpan(), maxBytes);
    }

    /// <summary>
    /// Computes the maximum number of characters from a given span
    /// that will fit in a given number of bytes when encoded using UTF-8.
    /// </summary>
    /// <param name="chars">The characters to encode.</param>
    /// <param name="maxBytes">The maximum number of bytes available for the encoded text.</param>
    /// <returns>The maximum number of characters, starting from the beginning of <paramref name="chars"/>,
    /// whose UTF-8 representation will fit in a maximum of <paramref name="maxBytes"/> bytes.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxBytes"/> is a negative number.</exception>
    /// <remarks>
    /// <para>This method does not use any functionality from the <c>System.Text</c> namespace;
    /// its computations are based solely on the UTF-8 standard.</para>
    /// <para>This method assumes that any unrepresentable character will be encoded as the Unicode replacement character
    /// (code point <c>0xFFFD</c>). This is the default for both <see cref="Encoding.UTF8"/>
    /// and <see cref="Utf8NoBomEncoding"/>.</para>
    /// <para>This method does not take into account the UTF-8 preamble (BOM), which would add 3 additional bytes
    /// at the start of the encoded string. If you plan to use the preamble, you should subtract 3 from the number
    /// of available bytes when calling this method.</para>
    /// </remarks>
    public static int GetMaxCharsInBytes(ReadOnlySpan<char> chars, int maxBytes)
    {
        Guard.IsGreaterThanOrEqualTo(maxBytes, 0);

        return MaxCharsInBytesHelper.GetMaxCharsInBytes(chars, maxBytes);
    }
}

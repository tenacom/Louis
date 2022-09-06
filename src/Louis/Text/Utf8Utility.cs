// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Text;
using Louis.ArgumentValidation;

namespace Louis.Text;

/// <summary>
/// Provides useful methods to work with UTF-8 encoding.
/// </summary>
public static class Utf8Utility
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
    /// <exception cref="System.ArgumentNullException"><paramref name="str"/> is <see langword="null"/>.</exception>
    /// <exception cref="System.ArgumentException"><paramref name="maxBytes"/> is zero or a negative number.</exception>
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
        => UnsafeGetMaxCharsInBytes(
            Validated.NotNull(str).AsSpan(),
            Require.Of(maxBytes).GreaterThanZero());

    /// <summary>
    /// Computes the maximum number of characters from a given span
    /// that will fit in a given number of bytes when encoded using UTF-8.
    /// </summary>
    /// <param name="chars">The characters to encode.</param>
    /// <param name="maxBytes">The maximum number of bytes available for the encoded text.</param>
    /// <returns>The maximum number of characters, starting from the beginning of <paramref name="chars"/>,
    /// whose UTF-8 representation will fit in a maximum of <paramref name="maxBytes"/> bytes.</returns>
    /// <exception cref="System.ArgumentException"><paramref name="maxBytes"/> is zero or a negative number.</exception>
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
        => UnsafeGetMaxCharsInBytes(
            chars,
            Require.Of(maxBytes).GreaterThanZero());

    internal static int UnsafeGetMaxCharsInBytes(ReadOnlySpan<char> chars, int maxBytes)
    {
        var result = 0;
        var highSurrogateEncountered = false;
        foreach (var c in chars)
        {
            switch ((int)c)
            {
                case < 0x80: // ASCII equivalent -> one byte
                    if (highSurrogateEncountered)
                    {
                        // High surrogate followed by non-surrogate.
                        // The high surrogate will be encoded as the Unicode replacement character (0xFFFD)
                        // which takes 3 bytes in UTF-8.
                        if (AddBytesAndCheckLimit(1, 3))
                        {
                            return result;
                        }

                        highSurrogateEncountered = false;
                    }

                    if (AddBytesAndCheckLimit(1, 1))
                    {
                        return result;
                    }

                    break;

                case < 0x800: // UTF-16 character -> UTF-8 two-byte sequence
                    if (highSurrogateEncountered)
                    {
                        // High surrogate followed by non-surrogate.
                        // The high surrogate will be encoded as the Unicode replacement character (0xFFFD)
                        // which takes 3 bytes in UTF-8.
                        if (AddBytesAndCheckLimit(1, 3))
                        {
                            return result;
                        }

                        highSurrogateEncountered = false;
                    }

                    if (AddBytesAndCheckLimit(1, 2))
                    {
                        return result;
                    }

                    break;

                case >= 0xD800 and < 0xDC00: // UTF-16 high surrogate
                    if (highSurrogateEncountered)
                    {
                        // High surrogate is followed by another high surrogate.
                        // The first high surrogate will be encoded as the Unicode replacement character (0xFFFD)
                        // which takes 3 bytes in UTF-8.
                        // The second high surrogate is accounted for, but translates to no UTF-8 bytes for now.
                        if (AddBytesAndCheckLimit(1, 3))
                        {
                            return result;
                        }
                    }

                    highSurrogateEncountered = true;
                    break;

                case >= 0xDC00 and < 0xDFFF: // UTF-16 low surrogate
                    if (highSurrogateEncountered)
                    {
                        // High surrogate followed by low surrogate.
                        // This means a code point outside of the Basic Multilingual Plane,
                        // which will be encoded as 4 bytes in UTF-8.
                        if (AddBytesAndCheckLimit(2, 4))
                        {
                            return result;
                        }

                        highSurrogateEncountered = false;
                    }
                    else
                    {
                        // Low surrogate without a high surrogate.
                        // Will be encoded as the Unicode replacement character (0xFFFD) hence 3 bytes.
                        if (AddBytesAndCheckLimit(1, 3))
                        {
                            return result;
                        }
                    }

                    break;

                default: // 0x1000..0xFFFF (excluding surrogates) -> UTF-8 three-byte sequence
                    if (highSurrogateEncountered)
                    {
                        // High surrogate followed by non-surrogate.
                        // The high surrogate will be encoded as the Unicode replacement character (0xFFFD)
                        // which takes 3 bytes in UTF-8.
                        if (AddBytesAndCheckLimit(1, 3))
                        {
                            return result;
                        }

                        highSurrogateEncountered = false;
                    }

                    if (AddBytesAndCheckLimit(1, 3))
                    {
                        return result;
                    }

                    break;
            }
        }

        return result;

        bool AddBytesAndCheckLimit(int charsTaken, int bytesAdded)
        {
            if (maxBytes - result < bytesAdded)
            {
                return true;
            }

            result += charsTaken;
            return false;
        }
    }
}

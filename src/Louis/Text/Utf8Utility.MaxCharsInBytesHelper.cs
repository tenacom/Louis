// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;

namespace Louis.Text;

partial class Utf8Utility
{
    private ref struct MaxCharsInBytesHelper
    {
        private int _totalCharsTaken;
        private int _totalBytesAdded;
        private bool _highSurrogateEncountered;

        private int MaxBytes { get; init; }

        public static int GetMaxCharsInBytes(ReadOnlySpan<char> chars, int maxBytes)
        {
            if (maxBytes < 1)
            {
                return 0;
            }

            var instance = new MaxCharsInBytesHelper { MaxBytes = maxBytes };
            return instance.GetMaxCharsInBytes(chars);
        }

        private int GetMaxCharsInBytes(ReadOnlySpan<char> chars)
        {
            foreach (var c in chars)
            {
                if ((int)c switch {
                    < 0x80 => TryAddSingleByte(),
                    < 0x800 => TryAddTwoByteSequence(),
                    >= 0xD800 and < 0xDC00 => TryAddHighSurrogate(),
                    >= 0xDC00 and < 0xDFFF => TryAddLowSurrogate(),
                    _ => TryAddThreeByteSequence(),
                })
                {
                    break;
                }
            }

            return _totalCharsTaken;
        }

        private bool TryAddSingleByte() => TryAddPrecedingHighSurrogate() || TryAddBytes(1, 1);

        private bool TryAddTwoByteSequence() => TryAddPrecedingHighSurrogate() || TryAddBytes(1, 2);

        private bool TryAddThreeByteSequence() => TryAddPrecedingHighSurrogate() || TryAddBytes(1, 3);

        private bool TryAddHighSurrogate()
        {
            if (TryAddPrecedingHighSurrogate())
            {
                return true;
            }

            _highSurrogateEncountered = true;
            return false;
        }

        private bool TryAddLowSurrogate()
        {
            if (!_highSurrogateEncountered)
            {
                // A UTF-16 low surrogate without a preceding high surrogate
                // will be encoded as the Unicode replacement character (0xFFFD),
                // which takes 3 bytes in UTF-8.
                return TryAddBytes(1, 3);
            }

            _highSurrogateEncountered = false;

            // A UTF-16 high surrogate followed by a low surrogate
            // represents a code point outside of the Basic Multilingual Plane,
            // which takes 4 bytes in UTF-8.
            return TryAddBytes(2, 4);
        }

        // A UTF-16 high surrogate not followed by a low surrogate
        // will be encoded as the Unicode replacement character (0xFFFD),
        // which takes 3 bytes in UTF-8.
        private bool TryAddPrecedingHighSurrogate()
        {
            if (!_highSurrogateEncountered)
            {
                return false;
            }

            _highSurrogateEncountered = false;
            return TryAddBytes(1, 3);
        }

        private bool TryAddBytes(int charsTaken, int bytesAdded)
        {
            if (MaxBytes - _totalBytesAdded < bytesAdded)
            {
                return true;
            }

            _totalCharsTaken += charsTaken;
            _totalBytesAdded += bytesAdded;
            return false;
        }
    }
}

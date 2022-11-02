// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Louis.ArgumentValidation;
using Louis.Diagnostics;

namespace Louis;

/// <summary>
/// Provides methods to perform range checks and for constraining values to be within ranges.
/// </summary>
public static partial class RangeCheck
{
    private static void EnsureValidRange<T>(T min, T max)
        where T : IComparable<T>
    {
        if (min.CompareTo(max) > 0)
        {
            ThrowInvalidRange(min, max);
        }
    }

    private static void EnsureValidComparerAndRange<T>(T min, T max, IComparer<T> comparer)
        where T : notnull
    {
        if (Validated.NotNull(comparer).Compare(min, max) > 0)
        {
            ThrowInvalidRange(min, max);
        }
    }

    private static void ThrowInvalidRange<T>(T min, T max)
        => throw new ArgumentException($"{ExceptionHelper.FormatObject(min)} cannot be greater than {ExceptionHelper.FormatObject(max)}.", nameof(min));
}

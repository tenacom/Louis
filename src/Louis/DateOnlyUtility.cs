// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Louis;

/// <summary>
/// Provides utility methods to work with <see cref="DateOnly"/>.
/// </summary>
public static class DateOnlyUtility
{
    /// <summary>
    /// Gets a <see cref="DateOnly"/> instance whose value represents the current date.
    /// </summary>
    public static DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}

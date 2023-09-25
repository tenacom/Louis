// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Louis;

/// <summary>
/// Provides extension methods for instances of <see cref="DateOnly"/>.
/// </summary>
public static partial class DateOnlyExtensions
{
    /// <summary>
    /// Given a date, returns the first day of the same month.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents the first day of the month of <paramref name="this"/>.</returns>
    public static DateOnly GetStartOfMonth(this DateOnly @this)
        => @this.AddDays(1 - @this.Day);

    /// <summary>
    /// Given a date, returns the last day of the same month.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents the last day of the month of <paramref name="this"/>.</returns>
    public static DateOnly GetEndOfMonth(this DateOnly @this)
        => @this.AddDays(DateTime.DaysInMonth(@this.Year, @this.Month) - @this.Day);
}

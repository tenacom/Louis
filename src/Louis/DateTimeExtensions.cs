// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Louis;

/// <summary>
/// Provides extension methods for instances of <see cref="DateTime"/>.
/// </summary>
public static partial class DateTimeExtensions
{
    /// <summary>
    /// Given a date and time, returns the last tick of the day.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents the last tick on the same day as <paramref name="this"/>.</returns>
    public static DateTime GetEndOfDay(this DateTime @this)
        => @this.Date.AddTicks(TimeConstants.TicksPerDay - 1);

    /// <summary>
    /// Given a date and time, returns midnight on the first day of the same month.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents midnight on the first day of the month of <paramref name="this"/>.</returns>
    public static DateTime GetStartOfMonth(this DateTime @this)
        => @this.Date.AddDays(1 - @this.Day);

    /// <summary>
    /// Given a date and time, returns the last tick of the last day of the same month.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents the last tick of the last day of the month of <paramref name="this"/>.</returns>
    public static DateTime GetEndOfMonth(this DateTime @this)
        => @this.GetLastDayOfMonth().GetEndOfDay();

    /// <summary>
    /// Given a date and time, returns midnight on the last day of the same month.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents midnight on the last day of the month of <paramref name="this"/>.</returns>
    public static DateTime GetLastDayOfMonth(this DateTime @this)
        => @this.Date.AddDays(DateTime.DaysInMonth(@this.Year, @this.Month) - @this.Day);
}

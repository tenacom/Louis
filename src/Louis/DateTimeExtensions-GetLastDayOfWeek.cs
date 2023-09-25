// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using CommunityToolkit.Diagnostics;

namespace Louis;

partial class DateTimeExtensions
{
    /// <summary>
    /// Given a date and time, returns midnight on the last day of the same week, according to the current culture.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents the last day of the week of <paramref name="this"/>,
    /// according to the current culture.</returns>
    /// <seealso cref="CultureInfo.CurrentCulture"/>
    /// <seealso cref="CultureInfo.DateTimeFormat"/>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static DateTime GetLastDayOfWeek(this DateTime @this)
        => GetLastDayOfWeek(@this, CultureInfo.CurrentCulture);

    /// <summary>
    /// Given a date and time, returns midnight on the last day of the same week, according to the specified culture.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <param name="culture">An object that supplies culture-specific information.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents the last day of the week of <paramref name="this"/>,
    /// according to <paramref name="culture"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="culture"/> is <see langword="null"/>.</exception>
    /// <seealso cref="CultureInfo"/>
    /// <seealso cref="CultureInfo.DateTimeFormat"/>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static DateTime GetLastDayOfWeek(this DateTime @this, CultureInfo culture)
    {
        Guard.IsNotNull(culture);
        return GetLastDayOfWeek(@this, culture.DateTimeFormat);
    }

    /// <summary>
    /// Given a date and time, returns midnight on the last day of the same week, according to the specified formatting information.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <param name="dateTimeFormat">An object that supplies culture-specific date formatting information.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents the last day of the week of <paramref name="this"/>,
    /// according to <paramref name="dateTimeFormat"/>.</returns>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static DateTime GetLastDayOfWeek(this DateTime @this, DateTimeFormatInfo dateTimeFormat)
        => GetLastDayOfWeek(@this, dateTimeFormat.FirstDayOfWeek);

    /// <summary>
    /// Returns midnight on the earliest date, greater or equal to a given date and time, whose day of the week is the one preceding a specified day of the week.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateTime"/> on which this method is called.</param>
    /// <param name="firstDayOfWeek">A <see cref="DayOfWeek"/> value representing the day that starts the week.</param>
    /// <returns>An instance of <see cref="DateTime"/> whose value represents the last day of the week of <paramref name="this"/>,
    /// if weeks are considered starting with <paramref name="firstDayOfWeek"/>.</returns>
    /// <seealso cref="DayOfWeek"/>
    public static DateTime GetLastDayOfWeek(this DateTime @this, DayOfWeek firstDayOfWeek)
        => @this.AddDays(6 - DateUtility.GetDaysFromStartOfWeek(@this.DayOfWeek, firstDayOfWeek));
}

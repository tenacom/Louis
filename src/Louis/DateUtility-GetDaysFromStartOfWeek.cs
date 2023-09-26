// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using CommunityToolkit.Diagnostics;

namespace Louis;

partial class DateUtility
{
    /// <summary>
    /// Returns the amount of full days separating a given day of the week from the first day of the same week, according to the current culture.
    /// </summary>
    /// <param name="today">A <see cref="DayOfWeek"/> value.</param>
    /// <returns>An integer number between 0 and 6 representing the days between the start of the week and <paramref name="today"/>,
    /// according to the current culture.</returns>
    /// <seealso cref="CultureInfo.CurrentCulture"/>
    /// <seealso cref="CultureInfo.DateTimeFormat"/>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static int GetDaysFromStartOfWeek(DayOfWeek today)
        => GetDaysFromStartOfWeek(today, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

    /// <summary>
    /// Returns the amount of full days separating a given day of the week from the first day of the same week, according to the specified culture.
    /// </summary>
    /// <param name="today">A <see cref="DayOfWeek"/> value.</param>
    /// <param name="culture">An object that supplies culture-specific information.</param>
    /// <returns>An integer number between 0 and 6 representing the days between the start of the week and <paramref name="today"/>,
    /// according to <paramref name="culture"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="culture"/> is <see langword="null"/>.</exception>
    /// <seealso cref="CultureInfo"/>
    /// <seealso cref="CultureInfo.DateTimeFormat"/>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static int GetDaysFromStartOfWeek(DayOfWeek today, CultureInfo culture)
    {
        Guard.IsNotNull(culture);
        return GetDaysFromStartOfWeek(today, culture.DateTimeFormat.FirstDayOfWeek);
    }

    /// <summary>
    /// Returns the amount of full days separating a given day of the week from the first day of the same week, according to the specified formatting information.
    /// </summary>
    /// <param name="today">A <see cref="DayOfWeek"/> value.</param>
    /// <param name="dateTimeFormat">An object that supplies culture-specific date formatting information.</param>
    /// <returns>An integer number between 0 and 6 representing the days between the start of the week and <paramref name="today"/>,
    /// according to <paramref name="dateTimeFormat"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dateTimeFormat"/> is <see langword="null"/>.</exception>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static int GetDaysFromStartOfWeek(DayOfWeek today, DateTimeFormatInfo dateTimeFormat)
    {
        Guard.IsNotNull(dateTimeFormat);
        return GetDaysFromStartOfWeek(today, dateTimeFormat.FirstDayOfWeek);
    }

    /// <summary>
    /// Returns the amount of full days separating a given day of the week from the first day of the same week, if a given day is considered the first of the week.
    /// </summary>
    /// <param name="today">A <see cref="DayOfWeek"/> value.</param>
    /// <param name="firstDayOfWeek">A <see cref="DayOfWeek"/> value representing the day that starts the week.</param>
    /// <returns>An integer number between 0 and 6 representing the days between the start of the week and <paramref name="today"/>,
    /// if weeks are considered starting with <paramref name="firstDayOfWeek"/>.</returns>
    /// <seealso cref="DayOfWeek"/>
    public static int GetDaysFromStartOfWeek(DayOfWeek today, DayOfWeek firstDayOfWeek)
        => (7 + (int)today - (int)firstDayOfWeek) & 7;
}

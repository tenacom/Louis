// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using CommunityToolkit.Diagnostics;

namespace Louis;

partial class DateOnlyExtensions
{
    /// <summary>
    /// Given a date, returns the first day of the same week, according to the current culture.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateOnly"/> on which this method is called.</param>
    /// <returns>An instance of <see cref="DateOnly"/> whose value represents the first day of the week of <paramref name="this"/>,
    /// according to the current culture.</returns>
    /// <seealso cref="CultureInfo.CurrentCulture"/>
    /// <seealso cref="CultureInfo.DateTimeFormat"/>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static DateOnly GetStartOfWeek(this DateOnly @this)
        => GetStartOfWeek(@this, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);

    /// <summary>
    /// Given a date, returns the first day of the same week, according to the specified culture.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateOnly"/> on which this method is called.</param>
    /// <param name="culture">An object that supplies culture-specific information.</param>
    /// <returns>An instance of <see cref="DateOnly"/> whose value represents the first day of the week of <paramref name="this"/>,
    /// according to <paramref name="culture"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="culture"/> is <see langword="null"/>.</exception>
    /// <seealso cref="CultureInfo"/>
    /// <seealso cref="CultureInfo.DateTimeFormat"/>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static DateOnly GetStartOfWeek(this DateOnly @this, CultureInfo culture)
    {
        Guard.IsNotNull(culture);
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return GetStartOfWeek(@this, culture.DateTimeFormat.FirstDayOfWeek);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Given a date, returns the first day of the same week, according to the specified formatting information.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateOnly"/> on which this method is called.</param>
    /// <param name="dateTimeFormat">An object that supplies culture-specific date formatting information.</param>
    /// <returns>An instance of <see cref="DateOnly"/> whose value represents the first day of the week of <paramref name="this"/>,
    /// according to <paramref name="dateTimeFormat"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dateTimeFormat"/> is <see langword="null"/>.</exception>
    /// <seealso cref="DateTimeFormatInfo.FirstDayOfWeek"/>
    public static DateOnly GetStartOfWeek(this DateOnly @this, DateTimeFormatInfo dateTimeFormat)
    {
        Guard.IsNotNull(dateTimeFormat);
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        return GetStartOfWeek(@this, dateTimeFormat.FirstDayOfWeek);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Returns the most recent date, earlier or equal to a given date, whose day of the week is equal to a specified day of the week.
    /// </summary>
    /// <param name="this">The instance of <see cref="DateOnly"/> on which this method is called.</param>
    /// <param name="firstDayOfWeek">A <see cref="DayOfWeek"/> value representing the day that starts the week.</param>
    /// <returns>An instance of <see cref="DateOnly"/> whose value represents the first day of the week of <paramref name="this"/>,
    /// if weeks are considered starting with <paramref name="firstDayOfWeek"/>.</returns>
    /// <seealso cref="DayOfWeek"/>
    public static DateOnly GetStartOfWeek(this DateOnly @this, DayOfWeek firstDayOfWeek)
        => @this.AddDays(-DateUtility.GetDaysFromStartOfWeek(@this.DayOfWeek, firstDayOfWeek));
}

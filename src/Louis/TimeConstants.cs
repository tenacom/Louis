// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Louis;

/// <summary>
/// Provides constants for typical "magic numbers" used when working with times or time intervals.
/// </summary>
public static class TimeConstants
{
    /// <summary>
    /// Gets the number of ticks in a millisecond.
    /// </summary>
    public const long TicksPerMillisecond = 10000L;

    /// <summary>
    /// Gets the number of milliseconds in a second.
    /// </summary>
    public const int MillisecondsPerSecond = 1000;

    /// <summary>
    /// Gets the number of seconds in a minute.
    /// </summary>
    public const int SecondsPerMinute = 60;

    /// <summary>
    /// Gets the number of minutes in an hour.
    /// </summary>
    public const int MinutesPerHour = 60;

    /// <summary>
    /// Gets the number of hours in a day.
    /// </summary>
    public const int HoursPerDay = 24;

    /// <summary>
    /// Gets the number of days in a week.
    /// </summary>
    public const int DaysPerWeek = 7;

    /// <summary>
    /// Gets the number of ticks in a second.
    /// </summary>
    public const long TicksPerSecond = TicksPerMillisecond * MillisecondsPerSecond;

    /// <summary>
    /// Gets the number of ticks in a minute.
    /// </summary>
    public const long TicksPerMinute = TicksPerSecond * SecondsPerMinute;

    /// <summary>
    /// Gets the number of ticks in an hour.
    /// </summary>
    public const long TicksPerHour = TicksPerMinute * MinutesPerHour;

    /// <summary>
    /// Gets the number of ticks in a day.
    /// </summary>
    public const long TicksPerDay = TicksPerHour * HoursPerDay;

    /// <summary>
    /// Gets the number of ticks in a week.
    /// </summary>
    public const long TicksPerWeek = TicksPerDay * DaysPerWeek;

    /// <summary>
    /// Gets the number of milliseconds in a minute.
    /// </summary>
    public const int MillisecondsPerMinute = MillisecondsPerSecond * SecondsPerMinute;

    /// <summary>
    /// Gets the number of milliseconds in an hour.
    /// </summary>
    public const int MillisecondsPerHour = MillisecondsPerMinute * MinutesPerHour;

    /// <summary>
    /// Gets the number of milliseconds in a day.
    /// </summary>
    public const int MillisecondsPerDay = MillisecondsPerHour * HoursPerDay;

    /// <summary>
    /// Gets the number of milliseconds in a week.
    /// </summary>
    public const int MillisecondsPerWeek = MillisecondsPerDay * DaysPerWeek;

    /// <summary>
    /// Gets the number of seconds in an hour.
    /// </summary>
    public const int SecondsPerHour = SecondsPerMinute * MinutesPerHour;

    /// <summary>
    /// Gets the number of seconds in a day.
    /// </summary>
    public const int SecondsPerDay = SecondsPerHour * HoursPerDay;

    /// <summary>
    /// Gets the number of seconds in a week.
    /// </summary>
    public const int SecondsPerWeek = SecondsPerDay * DaysPerWeek;

    /// <summary>
    /// Gets the number of minutes in a day.
    /// </summary>
    public const int MinutesPerDay = MinutesPerHour * HoursPerDay;

    /// <summary>
    /// Gets the number of minutes in a week.
    /// </summary>
    public const int MinutesPerWeek = MinutesPerDay * DaysPerWeek;

    /// <summary>
    /// Gets the number of hours in a week.
    /// </summary>
    public const int HoursPerWeek = HoursPerDay * DaysPerWeek;
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Louis.Logging;

#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression

partial class LoggerExtensions
{
    /// <summary>
    /// Writes a critical log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">The log message to write.</param>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static void LogCritical(this ILogger @this, string message)
    {
        Guard.IsNotNull(@this);

        if (@this.IsEnabled(LogLevel.Critical))
        {
            @this.Log(LogLevel.Critical, NullEventId, message, null, StaticMessageFormatter);
        }
    }

    /// <summary>
    /// Writes a critical log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">The log message to write.</param>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static void LogCritical(this ILogger @this, EventId eventId, string message)
    {
        Guard.IsNotNull(@this);

        if (@this.IsEnabled(LogLevel.Critical))
        {
            @this.Log(LogLevel.Critical, eventId, message, null, StaticMessageFormatter);
        }
    }

    /// <summary>
    /// Writes a critical log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write.</param>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static void LogCritical(this ILogger @this, Exception? exception, string message)
    {
        Guard.IsNotNull(@this);

        if (@this.IsEnabled(LogLevel.Critical))
        {
            @this.Log(LogLevel.Critical, NullEventId, message, exception, StaticMessageFormatter);
        }
    }

    /// <summary>
    /// Writes a critical log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write.</param>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static void LogCritical(this ILogger @this, EventId eventId, Exception? exception, string message)
    {
        Guard.IsNotNull(@this);

        if (@this.IsEnabled(LogLevel.Critical))
        {
            @this.Log(LogLevel.Critical, eventId, message, exception, StaticMessageFormatter);
        }
    }

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static void LogCritical(
        this ILogger @this,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Critical message)
    {
        // Arguments are validated in the constructor of LogInterpolatedStringHandler.
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Critical, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static void LogCritical(
        this ILogger @this,
        EventId eventId,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Critical message)
    {
        // Arguments are validated in the constructor of LogInterpolatedStringHandler.
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Critical, eventId, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static void LogCritical(
        this ILogger @this,
        Exception? exception,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Critical message)
    {
        // Arguments are validated in the constructor of LogInterpolatedStringHandler.
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Critical, exception, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes a critical log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static void LogCritical(
        this ILogger @this,
        EventId eventId,
        Exception? exception,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Critical message)
    {
        // Arguments are validated in the constructor of LogInterpolatedStringHandler.
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Critical, eventId, exception, template, arguments);
        }
    }
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Louis.Logging;

#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression

partial class LoggerExtensions
{
    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">The log message to write.</param>
    public static void LogError(this ILogger @this, string message)
    {
        if (@this.IsEnabled(LogLevel.Error))
        {
            @this.Log(LogLevel.Error, message);
        }
    }

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">The log message to write.</param>
    public static void LogError(this ILogger @this, EventId eventId, string message)
    {
        if (@this.IsEnabled(LogLevel.Error))
        {
            @this.Log(LogLevel.Error, eventId, message);
        }
    }

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write.</param>
    public static void LogError(this ILogger @this, Exception? exception, string message)
    {
        if (@this.IsEnabled(LogLevel.Error))
        {
            @this.Log(LogLevel.Error, exception, message);
        }
    }

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write.</param>
    public static void LogError(this ILogger @this, EventId eventId, Exception? exception, string message)
    {
        if (@this.IsEnabled(LogLevel.Error))
        {
            @this.Log(LogLevel.Error, eventId, exception, message);
        }
    }

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    public static void LogError(
        this ILogger @this,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Error message)
    {
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Error, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    public static void LogError(
        this ILogger @this,
        EventId eventId,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Error message)
    {
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Error, eventId, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    public static void LogError(
        this ILogger @this,
        Exception? exception,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Error message)
    {
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Error, exception, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes an error log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    public static void LogError(
        this ILogger @this,
        EventId eventId,
        Exception? exception,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Error message)
    {
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Error, eventId, exception, template, arguments);
        }
    }
}

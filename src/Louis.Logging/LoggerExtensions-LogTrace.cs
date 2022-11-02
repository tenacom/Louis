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
    /// Writes a trace log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">The log message to write.</param>
    public static void LogTrace(this ILogger @this, string message)
    {
        if (@this.IsEnabled(LogLevel.Trace))
        {
            @this.Log(LogLevel.Trace, message);
        }
    }

    /// <summary>
    /// Writes a trace log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">The log message to write.</param>
    public static void LogTrace(this ILogger @this, EventId eventId, string message)
    {
        if (@this.IsEnabled(LogLevel.Trace))
        {
            @this.Log(LogLevel.Trace, eventId, message);
        }
    }

    /// <summary>
    /// Writes a trace log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write.</param>
    public static void LogTrace(this ILogger @this, Exception? exception, string message)
    {
        if (@this.IsEnabled(LogLevel.Trace))
        {
            @this.Log(LogLevel.Trace, exception, message);
        }
    }

    /// <summary>
    /// Writes a trace log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write.</param>
    public static void LogTrace(this ILogger @this, EventId eventId, Exception? exception, string message)
    {
        if (@this.IsEnabled(LogLevel.Trace))
        {
            @this.Log(LogLevel.Trace, eventId, exception, message);
        }
    }

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    public static void LogTrace(
        this ILogger @this,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Trace message)
    {
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Trace, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    public static void LogTrace(
        this ILogger @this,
        EventId eventId,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Trace message)
    {
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Trace, eventId, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    public static void LogTrace(
        this ILogger @this,
        Exception? exception,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Trace message)
    {
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Trace, exception, template, arguments);
        }
    }

    /// <summary>
    /// Formats and writes a trace log message.
    /// </summary>
    /// <param name="this">The <see cref="ILogger"/> to write to.</param>
    /// <param name="eventId">The event id associated with the log.</param>
    /// <param name="exception">The exception to log.</param>
    /// <param name="message">The log message to write. This parameter must be an interpolated string.</param>
    public static void LogTrace(
        this ILogger @this,
        EventId eventId,
        Exception? exception,
        [InterpolatedStringHandlerArgument("this")]
        ref LogInterpolatedStringHandler.Trace message)
    {
        if (message.IsEnabled)
        {
            var (template, arguments) = message.GetDataAndDispose();
            @this.Log(LogLevel.Trace, eventId, exception, template, arguments);
        }
    }
}

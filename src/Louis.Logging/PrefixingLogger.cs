// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Louis.Logging;

/// <summary>
/// Wraps an existing logger, prefixing all log messages with a given string.
/// </summary>
public sealed class PrefixingLogger : ILogger
{
    private readonly ILogger _realWrappedLogger;
    private readonly string _realPrefix;

    /// <summary>
    /// Initializes a new instance of the <see cref="PrefixingLogger"/> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> interface to wrap.</param>
    /// <param name="prefix">The desired prefix for log messages.</param>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="logger"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="prefix"/> is <see langword="null"/>.</para>
    /// </exception>
    public PrefixingLogger(ILogger logger, string prefix)
    {
        Guard.IsNotNull(logger);
        Guard.IsNotNull(prefix);
        WrappedLogger = logger;
        Prefix = prefix;
        if (WrappedLogger is PrefixingLogger prefixingLogger)
        {
            _realWrappedLogger = prefixingLogger._realWrappedLogger;
            _realPrefix = prefix + prefixingLogger._realPrefix;
        }
        else
        {
            _realWrappedLogger = WrappedLogger;
            _realPrefix = Prefix;
        }
    }

    /// <summary>
    /// Gets the logger wrapped by this instance.
    /// </summary>
    public ILogger WrappedLogger { get; }

    /// <summary>
    /// Gets the prefix applied to log messages.
    /// </summary>
    public string Prefix { get; }

    /// <inheritdoc/>
    IDisposable? ILogger.BeginScope<TState>(TState state) => WrappedLogger.BeginScope(state);

    /// <inheritdoc/>
    bool ILogger.IsEnabled(LogLevel logLevel) => WrappedLogger.IsEnabled(logLevel);

    /// <inheritdoc/>
    void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => _realWrappedLogger.Log(logLevel, eventId, state, exception, (s, e) => _realPrefix + formatter(s, e));
}

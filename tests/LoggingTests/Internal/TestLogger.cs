// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Internal;

public class TestLogger : ILogger
{
    private readonly bool _enableTrace;
    private readonly bool _enableDebug;
    private readonly bool _enableInformation;
    private readonly bool _enableWarning;
    private readonly bool _enableError;
    private readonly bool _enableCritical;

    internal TestLogger(bool enable = true)
        : this(enableOthers: enable)
    {
    }

    internal TestLogger(LogLevel singleLevel, bool enable = true)
        : this(
            enableTrace: singleLevel == LogLevel.Trace ? enable : null,
            enableDebug: singleLevel == LogLevel.Debug ? enable : null,
            enableInformation: singleLevel == LogLevel.Information ? enable : null,
            enableWarning: singleLevel == LogLevel.Warning ? enable : null,
            enableError: singleLevel == LogLevel.Error ? enable : null,
            enableCritical: singleLevel == LogLevel.Critical ? enable : null,
            enableOthers: !enable)
    {
    }

    internal TestLogger(
        bool? enableTrace = null,
        bool? enableDebug = null,
        bool? enableInformation = null,
        bool? enableWarning = null,
        bool? enableError = null,
        bool? enableCritical = null,
        bool enableOthers = false)
    {
        _enableTrace = enableTrace ?? enableOthers;
        _enableDebug = enableDebug ?? enableOthers;
        _enableInformation = enableInformation ?? enableOthers;
        _enableWarning = enableWarning ?? enableOthers;
        _enableError = enableError ?? enableOthers;
        _enableCritical = enableCritical ?? enableOthers;
    }

    public (LogLevel LogLevel, bool WasEnabled, string Message) LastEntry { get; private set; } = (LogLevel.None, false, string.Empty);

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel)
        => logLevel switch {
            LogLevel.Trace => _enableTrace,
            LogLevel.Debug => _enableDebug,
            LogLevel.Information => _enableInformation,
            LogLevel.Warning => _enableWarning,
            LogLevel.Error => _enableError,
            LogLevel.Critical => _enableCritical,
            _ => false,
        };

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        => LastEntry = (logLevel, IsEnabled(logLevel), formatter(state, exception));
}

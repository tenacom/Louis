// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace UnitTests;

public sealed class PrefixingLoggerTests
{
    [Fact]
    public void Constructor_WithLoggerNull_ThrowsArgumentNullException()
    {
        var func = () => new PrefixingLogger(null!, "Prefix:");
        func.Should().Throw<ArgumentNullException>()
            .Where(e => e.ParamName == "logger");
    }

    [Fact]
    public void Constructor_WithPrefixNull_ThrowsArgumentNullException()
    {
        var logger = new TestLogger(enable: true);
        var func = () => new PrefixingLogger(logger, null!);
        func.Should().Throw<ArgumentNullException>()
            .Where(e => e.ParamName == "prefix");
    }

    [Fact]
    public void WrappedLogger_IsTheLoggerPassedToConstructor()
    {
        var logger = new TestLogger(enable: true);
        var prefixingLogger1 = new PrefixingLogger(logger, "foo,");
        var prefixingLogger2 = new PrefixingLogger(prefixingLogger1, "bar,");
        prefixingLogger1.WrappedLogger.Should().Be(logger);
        prefixingLogger2.WrappedLogger.Should().Be(prefixingLogger1);
    }

    [Fact]
    public void Prefix_IsThePrefixPassedToConstructor()
    {
        var logger = new TestLogger(enable: true);
        var prefixingLogger1 = new PrefixingLogger(logger, "foo,");
        var prefixingLogger2 = new PrefixingLogger(prefixingLogger1, "bar,");
        prefixingLogger1.Prefix.Should().Be("foo,");
        prefixingLogger2.Prefix.Should().Be("bar,");
    }

    [Fact]
    public void Log_WithPrefix_PrefixesLogMessages()
    {
        var logger = new TestLogger(enable: true);
        var prefixingLogger = new PrefixingLogger(logger, "foo,");
        prefixingLogger.LogInformation("bar");
        logger.LastEntry.Message.Should().Be("foo,bar");
    }

    [Fact]
    public void Log_WithNestedPrefixingLoggers_ConcatenatesPrefixes()
    {
        var logger = new TestLogger(enable: true);
        var prefixingLogger1 = new PrefixingLogger(logger, "foo,");
        var prefixingLogger2 = new PrefixingLogger(prefixingLogger1, "bar,");
        prefixingLogger2.LogInformation("baz");
        logger.LastEntry.Message.Should().Be("bar,foo,baz");
    }
}

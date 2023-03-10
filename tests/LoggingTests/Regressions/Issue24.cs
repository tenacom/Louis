// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Regressions;

public class Issue24
{
    private const string GotchaMessage = "Gotcha!";

    [Fact]
    public void LogTrace_WhenTraceEnabled_EvaluatesInterpolations()
    {
        var logger = new TestLogger(enableTrace: true, enableOthers: false);
        logger.Invoking(l => l.LogTrace($"Let's go {Boom()}!"))
              .Should().Throw<InvalidOperationException>()
              .WithMessage(GotchaMessage);
    }

    [Fact]
    public void LogTrace_WhenTraceNotEnabled_DoesNotEvaluateInterpolations()
    {
        var logger = new TestLogger(enableTrace: false, enableOthers: true);
        logger.Invoking(l => l.LogTrace($"Let's go {Boom()}!"))
              .Should().NotThrow();
    }

    [Fact]
    public void LogDebug_WhenDebugEnabled_EvaluatesInterpolations()
    {
        var logger = new TestLogger(enableDebug: true, enableOthers: false);
        logger.Invoking(l => l.LogDebug($"Let's go {Boom()}!"))
              .Should().Throw<InvalidOperationException>()
              .WithMessage(GotchaMessage);
    }

    [Fact]
    public void LogDebug_WhenDebugNotEnabled_DoesNotEvaluateInterpolations()
    {
        var logger = new TestLogger(enableDebug: false, enableOthers: true);
        logger.Invoking(l => l.LogDebug($"Let's go {Boom()}!"))
              .Should().NotThrow();
    }

    [Fact]
    public void LogInformation_WhenInformationEnabled_EvaluatesInterpolations()
    {
        var logger = new TestLogger(enableInformation: true, enableOthers: false);
        logger.Invoking(l => l.LogInformation($"Let's go {Boom()}!"))
              .Should().Throw<InvalidOperationException>()
              .WithMessage(GotchaMessage);
    }

    [Fact]
    public void LogInformation_WhenInformationNotEnabled_DoesNotEvaluateInterpolations()
    {
        var logger = new TestLogger(enableInformation: false, enableOthers: true);
        logger.Invoking(l => l.LogInformation($"Let's go {Boom()}!"))
              .Should().NotThrow();
    }

    [Fact]
    public void LogWarning_WhenWarningEnabled_EvaluatesInterpolations()
    {
        var logger = new TestLogger(enableWarning: true, enableOthers: false);
        logger.Invoking(l => l.LogWarning($"Let's go {Boom()}!"))
              .Should().Throw<InvalidOperationException>()
              .WithMessage(GotchaMessage);
    }

    [Fact]
    public void LogWarning_WhenWarningNotEnabled_DoesNotEvaluateInterpolations()
    {
        var logger = new TestLogger(enableWarning: false, enableOthers: true);
        logger.Invoking(l => l.LogWarning($"Let's go {Boom()}!"))
              .Should().NotThrow();
    }

    [Fact]
    public void LogError_WhenErrorEnabled_EvaluatesInterpolations()
    {
        var logger = new TestLogger(enableError: true, enableOthers: false);
        logger.Invoking(l => l.LogError($"Let's go {Boom()}!"))
              .Should().Throw<InvalidOperationException>()
              .WithMessage(GotchaMessage);
    }

    [Fact]
    public void LogError_WhenErrorNotEnabled_DoesNotEvaluateInterpolations()
    {
        var logger = new TestLogger(enableError: false, enableOthers: true);
        logger.Invoking(l => l.LogError($"Let's go {Boom()}!"))
              .Should().NotThrow();
    }

    [Fact]
    public void LogCritical_WhenCriticalEnabled_EvaluatesInterpolations()
    {
        var logger = new TestLogger(enableCritical: true, enableOthers: false);
        logger.Invoking(l => l.LogCritical($"Let's go {Boom()}!"))
              .Should().Throw<InvalidOperationException>()
              .WithMessage(GotchaMessage);
    }

    [Fact]
    public void LogCritical_WhenCriticalNotEnabled_DoesNotEvaluateInterpolations()
    {
        var logger = new TestLogger(enableCritical: false, enableOthers: true);
        logger.Invoking(l => l.LogCritical($"Let's go {Boom()}!"))
              .Should().NotThrow();
    }

    private int Boom() => throw new InvalidOperationException(GotchaMessage);
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Louis.Logging.Internal;

internal static class InternalGuard
{
    public static void IsValidLogLevelForWriting(LogLevel value, [CallerArgumentExpression("value")] string name = "")
    {
        if (value is LogLevel.Trace or LogLevel.Debug or LogLevel.Information or LogLevel.Warning or LogLevel.Error or LogLevel.Critical)
        {
            return;
        }

        ThrowHelper.ThrowArgumentException(name, $"Parameter should be a valid log level for writing (value = {value}).");
    }
}

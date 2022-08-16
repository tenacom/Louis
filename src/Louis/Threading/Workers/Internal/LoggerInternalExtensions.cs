// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;

namespace Louis.Threading.Workers.Internal;

internal static class LoggerInternalExtensions
{
    private static readonly Action<ILogger, AsyncWorkerState, Exception?> LogServiceStateChanged
        = LoggerMessage.Define<AsyncWorkerState>(
            LogLevel.Debug,
            new EventId(1, nameof(ServiceStateChanged)),
            "Worker state changed to {State}");

    private static readonly Action<ILogger, Exception?> LogOperationCanceled
        = LoggerMessage.Define(
            LogLevel.Information,
            new EventId(2, nameof(OperationCanceled)),
            "Operation canceled");

    public static void ServiceStateChanged(this ILogger @this, AsyncWorkerState state)
        => LogServiceStateChanged(@this, state, null);

    public static void OperationCanceled(this ILogger @this) => LogOperationCanceled(@this, null);
}

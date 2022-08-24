// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Louis.Logging;

#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented

partial struct LogInterpolatedStringHandler
{
    [InterpolatedStringHandler]
    public ref struct Warning
    {
        private LogInterpolatedStringHandler _handler;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Warning(int literalLength, int formattedCount, ILogger logger, out bool isEnabled)
        {
            _handler = new(literalLength, formattedCount, logger, LogLevel.Critical, out isEnabled);
        }

        internal bool IsEnabled
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _handler.IsEnabled;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AppendLiteral(string s)
            => _handler.AppendLiteral(s);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - We need CallerArgumentExpression here
        public void AppendFormatted(object? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
            => _handler.AppendFormatted(value, alignment, format, name);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - We need CallerArgumentExpression here
        public void AppendFormatted<T>(T? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
            where T : struct
            => _handler.AppendFormatted(value, alignment, format, name);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal (string Template, object?[] Arguments) GetDataAndDispose()
            => _handler.GetDataAndClear();
    }
}

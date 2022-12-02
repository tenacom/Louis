// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Louis.Logging;

// Disable some documentation-related warnings because this type is for internal use.
#pragma warning disable SA1611 // Element parameters should be documented
#pragma warning disable SA1618 // Generic type parameters should be documented
#pragma warning disable SA1642 // Constructor summary documentation should begin with standard text

// This type is internal-use-only, but interpolation string handlers have to be public
#pragma warning disable CA1034 // Nested types should not be visible

partial struct LogInterpolatedStringHandler
{
    /// <summary>
    /// This type is only meant for internal use by L.o.U.I.S. and should not be used directly.
    /// </summary>
    [InterpolatedStringHandler]
    public ref struct Critical
    {
        private LogInterpolatedStringHandler _handler;

        /// <summary>
        /// This type is only meant for internal use by L.o.U.I.S. and should not be used directly.
        /// </summary>
        public Critical(int literalLength, int formattedCount, ILogger @this, out bool isEnabled)
        {
            _handler = new(literalLength, formattedCount, @this, LogLevel.Critical, out isEnabled);
        }

        internal bool IsEnabled => _handler.IsEnabled;

        /// <summary>
        /// This type is only meant for internal use by L.o.U.I.S. and should not be used directly.
        /// </summary>
        public void AppendLiteral(string s) => _handler.AppendLiteral(s);

        /// <summary>
        /// This type is only meant for internal use by L.o.U.I.S. and should not be used directly.
        /// </summary>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - We need CallerArgumentExpression here
        public void AppendFormatted(object? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
            => _handler.AppendFormatted(value, alignment, format, name);

        /// <summary>
        /// This type is only meant for internal use by L.o.U.I.S. and should not be used directly.
        /// </summary>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - We need CallerArgumentExpression here
        public void AppendFormatted<T>(T? value, int alignment = 0, string? format = null, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
            where T : struct
            => _handler.AppendFormatted(value, alignment, format, name);

        internal (string Template, object?[] Arguments) GetDataAndDispose() => _handler.GetDataAndClear();
    }
}

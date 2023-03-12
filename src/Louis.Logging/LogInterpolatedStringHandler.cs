// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using CommunityToolkit.Diagnostics;
using Louis.Logging.Internal;
using Louis.Text;
using Microsoft.Extensions.Logging;

namespace Louis.Logging;

// Disable some documentation-related warnings because this type is for internal use.
#pragma warning disable SA1611 // Element parameters should be documented
#pragma warning disable SA1618 // Generic type parameters should be documented
#pragma warning disable SA1642 // Constructor summary documentation should begin with standard text

/// <summary>
/// This type is only meant for internal use by L.o.U.I.S. and should not be used directly.
/// </summary>
[InterpolatedStringHandler]
public ref partial struct LogInterpolatedStringHandler
{
    private const int InitialTemplateCapacity = 1024;

    private static readonly ThreadLocal<StringBuilder> TemplateBuilder = new(static () => new(InitialTemplateCapacity));

    private object?[] _arguments = null!;
    private int _argumentIndex;

    /// <summary>
    /// This constructor is only meant for internal use by L.o.U.I.S. and should not be used directly.
    /// </summary>
    // This constructor is the first place where we can check for a null @this in extension methods taking a LogInterpolatedStringHandler.
    // We want a NullArgumentException to refer to the public-facing @this parameter, hence the name.
    public LogInterpolatedStringHandler(int literalLength, int formattedCount, ILogger @this, LogLevel logLevel, out bool isEnabled)
    {
        Guard.IsNotNull(@this);
        InternalGuard.IsValidLogLevelForWriting(logLevel);

        _argumentIndex = 0;
        isEnabled = IsEnabled = @this.IsEnabled(logLevel);
        if (isEnabled)
        {
            _arguments = new object?[formattedCount];
        }
    }

    internal bool IsEnabled { get; }

    /// <summary>
    /// This method is only meant for internal use by L.o.U.I.S. and should not be used directly.
    /// </summary>
#pragma warning disable CA1822 // Mark members as static - Would probably violate the custom string interpolation protocol
    public void AppendLiteral(string s)
#pragma warning restore CA1822 // Mark members as static
    {
        var tb = TemplateBuilder.Value!;
        var source = s.AsSpan();
        while (!source.IsEmpty)
        {
            var escapeIndex = source.IndexOfAny('{', '}');
            if (escapeIndex < 0)
            {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                _ = tb.Append(source);
#else
                _ = tb.Append(source.ToString());
#endif
                break;
            }

            if (escapeIndex > 0)
            {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                _ = tb.Append(source[..escapeIndex]);
#else
                _ = tb.Append(source[..escapeIndex].ToString());
#endif
            }

            _ = tb.Append(source[escapeIndex], 2);
            source = source[(escapeIndex + 1)..];
        }
    }

    /// <summary>
    /// This method is only meant for internal use by L.o.U.I.S. and should not be used directly.
    /// </summary>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - We need CallerArgumentExpression here
    public void AppendFormatted(object? value, int alignment = 0, string? format = null, [CallerArgumentExpression(nameof(value))] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
    {
        var tb = TemplateBuilder.Value!;
        _arguments[_argumentIndex] = value;
        var isNamePrefixed = false;
        var argumentName = ReadOnlySpan<char>.Empty;
        if (!string.IsNullOrEmpty(name))
        {
            isNamePrefixed = name[0] == '@';
            argumentName = name.AsSpan(isNamePrefixed ? 1 : 0);
            if (!UnicodeCharacterUtility.IsValidIdentifier(argumentName))
            {
                argumentName = ReadOnlySpan<char>.Empty;
            }
        }

        if (argumentName.IsEmpty)
        {
            argumentName = ("Arg" + _argumentIndex.ToString(CultureInfo.InvariantCulture)).AsSpan();
        }

        tb.Append('{');
        if (isNamePrefixed)
        {
            tb.Append('@');
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        tb.Append(argumentName);
#else
        tb.Append(argumentName.ToString());
#endif
        if (alignment != 0)
        {
            tb.Append(',');
            tb.Append(alignment);
        }

        if (format != null)
        {
            tb.Append(':');
            tb.Append(format);
        }

        tb.Append('}');
        _argumentIndex++;
    }

    /// <summary>
    /// This method is only meant for internal use by L.o.U.I.S. and should not be used directly.
    /// </summary>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - We need CallerArgumentExpression here
    public void AppendFormatted<T>(T? value, int alignment = 0, string? format = null, [CallerArgumentExpression(nameof(value))] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : struct
        => AppendFormatted(value.HasValue ? value.GetValueOrDefault() : null, alignment, format, name);

    internal (string Template, object?[] Arguments) GetDataAndClear()
    {
        var tb = TemplateBuilder.Value!;
        var template = tb.ToString();
        _ = tb.Clear();
        var arguments = _arguments;
        _arguments = null!;
        return (template, arguments);
    }
}

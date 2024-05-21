// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET6_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Louis.Text;

#pragma warning disable CA1815 // Override equals and operator equals on value types - They would never be used anyway
#pragma warning disable RS0027 // API with optional parameter(s) should have the most parameters amongst its public overloads - Just mimicking StringBuilder.AppendInterpolatedStringHandler's APIs here.

/// <summary>
/// <para>Provides an interpolated string handler that only performs formatting if a given condition is <see langword="true"/>.</para>
/// <para>This type is only available on .NET 6.0 and greater.</para>
/// <para>This type is only meant to be used by compiler-generated code. As such, it performs no parameter validation whatsoever.</para>
/// <example>
/// <para>The following code shows how this type can be used as an interpolated string handler:</para>
/// <code>
/// using System;
/// using System.Runtime.CompilerServices;
///
/// public static class ConsoleUtility
/// {
///     // Write a message to the console only if condition is true.
///     // Usage example: ConsoleUtility.WriteLineIf(score > 100, $"Your score is {score}. That's awesome!");
///     // String interpolation will only take place if necessary.
///     public static void WriteLineIf(bool condition, [InterpolatedStringHandlerArgument(nameof(condition))] ref TrueConditionInterpolatedStringHandler message)
///     {
///         if (condition)
///         {
///             Console.WriteLine(message.ToStringAndClear());
///         }
///     }
/// }
/// </code>
/// </example>
/// </summary>
[InterpolatedStringHandler]
public struct TrueConditionInterpolatedStringHandler
{
    private StringBuilder? _builder;
    private StringBuilder.AppendInterpolatedStringHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrueConditionInterpolatedStringHandler"/> struct.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount">The number of interpolation expressions in the interpolated string.</param>
    /// <param name="condition">The condition that determines whether formatting will actually take place.</param>
    /// <param name="shouldAppend">A value indicating whether formatting should proceed.</param>
    /// <remarks>
    /// <para>This constructor is only intended to be called by compiler-generated code.
    /// As such, it doesn't perform argument validation.</para>
    /// </remarks>
    public TrueConditionInterpolatedStringHandler(int literalLength, int formattedCount, bool condition, out bool shouldAppend)
    {
        shouldAppend = condition;
        if (shouldAppend)
        {
            var builder = new StringBuilder();
            _handler = new(literalLength, formattedCount, builder);
            _builder = builder;
        }
    }

    /// <summary>
    /// Extracts the built string and frees any allocated memory.
    /// </summary>
    /// <returns>The built string.</returns>
    public string ToStringAndClear()
    {
        if (_builder is null)
        {
            return string.Empty;
        }

        var message = _builder.ToString();
        this = default;
        return message;
    }

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendLiteral(string)"/>
    public void AppendLiteral(string value) => _handler.AppendLiteral(value);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted{T}(T)"/>
    public void AppendFormatted<T>(T value) => _handler.AppendFormatted(value);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted{T}(T, string?)"/>
    public void AppendFormatted<T>(T value, string? format) => _handler.AppendFormatted(value, format);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted{T}(T, int)"/>
    public void AppendFormatted<T>(T value, int alignment) => _handler.AppendFormatted(value, alignment);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted{T}(T, int, string?)"/>
    public void AppendFormatted<T>(T value, int alignment, string? format) => _handler.AppendFormatted(value, alignment, format);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted(ReadOnlySpan{char})"/>
    public void AppendFormatted(ReadOnlySpan<char> value) => _handler.AppendFormatted(value);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted(ReadOnlySpan{char}, int, string?)"/>
    public void AppendFormatted(ReadOnlySpan<char> value, int alignment = 0, string? format = null) => _handler.AppendFormatted(value, alignment, format);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted(string?)"/>
    public void AppendFormatted(string? value) => _handler.AppendFormatted(value);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted(string?, int, string?)"/>
    public void AppendFormatted(string? value, int alignment = 0, string? format = null) => _handler.AppendFormatted(value, alignment, format);

    /// <inheritdoc cref="StringBuilder.AppendInterpolatedStringHandler.AppendFormatted(object?, int, string?)"/>
    public void AppendFormatted(object? value, int alignment = 0, string? format = null) => _handler.AppendFormatted(value, alignment, format);
}

#endif

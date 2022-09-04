// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Louis.ArgumentValidation;

/// <summary>
/// Provides helper methods for argument validation.
/// </summary>
[StackTraceHidden]
public static class ArgHelper
{
    private const string DefaultArgumentExceptionMessage = "{@} is not a valid value.";
    private const string DefaultArgumentOutOfRangeExceptionMessage = "{@} is not in the allowed range.";
    private const string NullValue = "<null>";

    /// <summary>
    /// <para>Constructs and returns an <see cref="ArgumentException"/> with a custom message format.</para>
    /// <para>See the Remarks section for more details.</para>
    /// </summary>
    /// <param name="name">The name of the argument.</param>
    /// <param name="value">The value of the argument.</param>
    /// <param name="message">
    /// <para>The format of the exception message (see <see cref="FormatMessage"/> for details).</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns>A newly-constructed <see cref="ArgumentException"/>.</returns>
    /// <exception cref="FormatException">A custom format in <paramref name="message"/>is invalid.</exception>
    public static Exception MakeArgumentException(string name, object? value, string? message)
        => new ArgumentException(FormatMessage(message ?? DefaultArgumentExceptionMessage, value), name);

    /// <summary>
    /// <para>Constructs and returns an <see cref="ArgumentException"/> with a custom message format.</para>
    /// <para>See the Remarks section for more details.</para>
    /// </summary>
    /// <param name="name">The name of the argument.</param>
    /// <param name="value">The value of the argument.</param>
    /// <param name="innerException">The exception that is the cause of the returned exception.</param>
    /// <param name="message">
    /// <para>The format of the exception message (see <see cref="FormatMessage"/> for details).</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns>A newly-constructed <see cref="ArgumentException"/>.</returns>
    /// <exception cref="FormatException">A custom format in <paramref name="message"/>is invalid.</exception>
    public static Exception MakeArgumentException(string name, object? value, Exception innerException, string? message)
        => throw new ArgumentException(FormatMessage(message ?? DefaultArgumentExceptionMessage, value), name, innerException);

    /// <summary>
    /// <para>Constructs and returns an <see cref="ArgumentOutOfRangeException"/> with a custom message format.</para>
    /// <para>See the Remarks section for more details.</para>
    /// </summary>
    /// <param name="name">The name of the argument.</param>
    /// <param name="value">The value of the argument.</param>
    /// <param name="message">
    /// <para>The format of the exception message (see <see cref="FormatMessage"/> for details).</para>
    /// <para>If this parameter is <see langword="null"/>, a default message will be used.</para>
    /// </param>
    /// <returns>A newly-constructed <see cref="ArgumentOutOfRangeException"/>.</returns>
    /// <exception cref="FormatException">A custom format in <paramref name="message"/>is invalid.</exception>
    public static Exception MakeArgumentOutOfRangeException(string name, object? value, string? message)
        => new ArgumentOutOfRangeException(name, FormatMessage(message ?? DefaultArgumentOutOfRangeExceptionMessage, value));

    /// <summary>
    /// Prepares an exception message by replacing the format item or items in the specified string
    /// with the string representation of the given object.
    /// </summary>
    /// <param name="message">
    /// <para>The format of the exception message.</para>
    /// <para>If this parameter is <see langword="null"/> or an empty string (<c>""</c>),
    /// it will be returned unchanged and no exception will be raised.</para>
    /// <para>If this parameter does not contain any format item, it will be returned unchanged.</para>
    /// </param>
    /// <param name="value">The value of the argument that caused the exception whose message is being formatted.</param>
    /// <returns>A formatted exception message, if there is at least one format item in <paramref name="message"/>;
    /// otherwise, <paramref name="message"/>.</returns>
    /// <remarks>
    /// <para>If <paramref name="value"/> is not <see langword="null"/>, every occurrence of the string <c>{@}</c> in the exception message
    /// will be replaced by calling <see cref="object.ToString()">ToString()</see> on <paramref name="value"/>.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/>, every occurrence of the string <c>{@}</c> in the exception message
    /// will be replaced by the string <c>%lt;null&gt;</c>.</para>
    /// <para>If the type of <paramref name="value"/> is known to the caller, it is possible to specify a custom format:
    /// for example, if <paramref name="value"/> is an integer, <c>{@8X}</c> will be replaced by the result of calling
    /// <see cref="string.Format(System.IFormatProvider?,string,object?)"><c>string.Format(CultureInfo.CurrentCulture, "{0:8X}, value)</c></see>.</para>
    /// </remarks>
    public static string FormatMessage(string message, object? value)
    {
        if (string.IsNullOrEmpty(message))
        {
            return message;
        }

        var len = message.Length;
        var maxStartPos = len - 3;
        var startPos = message.IndexOf("{@", StringComparison.Ordinal);
        if (startPos < 0 || startPos > maxStartPos)
        {
            return message;
        }

        var hasValue = value is not null;
        var sb = new StringBuilder(len);
        var pos = 0;
        while (pos < maxStartPos)
        {
            var endPos = message.IndexOf('}', startPos + 2);
            if (endPos < 0)
            {
                break;
            }

            _ = sb.Append(message, pos, startPos - pos);
            if (endPos > startPos + 2)
            {
                if (hasValue)
                {
#if NETSTANDARD || NETFRAMEWORK
                    var format = string.Concat("{0:", message.Substring(startPos + 2, endPos - startPos - 2), "}");
#else
                    var format = string.Concat("{0:", message.AsSpan(startPos + 2, endPos - startPos - 2), "}");
#endif
                    _ = sb.AppendFormat(CultureInfo.CurrentCulture, format, value);
                }
                else
                {
                    _ = sb.Append(NullValue);
                }
            }
            else
            {
                _ = sb.Append(message, pos, startPos - pos).Append(value ?? NullValue);
            }

            pos = endPos + 1;
            startPos = message.IndexOf("{@", pos, StringComparison.Ordinal);
            if (startPos < 0 || startPos > maxStartPos)
            {
                break;
            }
        }

        if (pos < len)
        {
            _ = sb.Append(message, pos, len - pos);
        }

        return sb.ToString();
    }
}

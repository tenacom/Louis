// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using Louis.Diagnostics;
using Louis.Text;

namespace Louis.ArgumentValidation;

/// <summary>
/// Provides helper methods for constructing and throwing exceptions.
/// </summary>
partial class ArgHelper
{
    private const string DefaultArgumentExceptionFormat = "{@} is not a valid value.";
    private const string NullValue = "<null>";

    /// <summary>
    /// Prepares the message for an <see cref="ArgumentException">ArgumentException</see>
    /// by replacing each format item in the specified <paramref name="format"/> string
    /// with a string representation of the given <paramref name="value"/>.
    /// </summary>
    /// <param name="format">
    /// <para>The format of the exception message.</para>
    /// <para>If this parameter is <see langword="null"/>, a default format will be used instead.</para>
    /// <para>If this parameter does not contain any format item, it will be returned unchanged.</para>
    /// </param>
    /// <param name="value">The value of the argument that caused the exception whose message is being formatted.</param>
    /// <returns>A formatted exception message, if there is at least one format item in <paramref name="format"/>;
    /// otherwise, <paramref name="format"/> unchanged.</returns>
    /// <exception cref="InternalErrorException"><paramref name="format"/> contains one or more items with
    /// format strings not valid for the type of <paramref name="value"/>.</exception>
    /// <remarks>
    /// <para>A format item is defined as either the string <c>{@}</c>, or the string <c>{@FMT}</c> where <c>FMT</c> is a
    /// standard or custom <see href="https://docs.microsoft.com/en-us/dotnet/standard/base-types/formatting-types">format string</see>.
    /// Please be aware that, unlike <see cref="string.Format(string,object)"/> and similar methods, this method does NOT support width and alignment specifiers.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/>, <c>FMT</c> will be ignored if present
    /// and each format item will be substituted with the string <c>%lt;null&gt;</c>.</para>
    /// <para>If <paramref name="value"/> is a <see langword="string"/>, <c>FMT</c> will be ignored if present
    /// and each format item will be substituted by converting <paramref name="value"/> to a
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#quoted-string-literals">quoted string literal</see>;
    /// if the length of <paramref name="value"/> exceeds 43 characters, only the first 20 and last 20 characters will be shown, with
    /// an ellipsis (three dots) between them to signify that there were more.</para>
    /// <para>If <paramref name="value"/> implements the <see cref="IFormattable"/> interface, each format item will be substituted
    /// by calling <see cref="IFormattable.ToString(string,IFormatProvider)"/> on <paramref name="value"/> with <c>FMT</c>
    /// and <see cref="CultureInfo.InvariantCulture"/>. If <c>FMT</c> is not present, the empty string will be used.</para>
    /// <para>If <paramref name="value"/> does not implement the <see cref="IFormattable"/> interface, <c>FMT</c> will be ignored if present
    /// and each format item will be substituted with the result of calling <see cref="object.ToString()">ToString()</see> on <paramref name="value"/>.</para>
    /// </remarks>
    public static string FormatArgumentExceptionMessage(string? format, object? value)
    {
        format ??= DefaultArgumentExceptionFormat;
        var len = format.Length;
        var maxStartPos = len - 3;
        var startPos = format.IndexOf("{@", StringComparison.Ordinal);
        if (startPos < 0 || startPos > maxStartPos)
        {
            return format;
        }

        var sb = new StringBuilder(len);
        var pos = 0;
        while (pos < maxStartPos)
        {
            var valueFormatPos = startPos + 2;
            var endPos = format.IndexOf('}', valueFormatPos);
            if (endPos < 0)
            {
                break;
            }

            _ = sb.Append(format, pos, startPos - pos);
            switch (value)
            {
                case string str:
                    _ = sb.AppendClippedQuotedLiteral(str, 20, 20, false);
                    break;
                case IFormattable formattable:
                    {
                        try
                        {
                            _ = sb.Append(formattable.ToString(format[valueFormatPos..endPos], CultureInfo.InvariantCulture));
                        }
                        catch (FormatException e)
                        {
                            throw new InternalErrorException($"Invalid format for {nameof(FormatArgumentExceptionMessage)}.", e);
                        }

                        break;
                    }

                default:
                    _ = sb.Append(value ?? NullValue);
                    break;
            }

            pos = endPos + 1;
            startPos = format.IndexOf("{@", pos, StringComparison.Ordinal);
            if (startPos < 0 || startPos > maxStartPos)
            {
                break;
            }
        }

        if (pos < len)
        {
            _ = sb.Append(format, pos, len - pos);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Prepares the message for an <see cref="ArgumentException">ArgumentException</see>
    /// by replacing each format item in the specified <paramref name="format"/> string
    /// with a string representation of the given <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The underlying type of <paramref name="value"/>.</typeparam>
    /// <param name="format">
    /// <para>The format of the exception message.</para>
    /// <para>If this parameter is <see langword="null"/>, a default format will be used instead.</para>
    /// <para>If this parameter does not contain any format item, it will be returned unchanged.</para>
    /// </param>
    /// <param name="value">The value of the argument that caused the exception whose message is being formatted.</param>
    /// <returns>A formatted exception message, if there is at least one format item in <paramref name="format"/>;
    /// otherwise, <paramref name="format"/> unchanged.</returns>
    /// <exception cref="InternalErrorException"><paramref name="format"/> contains one or more items with
    /// format strings not valid for the underlying type of <paramref name="value"/>.</exception>
    /// <remarks>
    /// <para>A format item is defined as either the string <c>{@}</c>, or the string <c>{@FMT}</c> where <c>FMT</c> is a
    /// standard or custom <see href="https://docs.microsoft.com/en-us/dotnet/standard/base-types/formatting-types">format string</see>.
    /// Please be aware that, unlike <see cref="string.Format(string,object)"/> and similar methods, this method does NOT support width and alignment specifiers.</para>
    /// <para>If <paramref name="value"/> is <see langword="null"/>, <c>FMT</c> will be ignored if present
    /// and each format item will be substituted with the string <c>%lt;null&gt;</c>.</para>
    /// <para>If <paramref name="value"/> implements the <see cref="IFormattable"/> interface, each format item will be substituted
    /// by calling <see cref="IFormattable.ToString(string,IFormatProvider)"/> on <paramref name="value"/> with <c>FMT</c>
    /// and <see cref="CultureInfo.InvariantCulture"/>. If <c>FMT</c> is not present, the empty string will be used.</para>
    /// <para>If <paramref name="value"/> does not implement the <see cref="IFormattable"/> interface, <c>FMT</c> will be ignored if present
    /// and each format item will be substituted with the result of calling <see cref="object.ToString()">ToString()</see> on <paramref name="value"/>.</para>
    /// </remarks>
    public static string FormatArgumentExceptionMessage<T>(string? format, T? value)
        where T : struct
#pragma warning disable IDE0030 // Use coalesce expression - The ternary operator better clarifies the choice between Value (of type T) and an ACTUAL null (not default(T?)).

        // ReSharper disable once MergeConditionalExpression - See before
        => FormatArgumentExceptionMessage(format, value.HasValue ? value.Value : null);
#pragma warning restore IDE0030 // Use coalesce expression
}

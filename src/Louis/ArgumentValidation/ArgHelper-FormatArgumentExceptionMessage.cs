// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Text;
using Louis.Diagnostics;

namespace Louis.ArgumentValidation;

/// <summary>
/// Provides helper methods for constructing and throwing exceptions.
/// </summary>
partial class ArgHelper
{
    private const string DefaultArgumentExceptionFormat = "{@} is not a valid value.";

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
    /// <para>Each format item is converted to a string representation of <paramref name="value"/>, using the same algorithm
    /// as <see cref="ExceptionHelper.FormatObject"/>.</para>
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

            _ = sb.Append(format, pos, startPos - pos)
                  .AppendFormattedObject(value, format[valueFormatPos..endPos]);

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
}

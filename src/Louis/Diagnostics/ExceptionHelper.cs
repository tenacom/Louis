// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Louis.Text;

namespace Louis.Diagnostics;

/// <summary>
/// Provides utility methods to construct exception messages.
/// </summary>
public static class ExceptionHelper
{
    /// <summary>
    /// The text used by <see cref="FormatObject"/> to represent <see langword="null"/> values.
    /// </summary>
    public const string NullText = "<null>";

    /// <summary>
    /// The text used by <see cref="FormatObject"/> to represent values
    /// whose <see cref="object.ToString">ToString</see> method returns <see langword="null"/>.
    /// </summary>
    public const string ToStringNullText = "<null!>";

    /// <summary>
    /// The text used by <see cref="FormatObject"/> to represent values
    /// whose <see cref="object.ToString">ToString</see> method returns the empty string.
    /// </summary>
    public const string ToStringEmptyText = "<empty!>";

    /// <summary>
    /// The text used by <see cref="FormatObject"/> to represent instances of <see cref="IFormattable"/>
    /// that cannot be formatted.
    /// </summary>
    public const string InvalidFormatText = "<invalid_format>";

#pragma warning disable SA1629 // Documentation text should end with a period - The colon in the <remarks> section is as intended.
    /// <summary>
    /// Returns a text representation of an object.
    /// </summary>
    /// <param name="obj">The object to represent. Can be <see langword="null"/>.</param>
    /// <param name="format">An optional format string to apply if <paramref name="obj"/>
    /// implements <see cref="IFormattable"/>.</param>
    /// <returns>A text representation of <paramref name="obj"/>.</returns>
    /// <remarks>
    /// <para>This method attempts to never throw exceptions and always return meaningful text,
    /// without cluttering exception messages with potentially very long string values,
    /// by using the following algorithm:</para>
    /// <list type="bullet">
    /// <item><description>if <paramref name="obj"/> is <see langword="null"/>, return <see cref="NullText"/>;</description></item>
    /// <item><description>if <paramref name="obj"/> is a <see langword="string"/>, convert it to a
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#quoted-string-literals">quoted string literal</see>,
    /// clipping it if longer than 43 characters by leaving the first and last 20 characters
    /// separated by an ellipsis (three dots);</description></item>
    /// <item><description>if <paramref name="obj"/> implements <see cref="IFormattable"/>, try to format it
    /// using the specified <paramref name="format"/> and the <see cref="CultureInfo.InvariantCulture">invariant culture</see>;
    /// on <see cref="FormatException"/>, fall back to an empty format string,
    /// then fall further back by returning <see cref="InvalidFormatText"/>;</description></item>
    /// <item><description>if <paramref name="obj"/> is the <see langword="default"/> value of a
    /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types">nullable value type</see>,
    /// return <see cref="NullText"/>;</description></item>
    /// <item><description>otherwise, call <paramref name="obj"/>.<see cref="object.ToString">ToString()</see> and return the result.
    /// If <c>ToString</c> throws an exception, return the exception's type name between angle brackets.</description></item>
    /// </list>
    /// </remarks>
#pragma warning restore SA1629 // Documentation text should end with a period
    public static string FormatObject(object? obj, string? format = null)
        => obj switch {
            null => NullText,
            string str => str.ToClippedQuotedLiteral(20, 20, false),
            IFormattable formattable => FormatFormattable(formattable, format),
            _ => FormatNonFormattable(obj),
        };

    internal static string FormatFormattable(IFormattable formattable, string? format)
    {
        try
        {
            return formattable.ToString(format, CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            if (string.IsNullOrEmpty(format))
            {
                return InvalidFormatText;
            }

            try
            {
                return formattable.ToString(string.Empty, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                return InvalidFormatText;
            }
        }
    }

    internal static string FormatNonFormattable(object obj)
    {
        string? result;
        try
        {
            result = obj.ToString();
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            return $"<{e.GetType().Name}>";
        }

        return result == null ? (Nullable.GetUnderlyingType(obj.GetType()) != null ? NullText : ToStringNullText)
            : result.Length == 0 ? (Nullable.GetUnderlyingType(obj.GetType()) != null ? NullText : ToStringEmptyText)
            : result;
    }
}

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
    /// <item><description>otherwise, check whether <paramref name="obj"/> implements <see cref="IFormattable"/>
    /// and act accordingly.</description></item>
    /// </list>
    /// <para>If <paramref name="obj"/> implements <see cref="IFormattable"/>:</para>
    /// <list type="bullet">
    /// <item><description>try to format <paramref name="obj"/> using the specified <paramref name="format"/>
    /// and the <see cref="CultureInfo.InvariantCulture">invariant culture</see>;</description></item>
    /// <item><description>if an exception is thrown, try again using an empty format string;</description></item>
    /// <item><description>if another exception is thrown, treat <paramref name="obj"/> as if it didn't implement <see cref="IFormattable"/>.</description></item>
    /// </list>
    /// <para>If <paramref name="obj"/> does not implement <see cref="IFormattable"/>:</para>
    /// <list type="bullet">
    /// <item><description>call <paramref name="obj"/>.<see cref="object.ToString">ToString()</see>;</description></item>
    /// <item><description>if <c>ToString</c> throws an exception, return a string of the form <c>&lt;{objType}:{exceptionType}&gt;</c>;</description></item>
    /// <item><description>if <c>ToString</c> returns <see langword="null"/>, return <see cref="ToStringNullText"/>;</description></item>
    /// <item><description>if <c>ToString</c> returns the empty string, return <see cref="ToStringNullText"/>;</description></item>
    /// <item><description>otherwise, return the result of <c>ToString</c>.</description></item>
    /// </list>
    /// <para>Keep in mind that instances of <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types">nullable value types</see>
    /// will be passed <see href="https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types#boxing-and-unboxing">boxed</see>
    /// and therefore seen as either <see langword="null"/> or a non-nullable value.</para>
    /// </remarks>
#pragma warning restore SA1629 // Documentation text should end with a period
    public static string FormatObject(object? obj, string? format = null)
        => obj switch {
            null => NullText,
            string str => str.ToClippedQuotedLiteral(20, 20, false),
            IFormattable formattable => FormatFormattable(formattable, format),
            _ => FormatNonFormattable(obj),
        };

    private static string FormatFormattable(IFormattable formattable, string? format)
    {
        try
        {
            return formattable.ToString(format, CultureInfo.InvariantCulture);
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            if (string.IsNullOrEmpty(format))
            {
                return FormatNonFormattable(formattable);
            }

            try
            {
                return formattable.ToString(string.Empty, CultureInfo.InvariantCulture);
            }
            catch (Exception e2) when (!e2.IsCriticalError())
            {
                return FormatNonFormattable(formattable);
            }
        }
    }

    private static string FormatNonFormattable(object obj)
    {
        string? result;
        try
        {
            result = obj.ToString();
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            return $"<{obj.GetType().Name}:{e.GetType().Name}>";
        }

        return result == null ? ToStringNullText
            : result.Length == 0 ? ToStringEmptyText
            : result;
    }
}

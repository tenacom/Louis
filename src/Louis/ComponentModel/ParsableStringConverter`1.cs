// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET7_0_OR_GREATER

using System;
using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Diagnostics;

namespace Louis.ComponentModel;

/// <summary>
/// <para>A type converters that uses the <see cref="IParsable{TSelf}"/> interface to convert strings to instances of a type.</para>
/// <para>If the type also implements <see cref="IFormattable"/>, it is used to perform the opposite conversion.</para>
/// </summary>
/// <typeparam name="T">The type to convert.</typeparam>
public class ParsableStringConverter<T> : SimpleStringConverter<T>
    where T : IParsable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParsableStringConverter{T}"/> class.
    /// </summary>
    public ParsableStringConverter()
        : base(DoConvertFromString, DoConvertToString)
    {
    }

    private static T DoConvertFromString(ITypeDescriptorContext? context, CultureInfo? culture, string value)
        => T.TryParse(value, culture, out var result)
            ? result
            : ThrowHelper.ThrowArgumentException<T>(nameof(value), $"Value is not parsable as {typeof(T).FullName}.");

    private static string DoConvertToString(ITypeDescriptorContext? context, CultureInfo? culture, T value)
        => (value is IFormattable formattable ? formattable.ToString(null, culture) : value.ToString())
         ?? ThrowHelper.ThrowArgumentException<string>($"Instance of {typeof(T).FullName} is not convertible to string.");
}

#endif

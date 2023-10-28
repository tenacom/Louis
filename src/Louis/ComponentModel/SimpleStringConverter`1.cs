// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace Louis.ComponentModel;

/// <summary>
/// Base class for type converters that can convert instances of a specific type to and from strings.
/// </summary>
/// <typeparam name="T">The type to convert.</typeparam>
public abstract class SimpleStringConverter<T> : TypeConverter
{
    private readonly Func<ITypeDescriptorContext?, CultureInfo?, string, T>? _convertFromString;
    private readonly Func<ITypeDescriptorContext?, CultureInfo?, T, string>? _convertToString;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleStringConverter{T}"/> class.
    /// </summary>
    /// <param name="convertFromString">A delegate used to convert strings to instances of <typeparamref name="T"/>,
    /// or <see langword="null"/> if this provider does not provide this functionality.</param>
    /// <param name="convertToString">A delegate used to convert instances of <typeparamref name="T"/> to strings,
    /// or <see langword="null"/> if this provider does not provide this functionality.</param>
    protected SimpleStringConverter(
        Func<ITypeDescriptorContext?, CultureInfo?, string, T>? convertFromString,
        Func<ITypeDescriptorContext?, CultureInfo?, T, string>? convertToString)
    {
        _convertFromString = convertFromString;
        _convertToString = convertToString;
    }

    /// <summary>
    /// Returns whether this converter can convert an object of type <paramref name="sourceType"/>
    /// to <typeparamref name="T"/>, using the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="Type"/> that represents the type to convert from.</param>
    /// <returns><see langword="true"/> if this converter can perform the conversion; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>This method always returns <see langword="true"/> if <paramref name="sourceType"/> is <see cref="string"/>;
    /// otherwise, it calls <see cref="TypeConverter.CanConvertFrom(ITypeDescriptorContext,Type)"/> and returns the result.</para>
    /// </remarks>
    public sealed override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        => (_convertFromString is not null && sourceType == typeof(string)) || base.CanConvertFrom(context, sourceType);

    /// <summary>
    /// Converts the given object to <typeparamref name="T"/>, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
    /// <param name="value">The <see cref="object"/> to convert.</param>
    /// <returns>An <see cref="object"/> that represents the converted value.</returns>
    /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
    public sealed override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        => _convertFromString is not null && value is string str
            ? _convertFromString(context, culture, str)
            : base.ConvertFrom(context, culture, value);

    /// <summary>
    /// Returns whether this converter can convert an object an instance of <typeparamref name="T"/> to <paramref name="destinationType"/>,
    /// using the specified <paramref name="context"/>.
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="destinationType">A <see cref="Type"/> that represents the type to convert to.</param>
    /// <returns><see langword="true"/> if this converter can perform the conversion; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>This method always returns <see langword="true"/> if <paramref name="destinationType"/> is <see cref="string"/>;
    /// otherwise, it calls <see cref="TypeConverter.CanConvertTo(ITypeDescriptorContext,Type)"/> and returns the result.</para>
    /// </remarks>
    public sealed override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        => (_convertToString is not null && destinationType == typeof(string)) || base.CanConvertTo(context, destinationType);

    /// <summary>
    /// Converts the given object to <paramref name="destinationType"/>, using the specified context and culture information.
    /// </summary>
    /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
    /// <param name="value">The <see cref="object"/> to convert.</param>
    /// <param name="destinationType">A <see cref="Type"/> that represents the type to convert to.</param>
    /// <returns>An <see cref="object"/> that represents the converted value.</returns>
    /// <exception cref="NotSupportedException">The conversion cannot be performed.</exception>
    public sealed override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
        => _convertToString is not null && value is T typedValue && destinationType == typeof(string)
            ? ConvertToString(context, culture, typedValue)
            : base.ConvertTo(context, culture, value, destinationType);
}

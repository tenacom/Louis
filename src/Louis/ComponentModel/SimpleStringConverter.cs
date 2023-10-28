// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Louis.ComponentModel;

/// <summary>
/// Provides utility methods to simplify use of type converters derived from <see cref="SimpleStringConverter{T}"/>.
/// </summary>
public static class SimpleStringConverter
{
    /// <summary>
    /// Registers a type converter for use by <see cref="TypeDescriptor"/> methods.
    /// </summary>
    /// <typeparam name="T">The type of the object to convert to / from <see cref="string"/>.</typeparam>
    /// <typeparam name="TConverter">The type of the converter to use.</typeparam>
    public static void AddToTypeDescriptor<T, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TConverter>()
        where TConverter : SimpleStringConverter<T>
        => TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TConverter)));
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Net.Mail;
using CommunityToolkit.Diagnostics;

namespace Louis.ComponentModel;

/// <summary>
/// Provides a type converter to convert <see cref="MailAddressCollection"/> instances to and from strings.
/// </summary>
public sealed class MailAddressCollectionConverter : SimpleStringConverter<MailAddressCollection>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MailAddressCollectionConverter"/> class.
    /// </summary>
    public MailAddressCollectionConverter()
        : base(DoConvertFromString, DoConvertToString)
    {
    }

    private static MailAddressCollection DoConvertFromString(ITypeDescriptorContext? context, CultureInfo? culture, string value)
    {
        Guard.IsNotEmpty(value, nameof(value));
        var result = new MailAddressCollection();
        try
        {
            result.Add(value);
        }
        catch (FormatException e)
        {
            ThrowHelper.ThrowArgumentException(nameof(value), "Value should be a valid e-mail address, or valid e-mail addresses separated by commas.", e);
        }

        return result;
    }

    private static string DoConvertToString(ITypeDescriptorContext? context, CultureInfo? culture, MailAddressCollection value) => value.ToString();
}

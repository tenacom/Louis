// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Mail;
using CommunityToolkit.Diagnostics;

namespace Louis.ComponentModel;

/// <summary>
/// Provides a type converter to convert <see cref="MailAddress"/> instances to and from strings.
/// </summary>
public sealed class MailAddressConverter : SimpleStringConverter<MailAddress>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MailAddressConverter"/> class.
    /// </summary>
    public MailAddressConverter()
        : base(DoConvertFromString, DoConvertToString)
    {
    }

    private static MailAddress DoConvertFromString(ITypeDescriptorContext? context, CultureInfo? culture, string value)
    {
#if NET5_0_OR_GREATER
        return MailAddress.TryCreate(value, out var result) ? result : ThrowOnInvalidString(nameof(value));
#else
        try
        {
            return new MailAddress(value);
        }
        catch (FormatException ex)
        {
            return ThrowOnInvalidString(nameof(value), ex);
        }
#endif

        [DoesNotReturn]
        static MailAddress ThrowOnInvalidString(string parameterName, Exception? innerException = null)
            => ThrowHelper.ThrowArgumentException<MailAddress>(parameterName, "Value should be a valid e-mail address.", innerException);
    }

    private static string DoConvertToString(ITypeDescriptorContext? context, CultureInfo? culture, MailAddress value) => value.ToString();
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;

namespace Louis.Text.Internal;

internal static class InternalGuard
{
    public static void IsDefinedStringLiteralKind(StringLiteralKind value, [CallerArgumentExpression("value")] string name = "")
    {
        if (value is StringLiteralKind.Quoted or StringLiteralKind.Verbatim)
        {
            return;
        }

        ThrowHelper.ThrowArgumentException(name, $"{value} is not a valid {nameof(StringLiteralKind)}.");
    }
}

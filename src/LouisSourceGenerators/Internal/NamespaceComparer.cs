// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace LouisSourceGenerators.Internal;

internal class NamespaceComparer : IComparer<string>
{
    public static IComparer<string> Instance { get; } = new NamespaceComparer();

    public int Compare(string x, string y)
    {
        if (x.StartsWith("System", StringComparison.Ordinal))
        {
            return y.StartsWith("System", StringComparison.Ordinal)
                ? string.Compare(x, y, StringComparison.Ordinal)
                : 1;
        }

        return y.StartsWith("System", StringComparison.Ordinal)
            ? -1
            : string.Compare(x, y, StringComparison.Ordinal);
    }
}

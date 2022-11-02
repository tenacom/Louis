// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

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

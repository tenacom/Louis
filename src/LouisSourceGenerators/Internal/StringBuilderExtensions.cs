// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LouisSourceGenerators.Internal;

internal static partial class StringBuilderExtensions
{
    public static StringBuilder AppendIndentedLine(this StringBuilder @this, int indent, string line)
        => @this
          .Append(' ', indent)
          .AppendLine(line);

    public static StringBuilder AppendUsingDirectives(
        this StringBuilder @this,
        string codeNamespace,
        IEnumerable<string> namespaces,
        params string[] initialNamespaces)
    {
        var usings = new HashSet<string>();
        foreach (var @namespace in initialNamespaces)
        {
            _ = usings.Add(@namespace);
        }

        foreach (var @namespace in namespaces)
        {
            _ = usings.Add(@namespace);
        }

        foreach (var @namespace in usings.Where(s => !string.Equals(s, codeNamespace, StringComparison.Ordinal)).OrderBy(s => s, new NamespaceComparer()))
        {
            _ = @this.AppendLine($"using {@namespace};");
        }

        return @this;
    }
}

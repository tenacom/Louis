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
using System.Linq;
using System.Text;

namespace LouisSourceGenerators.Internal;

partial class StringBuilderExtensions
{
    public static StringBuilder AppendCodeHeader(
        this StringBuilder @this,
        string? codeNamespace,
        IEnumerable<string> namespaces,
        params string[] initialNamespaces)
    {
        var allNamespaces = new HashSet<string>();
        foreach (var @namespace in initialNamespaces)
        {
            _ = allNamespaces.Add(@namespace);
        }

        foreach (var @namespace in namespaces)
        {
            _ = allNamespaces.Add(@namespace);
        }

        var usingNamespaces = allNamespaces
                             .Where(s => !string.Equals(s, codeNamespace, StringComparison.Ordinal))
                             .OrderBy(s => s, NamespaceComparer.Instance)
                             .ToArray();

        _ = @this.AppendLine("""
                             // ---------------------------------------------------------------------------------------
                             // Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
                             // Licensed under the MIT license.
                             // See the LICENSE file in the project root for full license information.
                             //
                             // Part of this file may be third-party code, distributed under a compatible license.
                             // See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
                             // ---------------------------------------------------------------------------------------

                             #nullable enable

                             // Disable RS0016 because public API analyzers keep complaining
                             // even after adding APIs to PublicAPI.{Shipped|Unshipped}.txt manually.
                             #pragma warning disable RS0016
                             
                             """);

        if (usingNamespaces.Length > 0)
        {
            foreach (var usingNamespace in usingNamespaces)
            {
                _ = @this.AppendLine($"using {usingNamespace};");
            }

            _ = @this.AppendLine();
        }

        if (codeNamespace is not null)
        {
            _ = @this.AppendLine($"namespace {codeNamespace};")
                     .AppendLine();
        }

        return @this;
    }
}

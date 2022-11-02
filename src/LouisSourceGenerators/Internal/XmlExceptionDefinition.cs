// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace LouisSourceGenerators.Internal;

internal sealed class XmlExceptionDefinition
{
    public XmlExceptionDefinition(string? @namespace, string typeName, params string[] xmlHelpRows)
    {
        Namespace = @namespace;
        TypeName = typeName;
        XmlHelpRows = xmlHelpRows;
    }

    public string? Namespace { get; }

    public string TypeName { get; }

    public IReadOnlyList<string> XmlHelpRows { get; }
}

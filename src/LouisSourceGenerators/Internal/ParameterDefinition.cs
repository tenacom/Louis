// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

namespace LouisSourceGenerators.Internal;

internal sealed class ParameterDefinition
{
    public ParameterDefinition(string? @namespace, string type, string name, string xmlHelp)
    {
        Namespace = @namespace;
        Type = type;
        Name = name;
        XmlHelp = xmlHelp;
    }

    public string? Namespace { get; }

    public string Type { get; }

    public string Name { get; }

    public string XmlHelp { get; }
}

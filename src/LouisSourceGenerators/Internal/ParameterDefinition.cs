// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace LouisSourceGenerators.Internal;

internal sealed class ParameterDefinition
{
    public ParameterDefinition(string? @namespace, string type, string name, string xmlHelp)
        : this(@namespace, type, false, name, xmlHelp)
    {
    }

    public ParameterDefinition(string? @namespace, string type, bool isParams, string name, string xmlHelp)
    {
        Namespace = @namespace;
        Type = type;
        IsParams = isParams;
        Name = name;
        XmlHelp = xmlHelp;
    }

    public string? Namespace { get; }

    public string Type { get; }

    public bool IsParams { get; }

    public string Name { get; }

    public string XmlHelp { get; }
}

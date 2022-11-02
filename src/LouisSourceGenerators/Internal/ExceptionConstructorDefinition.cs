// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace LouisSourceGenerators.Internal;

internal sealed class ExceptionConstructorDefinition
{
    private readonly List<ParameterDefinition> _parameters;
    private readonly List<XmlExceptionDefinition> _exceptions;

    internal ExceptionConstructorDefinition()
    {
        _parameters = new();
        _exceptions = new();
    }

    public IReadOnlyList<ParameterDefinition> Parameters => _parameters;

    public IReadOnlyList<XmlExceptionDefinition> Exceptions => _exceptions;

    public ExceptionConstructorDefinition WithParameters(params ParameterDefinition[] parameters)
    {
        _parameters.AddRange(parameters);
        return this;
    }

    public ExceptionConstructorDefinition WithException(XmlExceptionDefinition exception)
    {
        _exceptions.Add(exception);
        return this;
    }

    public ExceptionConstructorDefinition WithException(string? @namespace, string typeName, params string[] xmlHelpRows)
    {
        _exceptions.Add(new(@namespace, typeName, xmlHelpRows));
        return this;
    }
}

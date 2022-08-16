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
using System.Runtime.CompilerServices;

namespace LouisSourceGenerators.Internal;

internal sealed class ExceptionDefinition
{
    private readonly List<IReadOnlyList<ParameterDefinition>> _parameterLists = new();

    public ExceptionDefinition(string @namespace, string name)
    {
        Namespace = @namespace;
        Name = name;
    }

    public string Namespace { get; }

    public string Name { get; }

    public string TypeName => Name + "Exception";

    public IReadOnlyList<IReadOnlyList<ParameterDefinition>> ParameterLists => _parameterLists;

    public ExceptionDefinition WithoutParameters()
    {
        _parameterLists.Add(Array.Empty<ParameterDefinition>());
        return this;
    }

    public ExceptionDefinition WithParameters(params ParameterDefinition[] parameters)
    {
        _parameterLists.Add(parameters);
        return this;
    }
}

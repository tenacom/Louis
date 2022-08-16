// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace LouisSourceGenerators.Internal;

internal sealed class ExceptionDefinitionList : List<ExceptionDefinition>
{
    public ExceptionDefinition Define(string @namespace, string name)
    {
        var definition = new ExceptionDefinition(@namespace, name);
        Add(definition);
        return definition;
    }

    public IEnumerable<string> GetAllReferencedNamespaces()
    {
        foreach (var definition in this)
        {
            yield return definition.Namespace;
            foreach (var parameterList in definition.ParameterLists)
            {
                foreach (var @namespace in parameterList.Select(p => p.Namespace).OfType<string>())
                {
                    yield return @namespace;
                }
            }
        }
    }
}

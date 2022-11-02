// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace LouisSourceGenerators.Internal;

internal sealed class ExceptionDefinitionList : List<ExceptionDefinition>
{
    public ExceptionDefinition Define(string name, params string[] namespaces)
    {
        var definition = new ExceptionDefinition(name, namespaces);
        Add(definition);
        return definition;
    }

    public IEnumerable<string> GetAllReferencedNamespaces()
    {
        foreach (var definition in this)
        {
            foreach (var @namespace in definition.Namespaces)
            {
                yield return @namespace;
            }

            foreach (var ctor in definition.Constructors)
            {
                foreach (var @namespace in ctor.Parameters.Select(p => p.Namespace).OfType<string>())
                {
                    yield return @namespace;
                }
            }
        }
    }
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace LouisSourceGenerators.Internal;

internal sealed class ExceptionDefinition
{
    private readonly List<ExceptionConstructorDefinition> _constructors = new();

    public ExceptionDefinition(string name, IEnumerable<string> namespaces)
    {
        Name = name;
        Namespaces = namespaces;
    }

    public IEnumerable<string> Namespaces { get; }

    public string Name { get; }

    public string TypeName => Name + "Exception";

    public IReadOnlyList<ExceptionConstructorDefinition> Constructors => _constructors;

    public ExceptionDefinition WithParameterlessConstructor()
    {
        _constructors.Add(new ExceptionConstructorDefinition());
        return this;
    }

    public ExceptionDefinition WithConstructor(Action<ExceptionConstructorDefinition> configure)
    {
        var ctor = new ExceptionConstructorDefinition();
        configure(ctor);
        _constructors.Add(ctor);
        return this;
    }
}

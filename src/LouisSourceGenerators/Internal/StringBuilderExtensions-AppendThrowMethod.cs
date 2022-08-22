// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LouisSourceGenerators.Internal;

partial class StringBuilderExtensions
{
    public static StringBuilder AppendThrowMethod(this StringBuilder @this, ExceptionDefinition exception, IReadOnlyCollection<ParameterDefinition> parameterList, bool generic)
    {
        var parameters = string.Join(", ", parameterList.Select(p => $"{p.Type} {p.Name}"));
        var arguments = string.Join(", ", parameterList.Select(p => p.Name));
        var withTheSpecifiedParameters = parameterList.Count switch {
            0 => string.Empty,
            1 => " with the specified parameter",
            _ => " with the specified parameters",
        };

        var theSpecifiedParameters = parameterList.Count switch {
            0 => "no parameters",
            1 => "the specified parameter",
            _ => "the specified parameters",
        };

        _ = @this.AppendLine($$"""
                                 /// <summary>
                                 /// Throws a new <see cref="{{exception.TypeName}}"/>{{withTheSpecifiedParameters}}.
                                 /// </summary>
                             """);

        if (generic)
        {
            _ = @this.AppendLine("    /// <typeparam name=\"T\">The expected return type.</typeparam>");
        }

        foreach (var parameter in parameterList)
        {
            _ = @this.AppendLine($$"""    /// <param name="{{parameter.Name}}">{{parameter.XmlHelp}}</param>""");
        }

        if (generic)
        {
            _ = @this.AppendLine("    /// <returns>This method does not return.</returns>");
        }

        return @this
            .AppendLine($$"""
                            /// <exception cref="{{exception.TypeName}}">Always thrown with {{theSpecifiedParameters}}.</exception>
                            [DoesNotReturn]
                            [DebuggerHidden]
                            [DebuggerStepThrough]
                            [StackTraceHidden]
                        """)
           .AppendLine(generic ? $"    public static T {exception.Name}<T>({parameters})" : $"    public static void {exception.Name}({parameters})")
           .AppendLine($"        => throw new {exception.TypeName}({arguments});");
    }
}

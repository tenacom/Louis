// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using System.Text;

namespace LouisSourceGenerators.Internal;

partial class StringBuilderExtensions
{
    public static StringBuilder AppendThrowMethod(this StringBuilder @this, ExceptionDefinition exception, ExceptionConstructorDefinition ctor, bool generic)
    {
        var parameters = string.Join(", ", ctor.Parameters.Select(p => $"{(p.IsParams ? "params " : null)}{p.Type} {p.Name}"));
        var arguments = string.Join(", ", ctor.Parameters.Select(p => p.Name));
        return @this.AppendThrowMethodXmlDocumentation(exception, ctor, generic)
                    .AppendLine("    [DoesNotReturn]")
                    .AppendLine("    [DebuggerHidden]")
                    .AppendLine("    [DebuggerStepThrough]")
                    .AppendLine("    [StackTraceHidden]")
                    .AppendLine(generic
                         ? $"    public static T {exception.Name}<T>({parameters})"
                         : $"    public static void {exception.Name}({parameters})")
                    .AppendLine($"        => throw new {exception.TypeName}({arguments});");
    }

    private static StringBuilder AppendThrowMethodXmlDocumentation(this StringBuilder @this, ExceptionDefinition exception, ExceptionConstructorDefinition ctor, bool generic)
        => @this.AppendThrowMethodXmlSummary(exception, ctor)
                .AppendThrowMethodXmlTypeParameters(generic)
                .AppendThrowMethodXmlParameters(ctor)
                .AppendThrowMethodXmlReturns(generic)
                .AppendThrowMethodXmlExceptions(exception, ctor);

    private static StringBuilder AppendThrowMethodXmlSummary(this StringBuilder @this, ExceptionDefinition exception, ExceptionConstructorDefinition ctor)
    {
        var withTheSpecifiedParameters = ctor.Parameters.Count switch {
            0 => string.Empty,
            1 => " with the specified parameter",
            _ => " with the specified parameters",
        };

        return @this.AppendLine("    /// <summary>")
                    .AppendLine($"    /// Throws a new <see cref=\"{exception.TypeName}\"/>{withTheSpecifiedParameters}.")
                    .AppendLine("    /// </summary>");
    }

    private static StringBuilder AppendThrowMethodXmlTypeParameters(this StringBuilder @this, bool generic)
        => generic
            ? @this.AppendLine("    /// <typeparam name=\"T\">The expected return type.</typeparam>")
            : @this;

    private static StringBuilder AppendThrowMethodXmlParameters(this StringBuilder @this, ExceptionConstructorDefinition ctor)
    {
        foreach (var parameter in ctor.Parameters)
        {
            _ = @this.AppendLine($"    /// <param name=\"{parameter.Name}\">{parameter.XmlHelp}</param>");
        }

        return @this;
    }

    private static StringBuilder AppendThrowMethodXmlReturns(this StringBuilder @this, bool generic)
        => generic
            ? @this.AppendLine("    /// <returns>This method does not return.</returns>")
            : @this;

    private static StringBuilder AppendThrowMethodXmlExceptions(this StringBuilder @this, ExceptionDefinition exception, ExceptionConstructorDefinition ctor)
    {
        var theSpecifiedParameters = ctor.Parameters.Count switch {
            0 => "no parameters",
            1 => "the specified parameter",
            _ => "the specified parameters",
        };

        var xmlExceptionHelp = ctor.Exceptions.Count > 0
            ? $"Thrown with {theSpecifiedParameters} if the exception constructor is successful"
            : $"Always thrown with {theSpecifiedParameters}";

        _ = @this.AppendLine($"    /// <exception cref=\"{exception.TypeName}\">{xmlExceptionHelp}.</exception>");
        foreach (var xmlException in ctor.Exceptions)
        {
            _ = @this.AppendLine($"    /// <exception cref=\"{xmlException.TypeName}\">");
            foreach (var row in xmlException.XmlHelpRows)
            {
                _ = @this.AppendLine("    /// " + row);
            }

            _ = @this.AppendLine("    /// </exception>");
        }

        return @this;
    }
}

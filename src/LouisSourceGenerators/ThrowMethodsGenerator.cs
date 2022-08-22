// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System.Text;
using LouisSourceGenerators.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace LouisSourceGenerators;

[Generator]
public class ThrowMethodsGenerator : IIncrementalGenerator
{
    private readonly ExceptionDefinitionList _exceptions = new();

    public void Initialize(IncrementalGeneratorInitializationContext context)
        => context.RegisterPostInitializationOutput(ctx =>
        {
            ParameterDefinition message = new(null, "string?", "message", "An error message that describes the reason for the exception.");
            ParameterDefinition innerException = new("System", "Exception?", "innerException", "The exception that is the cause of the exception being thrown.");
            ParameterDefinition paramName = new(null, "string?", "paramName", "The name of the parameter that caused the exception.");
            ParameterDefinition invalidValue = new(null, "int", "invalidValue", "The value of the argument that caused the exception.");
            ParameterDefinition enumType = new("System", "Type", "enumType", @"The type of the enumeration.");
            ParameterDefinition actualValue = new(null, "object?", "actualValue", "The value of the argument that caused the exception.");
            ParameterDefinition objectName = new(null, "string?", "objectName", "The name of the disposed object.");
            ParameterDefinition token = new("System.Threading", "CancellationToken", "token", "A cancellation token associated with the operation that was canceled.");

            _ = _exceptions.Define("System", "Argument")
                           .WithoutParameters()
                           .WithParameters(message)
                           .WithParameters(message, innerException)
                           .WithParameters(message, paramName)
                           .WithParameters(message, paramName, innerException);

            _ = _exceptions.Define("System", "ArgumentNull")
                           .WithoutParameters()
                           .WithParameters(paramName)
                           .WithParameters(message, innerException)
                           .WithParameters(paramName, message);

            _ = _exceptions.Define("System", "ArgumentOutOfRange")
                           .WithoutParameters()
                           .WithParameters(paramName)
                           .WithParameters(message, innerException)
                           .WithParameters(paramName, message)
                           .WithParameters(paramName, actualValue, message);

            _ = _exceptions.Define("System", "Format")
                           .WithoutParameters()
                           .WithParameters(message)
                           .WithParameters(message, innerException);

            _ = _exceptions.Define("System", "InvalidOperation")
                           .WithoutParameters()
                           .WithParameters(message)
                           .WithParameters(message, innerException);

            _ = _exceptions.Define("System", "NotSupported")
                           .WithoutParameters()
                           .WithParameters(message)
                           .WithParameters(message, innerException);

            _ = _exceptions.Define("System", "PlatformNotSupported")
                           .WithoutParameters()
                           .WithParameters(message)
                           .WithParameters(message, innerException);

            _ = _exceptions.Define("System", "ObjectDisposed")
                           .WithParameters(objectName)
                           .WithParameters(message, innerException)
                           .WithParameters(objectName, message);

            _ = _exceptions.Define("System", "OperationCanceled")
                           .WithoutParameters()
                           .WithParameters(message)
                           .WithParameters(token)
                           .WithParameters(message, innerException)
                           .WithParameters(message, token)
                           .WithParameters(message, innerException, token);

            _ = _exceptions.Define("System", "Timeout")
                           .WithoutParameters()
                           .WithParameters(message)
                           .WithParameters(message, innerException);

            _ = _exceptions.Define("System", "Overflow")
                           .WithoutParameters()
                           .WithParameters(message)
                           .WithParameters(message, innerException);

            var sourceBuilder = new StringBuilder()
                               .AppendCodeHeader(
                                    "Louis.Diagnostics",
                                    _exceptions.GetAllReferencedNamespaces(),
                                    "System.Diagnostics", // DebuggerHiddenAttribute, DebuggerStepThroughAttribute, StackTraceHiddenAttribute
                                    "System.Diagnostics.CodeAnalysis") // DoesNotReturnAttribute
                               .AppendLine("partial class Throw")
                               .AppendLine("{");

            var first = true;
            foreach (var exception in _exceptions)
            {
                foreach (var parameterList in exception.ParameterLists)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        _ = sourceBuilder.AppendLine();
                    }

                    _ = sourceBuilder.AppendThrowMethod(exception, parameterList, false)
                                     .AppendLine()
                                     .AppendThrowMethod(exception, parameterList, true);
                }
            }

            _ = sourceBuilder.AppendLine("}");
            ctx.AddSource("Throw-Methods", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        });
}

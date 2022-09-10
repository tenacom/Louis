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
            ParameterDefinition innerExceptionNotNull = new("System", "Exception", "innerException", "The exception that is the cause of the exception being thrown.");
            ParameterDefinition paramName = new(null, "string?", "paramName", "The name of the parameter that caused the exception.");
            ParameterDefinition invalidValue = new(null, "int", "invalidValue", "The value of the argument that caused the exception.");
            ParameterDefinition enumType = new("System", "Type", "enumType", "The type of the enumeration.");
            ParameterDefinition actualValue = new(null, "object?", "actualValue", "The value of the argument that caused the exception.");
            ParameterDefinition objectName = new(null, "string?", "objectName", "The name of the disposed object.");
            ParameterDefinition token = new("System.Threading", "CancellationToken", "token", "A cancellation token associated with the operation that was canceled.");
            ParameterDefinition paramsInnerExceptions = new("System", "Exception[]", true, "innerExceptions", "The exceptions that are the cause of the exception being thrown.");
            ParameterDefinition innerExceptions = new("System.Collections.Generic", "IEnumerable<Exception>", "innerExceptions", "The exceptions that are the cause of the exception being thrown.");

            _ = _exceptions.Define("Argument", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(message))
                           .WithConstructor(c => c.WithParameters(message, innerException))
                           .WithConstructor(c => c.WithParameters(message, paramName))
                           .WithConstructor(c => c.WithParameters(message, paramName, innerException));

            _ = _exceptions.Define("ArgumentNull", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(paramName))
                           .WithConstructor(c => c.WithParameters(message, innerException))
                           .WithConstructor(c => c.WithParameters(paramName, message));

            _ = _exceptions.Define("ArgumentOutOfRange", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(paramName))
                           .WithConstructor(c => c.WithParameters(message, innerException))
                           .WithConstructor(c => c.WithParameters(paramName, message))
                           .WithConstructor(c => c.WithParameters(paramName, actualValue, message));

            _ = _exceptions.Define("Format", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(message))
                           .WithConstructor(c => c.WithParameters(message, innerException));

            _ = _exceptions.Define("InvalidOperation", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(message))
                           .WithConstructor(c => c.WithParameters(message, innerException));

            _ = _exceptions.Define("NotSupported", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(message))
                           .WithConstructor(c => c.WithParameters(message, innerException));

            _ = _exceptions.Define("PlatformNotSupported", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(message))
                           .WithConstructor(c => c.WithParameters(message, innerException));

            _ = _exceptions.Define("ObjectDisposed", "System")
                           .WithConstructor(c => c.WithParameters(objectName))
                           .WithConstructor(c => c.WithParameters(message, innerException))
                           .WithConstructor(c => c.WithParameters(objectName, message));

            _ = _exceptions.Define("OperationCanceled", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(message))
                           .WithConstructor(c => c.WithParameters(token))
                           .WithConstructor(c => c.WithParameters(message, innerException))
                           .WithConstructor(c => c.WithParameters(message, token))
                           .WithConstructor(c => c.WithParameters(message, innerException, token));

            _ = _exceptions.Define("Timeout", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(message))
                           .WithConstructor(c => c.WithParameters(message, innerException));

            _ = _exceptions.Define("Overflow", "System")
                           .WithParameterlessConstructor()
                           .WithConstructor(c => c.WithParameters(message))
                           .WithConstructor(c => c.WithParameters(message, innerException));

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
                foreach (var ctor in exception.Constructors)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        _ = sourceBuilder.AppendLine();
                    }

                    _ = sourceBuilder.AppendThrowMethod(exception, ctor, false)
                                     .AppendLine()
                                     .AppendThrowMethod(exception, ctor, true);
                }
            }

            _ = sourceBuilder.AppendLine("}");
            ctx.AddSource("Throw-Methods", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        });
}

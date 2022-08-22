// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace Louis.Diagnostics;

partial class ExceptionExtensions
{
    /// <summary>
    /// Returns a value that tells whether an <see cref="Exception"/> is of a type that
    /// is better not caught, as it signals an unrecoverable condition.
    /// </summary>
    /// <param name="this">The exception being thrown.</param>
    /// <returns><see langword="true"/> if <paramref name="this"/> represents a critical error;
    /// otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>The following exception types are considered critical errors:</para>
    /// <list type="bullet">
    /// <item><description><see cref="NullReferenceException"/>;</description></item>
    /// <item><description><see cref="StackOverflowException"/>;</description></item>
    /// <item><description><see cref="AppDomainUnloadedException"/>;</description></item>
    /// <item><description><see cref="BadImageFormatException"/>;</description></item>
    /// <item><description><see cref="InvalidProgramException"/>;</description></item>
    /// <item><description><see cref="ThreadAbortException"/>;</description></item>
    /// <item><description><see cref="AccessViolationException"/>;</description></item>
    /// <item><description><see cref="OutOfMemoryException"/>;</description></item>
    /// <item><description><see cref="SEHException"/>;</description></item>
    /// <item><description><see cref="SecurityException"/>;</description></item>
    /// <item><description>any exception type implementing the <see cref="ICriticalError"/> interface.</description></item>
    /// </list>
    /// <para>Of course, not all exceptions of the above types must necessarily cause an application
    /// to terminate: for example, attempting to dynamically load an assembly as a plug-in
    /// will cause a <see cref="BadImageFormatException"/> to be thrown if the assembly file is corrupted.
    /// However, these situations should be handled explicitly, by catching specific exception types in
    /// restricted contexts (to continue the above example, a <c>catch (BadImageFormatException)</c> block
    /// should be present in the method that attempts to load the plug-in) and appropriate action
    /// (such as alerting the user, logging, etc.) should be taken instead of letting the exception
    /// "fall through" to external <c>catch</c> blocks.</para>
    /// <para>This method is mainly meant for "generic" catch blocks, when the only valuable information
    /// is that "something has gone wrong". In these cases, a simple <c>catch</c> or <c>catch (Exception ex)</c>
    /// block will trigger
    /// <see href="https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1031">warning CA1031</see>
    /// unless the exception is rethrown, because a potential serious problem is being hidden from the user;
    /// a conditional <c>catch (Exception ex) when (!ex.IsCriticalError())</c>, however, will not only
    /// prevent the warning, but correctly avoid hiding unrecoverable error conditions.</para>
    /// </remarks>
    public static bool IsCriticalError(this Exception @this)
        => AnyCausingExceptionCore(@this, IsCriticalErrorCore);

    private static bool IsCriticalErrorCore(Exception exception) => exception
        is NullReferenceException
        or StackOverflowException
        or AppDomainUnloadedException
        or BadImageFormatException
        or InvalidProgramException
        or ThreadAbortException
        or AccessViolationException
        or OutOfMemoryException
        or SEHException
        or SecurityException
        or ICriticalError;
}

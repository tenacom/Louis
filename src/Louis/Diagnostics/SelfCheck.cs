// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Louis.Diagnostics;

/// <summary>
/// Provides methods to perform self-checks in library or application code.
/// </summary>
public static class SelfCheck
{
    /// <summary>
    /// Creates and returns an exception telling that an internal self-check has failed.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="filePath">The path of the source file where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <param name="lineNumber">The line number in source where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <returns>
    /// A newly-created instance of <see cref="InternalErrorException"/>.
    /// </returns>
    /// <remarks>
    /// <para>The returned exception will be of type <see cref="InternalErrorException"/>; its
    /// <see cref="Exception.Message">Message</see> property will contain the specified
    /// <paramref name="message"/>, preceded by an indication of the assembly, source file,
    /// and line number of the failed check.</para>
    /// </remarks>
    /// <seealso cref="CriticalFailure"/>
    [MethodImpl(MethodImplOptions.NoInlining)] // Ensure we get the correct stack frame in BuildMessage (see below)
    public static InternalErrorException Failure(
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => new(BuildMessage(message, filePath, lineNumber));

    /// <summary>
    /// Creates and returns a critical exception telling that an internal self-check has failed.
    /// The returned exception should cause the application to terminate immediately.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="filePath">The path of the source file where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <param name="lineNumber">The line number in source where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <returns>
    /// A newly-created instance of <see cref="CriticalInternalErrorException"/>.
    /// </returns>
    /// <remarks>
    /// <para>The returned exception will be of type <see cref="CriticalInternalErrorException"/>; its
    /// <see cref="Exception.Message">Message</see> property will contain the specified
    /// <paramref name="message"/>, preceded by an indication of the assembly, source file,
    /// and line number of the failed check.</para>
    /// <para>The returned exception is of a type that will be considered critical by the <see cref="ExceptionExtensions.IsCriticalError"/>
    /// method; therefore, assuming critical exception checks are implemented correctly, it will not
    /// be caught and will cause the application to terminate immediately.</para>
    /// </remarks>
    /// <seealso cref="Failure"/>
    [MethodImpl(MethodImplOptions.NoInlining)] // Ensure we get the correct stack frame in BuildMessage (see below)
    public static CriticalInternalErrorException CriticalFailure(
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => new(BuildMessage(message, filePath, lineNumber));

    /// <summary>
    /// Throws an exception telling that an internal self-check has failed.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="filePath">The path of the source file where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <param name="lineNumber">The line number in source where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <remarks>
    /// <para>The thrown exception will be of type <see cref="InternalErrorException"/>; its
    /// <see cref="Exception.Message">Message</see> property will contain the specified
    /// <paramref name="message"/>, preceded by an indication of the assembly, source file,
    /// and line number of the failed check.</para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.NoInlining)] // Ensure we get the correct stack frame in BuildMessage (see below)
    [DoesNotReturn]
    public static void Fail(
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => throw new InternalErrorException(BuildMessage(message, filePath, lineNumber));

    /// <summary>
    /// Throws a critical exception telling that an internal self-check has failed.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="filePath">The path of the source file where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <param name="lineNumber">The line number in source where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <remarks>
    /// <para>The thrown exception will be of type <see cref="CriticalInternalErrorException"/>; its
    /// <see cref="Exception.Message">Message</see> property will contain the specified
    /// <paramref name="message"/>, preceded by an indication of the assembly, source file,
    /// and line number of the failed check.</para>
    /// <para>The thrown exception is of a type that will be considered critical by the <see cref="ExceptionExtensions.IsCriticalError"/>
    /// method; therefore, assuming critical exception checks are implemented correctly, it will not
    /// be caught and will cause the application to terminate immediately.</para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.NoInlining)] // Ensure we get the correct stack frame in BuildMessage (see below)
    [DoesNotReturn]
    public static void FailCritically(
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => throw new CriticalInternalErrorException(BuildMessage(message, filePath, lineNumber));

    /// <summary>
    /// Throws an exception telling that an internal self-check has failed.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="message">The exception message.</param>
    /// <param name="filePath">The path of the source file where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <param name="lineNumber">The line number in source where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <returns>This method never returns.</returns>
    /// <remarks>
    /// <para>The thrown exception will be of type <see cref="InternalErrorException"/>; its
    /// <see cref="Exception.Message">Message</see> property will contain the specified
    /// <paramref name="message"/>, preceded by an indication of the assembly, source file,
    /// and line number of the failed check.</para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.NoInlining)] // Ensure we get the correct stack frame in BuildMessage (see below)
    [DoesNotReturn]
    public static T Fail<T>(
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => throw new InternalErrorException(BuildMessage(message, filePath, lineNumber));

    /// <summary>
    /// Throws a critical exception telling that an internal self-check has failed.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="message">The exception message.</param>
    /// <param name="filePath">The path of the source file where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <param name="lineNumber">The line number in source where this method is called.
    /// This parameter is automatically added by the compiler amd should never be provided explicitly.</param>
    /// <returns>This method never returns.</returns>
    /// <remarks>
    /// <para>The thrown exception will be of type <see cref="CriticalInternalErrorException"/>; its
    /// <see cref="Exception.Message">Message</see> property will contain the specified
    /// <paramref name="message"/>, preceded by an indication of the assembly, source file,
    /// and line number of the failed check.</para>
    /// <para>The thrown exception is of a type that will be considered critical by the <see cref="ExceptionExtensions.IsCriticalError"/>
    /// method; therefore, assuming critical exception checks are implemented correctly, it will not
    /// be caught and will cause the application to terminate immediately.</para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.NoInlining)] // Ensure we get the correct stack frame in BuildMessage (see below)
    [DoesNotReturn]
    public static T FailCritically<T>(
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        => throw new InternalErrorException(BuildMessage(message, filePath, lineNumber));

    // This method MUST be called directly from a public method of this class
    // in order to get the correct stack frame of the caller.
    private static string BuildMessage(string message, string filePath, int lineNumber)
    {
        // No point in throwing an exception if message is null.
        message ??= "<no message>";

        // Skip this method and the calling method when getting the stack frame.
        var assemblyName = new StackFrame(2).GetType().Assembly.GetName().Name ?? "<unknown>";

        // Ignore the line number if there is no source file to refer to.
        var haveFilePath = !string.IsNullOrEmpty(filePath);
        if (!haveFilePath)
        {
            lineNumber = 0;
        }

        // Precompute StringBuilder capacity to avoid multiple allocations:
        //   - message length, plus
        //   - assembly name length, plus
        //   - 3 chars for "[" and "] ", plus
        //   - if filePath is specified, file path length + 2 chars for ": " + 12 chars for "(<line number>)"
        //     (assuming 10 digits for lineNumber, as that's the maximum number of digits for a positive 32-bit integer)
        var capacity = message.Length + assemblyName.Length + 3 + (haveFilePath ? filePath.Length + 14 : 0);
        var sb = new StringBuilder(capacity)
                .Append('[')
                .Append(assemblyName);

        if (haveFilePath)
        {
            _ = sb.Append(": ").Append(filePath);
            if (lineNumber > 0)
            {
                _ = sb.Append('(').Append(lineNumber).Append(')');
            }
        }

        return sb.Append("] ").Append(message).ToString();
    }
}

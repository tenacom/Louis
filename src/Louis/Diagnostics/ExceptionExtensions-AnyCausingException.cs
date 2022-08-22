// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using Louis.ArgumentValidation;

namespace Louis.Diagnostics;

partial class ExceptionExtensions
{
    /// <summary>
    /// Determines whether any of the causing exceptions for this exception
    /// satisfies the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="this">The <see cref="Exception"/> on which this method is called.</param>
    /// <param name="predicate">A function to test each causing exception for a condition.</param>
    /// <returns><see langword="true"/> if <paramref name="predicate"/> returns <c>true</c> for at least one
    /// of the causing exceptions of <paramref name="this"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// <para>For an explanation of what are considered causing exceptions of an exception, see the "Remarks"
    /// section of <see cref="GetCausingExceptions"/>.</para>
    /// <para>This method yields the same result as calling <c>exception.GetCausingExceptions().Any(predicate)</c>,
    /// but is faster and allocates less memory. It is used internally by the <see cref="IsCriticalError"/> method.</para>
    /// </remarks>
    public static bool AnyCausingException(this Exception @this, Func<Exception, bool> predicate)
        => AnyCausingExceptionCore(@this, Arg.NotNull(predicate));

    private static bool AnyCausingExceptionCore(Exception exception, Func<Exception, bool> predicate)
    {
        if (exception is AggregateException aggregateException)
        {
            foreach (var innerException in aggregateException.InnerExceptions)
            {
                if (AnyCausingExceptionCore(innerException, predicate))
                {
                    return true;
                }
            }
        }
        else
        {
            if (exception.InnerException is { } innerException)
            {
                if (AnyCausingExceptionCore(innerException, predicate))
                {
                    return true;
                }
            }

            if (predicate(exception))
            {
                return true;
            }
        }

        return false;
    }
}

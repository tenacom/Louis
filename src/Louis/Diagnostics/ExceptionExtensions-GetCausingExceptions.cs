// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Louis.Diagnostics;

partial class ExceptionExtensions
{
    /// <summary>
    /// Enumerates all causing exceptions of the current exception.
    /// </summary>
    /// <param name="this">The <see cref="Exception"/> on which this method is called.</param>
    /// <returns>An enumeration of the causing exceptions of <paramref name="this"/>.</returns>
    /// <remarks>
    /// <para>The "causing exceptions" of an exceptions are the exceptions that caused it to be thrown,
    /// including itself (but excluding <see cref="AggregateException"/>s, as they are only meant
    /// as containers for other exceptions).</para>
    /// <para>Enumerating the causing exceptions of an exception is useful, for example, when we want
    /// to reconstruct the chain of events that lead to the current exception being thrown.
    /// <see cref="GetCausingExceptions"/> enumerates innermost exceptions first, in order to make
    /// this chain of events clear.</para>
    /// <para>Another common use case for causing exceptions is determining whether any of
    /// the causing exception of a caught exception is of a certain type, or anyway satisfies a
    /// given condition. While you can use LINQ's <see cref="Enumerable.Any{TSource}(IEnumerable{TSource},Func{TSource,bool})"/> method
    /// on the return value of <see cref="GetCausingExceptions"/>, L.o.U.I.S. also provides
    /// the <see cref="AnyCausingException"/> extension method, which is optimized specifically
    /// for this case.</para>
    /// </remarks>
    public static IEnumerable<Exception> GetCausingExceptions(this Exception @this) => new CausingExceptions(@this);

    private static IEnumerator<Exception> GetCausingExceptionsCore(Exception exception)
    {
        if (exception is AggregateException aggregateException)
        {
            // A foreach would very probably be fine here, generating the very same code,
            // but I want to stress that we're dealing with enumerators directly.
            using var enumerator = aggregateException.InnerExceptions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                // Early bailout for common cases - don't allocate an extra enumerator unless necessary.
                if (current is not AggregateException && current.InnerException is null)
                {
                    yield return current;
                }
                else
                {
                    using var innerEnumerator = GetCausingExceptionsCore(current);
                    while (innerEnumerator.MoveNext())
                    {
                        yield return innerEnumerator.Current;
                    }
                }
            }
        }
        else
        {
            if (exception.InnerException is { } innerException)
            {
                // Early bailout for common cases - don't allocate an extra enumerator unless necessary.
                if (innerException is not AggregateException && innerException.InnerException is null)
                {
                    yield return innerException;
                }
                else
                {
                    using var enumerator = GetCausingExceptionsCore(innerException);
                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;
                    }
                }
            }

            yield return exception;
        }
    }
}

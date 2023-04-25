// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Diagnostics;

namespace Louis.Collections;

partial class EnumerableExtensions
{
    /// <summary>
    /// Filters a sequence of nullable values, taking only those that are not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="this"/>.</typeparam>
    /// <param name="this">An <see cref="IEnumerable{T}"/> to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that are not <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> @this)
        where T : class
    {
        Guard.IsNotNull(@this);

        return @this.Where(x => x is not null) as IEnumerable<T>;
    }

    /// <summary>
    /// Filters a sequence of nullable values, taking only those that are not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="this"/>.</typeparam>
    /// <param name="this">An <see cref="IEnumerable{T}"/> to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the values of the elements from the input sequence that are not <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> @this)
        where T : struct
    {
        Guard.IsNotNull(@this);

        return @this.Where(x => x.HasValue).Select(x => x!.Value);
    }
}

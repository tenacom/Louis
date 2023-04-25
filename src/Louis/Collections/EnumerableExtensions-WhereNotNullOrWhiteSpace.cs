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
    /// Filters a sequence of nullable strings, taking only those that are not <see langword="null"/>,
    /// nor empty, nor consisting only of white-space characters.
    /// </summary>
    /// <param name="this">An <see cref="IEnumerable{T}">IEnumerable&lt;string?&gt;</see> to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}">IEnumerable&lt;string&gt;</see> that contains elements from the input sequence
    /// that are not <see langword="null"/>, nor empty, nor consisting only of white-space characters.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="this"/> is <see langword="null"/>.</exception>
    public static IEnumerable<string> WhereNotNullOrWhiteSpace(this IEnumerable<string?> @this)
    {
        Guard.IsNotNull(@this);
        return @this.Where(IsNotNullOrWhiteSpace) as IEnumerable<string>;

        static bool IsNotNullOrWhiteSpace(string? x) => !string.IsNullOrWhiteSpace(x);
    }
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;

namespace Louis.Threading;

/// <summary>
/// Provides utility methods to deal with <see cref="ValueTask"/> objects.
/// </summary>
public static class ValueTaskUtility
{
    /// <summary>
    /// Creates a task that will complete when all of the <see cref="ValueTask"/> objects in an enumerable collection have completed.
    /// </summary>
    /// <param name="valueTasks">The <see cref="ValueTask"/>s to wait on for completion.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the completion of all of the supplied <see cref="ValueTask"/>s.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="valueTasks"/> is <see langword="null"/>.</exception>
    public static async ValueTask WhenAll(IEnumerable<ValueTask> valueTasks)
    {
        Guard.IsNotNull(valueTasks);

        var pendingTasks = valueTasks.Where(vt => !vt.IsCompletedSuccessfully).Select(vt => vt.AsTask()).ToList();
        if (pendingTasks.Count > 0)
        {
            await Task.WhenAll(pendingTasks).ConfigureAwait(false);
        }
    }
}

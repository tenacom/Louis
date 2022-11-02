// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Louis;

/// <summary>
/// A delegate taking no parameters and returning a <see cref="ReadOnlySpan{T}"/>.
/// </summary>
/// <typeparam name="T">The type of items in the returned span.</typeparam>
/// <returns>A read-only span of <typeparamref name="T"/>.</returns>
public delegate ReadOnlySpan<T> ReadOnlySpanFunc<T>();

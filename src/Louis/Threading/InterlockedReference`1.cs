// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Louis.Threading;

/// <summary>
/// Implements thread-safe management of an object reference.
/// </summary>
/// <typeparam name="T">The type of the stored reference.</typeparam>
public struct InterlockedReference<T> : IEquatable<InterlockedReference<T>>, IEquatable<T?>
    where T : class
{
    private T? _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="InterlockedReference{T}"/> struct.
    /// </summary>
    /// <param name="value">The initial value of the reference.</param>
    public InterlockedReference(T? value)
    {
        _value = value;
    }

    /// <summary>
    /// <para>Gets or sets the reference stored in an <see cref="InterlockedReference{T}"/>.</para>
    /// <para>This property if thread-safe.</para>
    /// </summary>
    public T? Value
    {
        get => Interlocked.CompareExchange(ref _value, null, null);
        set => _ = Interlocked.Exchange(ref _value, value);
    }

    /// <summary>
    /// Gets a value indicating whether the reference stored in an <see cref="InterlockedReference{T}"/> is <see langword="null"/>.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the reference is <see langword="null"/>;
    /// <see langword="false"/> otherwise.
    /// </value>
    public bool IsNull => Interlocked.CompareExchange(ref _value, null, null) == null;

    /// <summary>
    /// Determines whether two specified interlocked references have the same value.
    /// </summary>
    /// <param name="a">The first instance to compare.</param>
    /// <param name="b">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(InterlockedReference<T> a, InterlockedReference<T> b) => ReferenceEquals(a.Value, b.Value);

    /// <summary>
    /// Determines whether two specified interlocked references have different values.
    /// </summary>
    /// <param name="a">The first instance to compare.</param>
    /// <param name="b">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="a"/> is different from the value of <paramref name="b"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(InterlockedReference<T> a, InterlockedReference<T> b) => !ReferenceEquals(a.Value, b.Value);

    /// <summary>
    /// Determines whether the value of a specified interlocked references is the same as a specified value.
    /// </summary>
    /// <param name="a">The interlocked reference to compare.</param>
    /// <param name="b">The reference to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(InterlockedReference<T> a, T? b) => ReferenceEquals(a.Value, b);

    /// <summary>
    /// Determines whether the value of a specified interlocked reference is different from a specified value.
    /// </summary>
    /// <param name="a">The interlocked reference to compare.</param>
    /// <param name="b">The reference to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="a"/> is different from the value of <paramref name="b"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(InterlockedReference<T> a, T? b) => !ReferenceEquals(a.Value, b);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj switch {
        null => Interlocked.CompareExchange(ref _value, null, null) == null,
        T value => Interlocked.CompareExchange(ref _value, null, null) == value,
        InterlockedReference<T> other => Interlocked.CompareExchange(ref _value, null, null) == other.Value,
        _ => false,
    };

    /// <inheritdoc/>
    public bool Equals(InterlockedReference<T> other) => ReferenceEquals(other.Value, Value);

    /// <inheritdoc/>
    public bool Equals(T? other) => Interlocked.CompareExchange(ref _value, null, null) == other;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Value);

    /// <inheritdoc/>
    public override string ToString() => Value?.ToString() ?? string.Empty;

    /// <summary>
    /// Sets the value of an interlocked reference.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>The previous value of this instance.</returns>
    public T? Exchange(T? value) => Interlocked.Exchange(ref _value, value);

    /// <summary>
    /// Compares the value of an interlocked reference to a given comparand and, if they are equal, replaces it with a given value.
    /// </summary>
    /// <param name="value">The value that replaces the destination value if the comparison by reference results in equality.</param>
    /// <param name="comparand">The value that is compared by reference to the value of this instance.</param>
    /// <returns>The original value of this instance.</returns>
    public T? CompareExchange(T? value, T? comparand) => Interlocked.CompareExchange(ref _value, value, comparand);
}

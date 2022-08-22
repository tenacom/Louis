// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Louis.Threading;

/// <summary>
/// Implements thread-safe management of a boolean value.
/// </summary>
public struct InterlockedFlag : IEquatable<InterlockedFlag>
{
    private int _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="InterlockedFlag"/> struct.
    /// </summary>
    /// <param name="value">The initial value of the flag.</param>
    public InterlockedFlag(bool value)
    {
        _value = value ? 1 : 0;
    }

    /// <summary>
    /// <para>Gets or sets a value indicating whether the flag is set.</para>
    /// <para>This property if thread-safe.</para>
    /// </summary>
    /// <value>
    /// The value of the flag.
    /// </value>
    public bool Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Volatile.Read(ref _value) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = Interlocked.Exchange(ref _value, value ? 1 : 0);
    }

    /// <summary>
    /// Determines whether two specified interlocked flags have the same value.
    /// </summary>
    /// <param name="a">The first flag to compare.</param>
    /// <param name="b">The second flag to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(InterlockedFlag a, InterlockedFlag b) => a.Value == b.Value;

    /// <summary>
    /// Determines whether two specified interlocked flags have different values.
    /// </summary>
    /// <param name="a">The first flag to compare.</param>
    /// <param name="b">The second flag to compare.</param>
    /// <returns><see langword="true"/> if the value of <paramref name="a"/> is different from the value of <paramref name="b"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(InterlockedFlag a, InterlockedFlag b) => a.Value != b.Value;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is InterlockedFlag other && Equals(other);

    /// <inheritdoc/>
    public bool Equals(InterlockedFlag other) => other.Value == Value;

    /// <inheritdoc/>
    public override int GetHashCode() => Value.GetHashCode();

    /// <inheritdoc/>
    public override string ToString() => Value.ToString();

    /// <summary>
    /// <para>Sets the value of an <see cref="InterlockedFlag"/> and returns the previous value.</para>
    /// <para>This method is thread-safe.</para>
    /// </summary>
    /// <param name="value">The value to assign to the flag.</param>
    /// <returns>The previous value of the flag.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CheckAndSet(bool value) => Interlocked.Exchange(ref _value, value ? 1 : 0) != 0;

    /// <summary>
    /// <para>Tries to set the value of an <see cref="InterlockedFlag"/> to the specified value.</para>
    /// <para>This method is thread-safe.</para>
    /// </summary>
    /// <param name="value">The desired value to set.</param>
    /// <returns>
    /// <see langword="true"/> if the flag has been set to <paramref name="value"/>;
    /// <see langword="false"/> if the value of the flag was already equal to <paramref name="value"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySet(bool value)
    {
        var numericValue = value ? 1 : 0;
        return Interlocked.CompareExchange(ref _value, numericValue, 1 - numericValue) != numericValue;
    }

    /// <summary>
    /// <para>Tries to set the value of an <see cref="InterlockedFlag"/> to <see langword="true"/>.</para>
    /// <para>This method is thread-safe.</para>
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the flag has been set;
    /// <see langword="false"/> if the flag was already set.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TrySet() => Interlocked.CompareExchange(ref _value, 1, 0) == 0;

    /// <summary>
    /// <para>Tries to reset the value of an <see cref="InterlockedFlag"/> to <see langword="false"/>.</para>
    /// <para>This method is thread-safe.</para>
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the flag has been reset;
    /// <see langword="false"/> if the value of the flag was already <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryReset() => Interlocked.CompareExchange(ref _value, 0, 1) != 0;
}

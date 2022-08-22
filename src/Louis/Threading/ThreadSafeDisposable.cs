// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;

namespace Louis.Threading;

/// <summary>
/// <para>Provides a simple, thread-safe implementation of <see cref="IDisposable"/>
/// for classes that do not own unmanaged resources.</para>
/// <para>This class does not implement <see href="https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose#implement-the-dispose-pattern">
/// Microsoft's recommended dispose pattern</see>: there is no <c>Dispose(bool)</c> method to override.</para>
/// <para>The reasons why this class' <see cref="IDisposable"/> implementation does not follow
/// Microsoft's guidelines is explained in great detail in <see href="">this article by Stephen Cleary</see>.
/// </para>
/// </summary>
public abstract class ThreadSafeDisposable : IDisposable
{
    private InterlockedFlag _disposed;

    /// <summary>
    /// Gets a value indicating whether this instance has been disposed.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if this instance has been disposed;
    /// <see langword="false"/> otherwise.
    /// </value>
    /// <remarks>
    /// <para>This property's value becomes <see langword="true"/> as soon as
    /// <see cref="Dispose"/> is called for the first time, even before actual disposal
    /// (implemented in an overridden <see cref="DisposeCore"/> method) begins.
    /// However, when this property is <see langword="true"/> the caller must treat the object
    /// as already disposed, i.e. not use it.</para>
    /// </remarks>
    public bool Disposed => _disposed.Value;

    /// <summary>
    /// <para>Frees, releases, or resets unmanaged resources owned by this instance.</para>
    /// <para>This method implements <see cref="IDisposable.Dispose"/>, although not as
    /// recommended by Microsoft. See the Remarks section for more details.</para>
    /// <para>Classes that directly own unmanaged resources must not derive from this class.</para>
    /// </summary>
    /// <remarks>
    /// <para>This method is not virtual. Derived classes must override the <see cref="DisposeCore"/> method
    /// to actually free, release, or reset their resources.</para>
    /// <para>This method uses <see cref="System.Threading.Interlocked">Interlocked</see> semantics
    /// to ensure that <see cref="DisposeCore"/> is called a most once, no matter how many times
    /// this method is called.</para>
    /// </remarks>
    public void Dispose()
    {
        if (!_disposed.TrySet())
        {
            return;
        }

        DisposeCore();
    }

    /// <summary>
    /// <para>Frees, releases, or resets unmanaged resources owned by this instance.</para>
    /// <para>This method is called by <see cref="Dispose"/> to perform the actual operations
    /// needed for disposing the object.</para>
    /// </summary>
    /// <remarks>
    /// <para><see cref="Dispose"/> uses <see cref="System.Threading.Interlocked">Interlocked</see> semantics
    /// to ensure that this method is called a most once, no matter how many times
    /// <see cref="Dispose"/> is called.</para>
    /// </remarks>
    protected abstract void DisposeCore();

    /// <summary>
    /// Ensures that this instance is not disposed, throwing <see cref="ObjectDisposedException"/>
    /// if it has already been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if this instance has already been disposed.</exception>
    protected void EnsureNotDisposed()
    {
        if (_disposed.Value)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}

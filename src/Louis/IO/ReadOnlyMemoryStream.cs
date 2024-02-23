// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using Louis.Diagnostics;
using PolyKit.Diagnostics.CodeAnalysis;

namespace Louis.IO;

/// <summary>
/// Implements a read-only <see cref="Stream"/> using a <see cref="ReadOnlyMemory{T}">ReadOnlyMemory&lt;byte&gt;</see> as backing store.
/// </summary>
public sealed class ReadOnlyMemoryStream : Stream
{
    private readonly ReadOnlyMemory<byte> _data;
    private bool _isOpen;
    private int _position;
    private Task<int>? _lastCompletedInt32Task;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyMemoryStream"/> class.
    /// </summary>
    /// <param name="data">The memory from which to create the current stream.</param>
    public ReadOnlyMemoryStream(ReadOnlyMemory<byte> data)
    {
        _data = data;
        _isOpen = true;
        _position = 0;
    }

    /// <summary>
    /// Gets a value indicating whether the current stream supports reading.
    /// </summary>
    /// <value><see langword="true"/> if the stream is open.</value>
    public override bool CanRead => _isOpen;

    /// <summary>
    /// Gets a value indicating whether the current stream supports seeking.
    /// </summary>
    /// <value><see langword="true"/> if the stream is open.</value>
    public override bool CanSeek => _isOpen;

    /// <summary>
    /// Gets a value indicating whether the current stream supports writing.
    /// </summary>
    /// <value>Always <see langword="false"/>.</value>
    public override bool CanWrite => false;

    /// <summary>
    /// Gets the length of the stream in bytes.
    /// </summary>
    /// <value>The length of the stream in bytes.</value>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override long Length
    {
        get
        {
            EnsureNotClosed();
            return _data.Length;
        }
    }

    /// <summary>
    /// Gets or sets the current position within the stream.
    /// </summary>
    /// <value>The current position within the stream.</value>
    /// <exception cref="ArgumentOutOfRangeException">The position is set to a negative value or a value greater than <see cref="int.MaxValue"/>.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override long Position
    {
        get
        {
            EnsureNotClosed();
            return _position;
        }
        set
        {
            Guard.IsGreaterThan(value, 0);
            EnsureNotClosed();
            if (value > int.MaxValue)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException("Stream position must be non-negative and less than 2^31.");
            }

            _position = (int)value;
        }
    }

    /// <summary>
    /// Overrides the <see cref="Stream.Flush"/> method, performing no action.
    /// </summary>
    /// <remarks>
    /// <para>Since a <see cref="ReadOnlyMemoryStream"/> cannot be written to, this method is redundant.
    /// However, flushing a read-only stream is a valid operation; therefore this method simply returns
    /// without doing anything.</para>
    /// </remarks>
    public override void Flush()
    {
    }

    /// <summary>
    /// Overrides the <see cref="Stream.FlushAsync(CancellationToken)"/> method, performing no action.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous flush operation.</returns>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled. This exception is stored into the returned task.</exception>
    /// <remarks>
    /// <para>Since a <see cref="ReadOnlyMemoryStream"/> cannot be written to, this method is redundant.
    /// However, flushing a read-only stream is a valid operation; therefore this method simply returns
    /// a completed <see cref="Task"/> without doing anything, unless <paramref name="cancellationToken"/>
    /// has been canceled.</para>
    /// </remarks>
    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Reads a block of bytes from the current stream and writes the data to a buffer.
    /// </summary>
    /// <param name="buffer">When this method returns, contains the specified byte array
    /// with the values between <c>offset</c> and <c>(offset + count - 1)</c> replaced by the characters read from the current stream.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing data from the current stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <returns>The total number of bytes written into the buffer.
    /// This can be less than the number of bytes requested if that number of bytes are not currently available,
    /// or zero if the end of the stream is reached before any bytes are read.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
    /// <exception cref="ArgumentException"><paramref name="offset"/> subtracted from the length of <paramref name="buffer"/> is less than <paramref name="count"/>.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override int Read(byte[] buffer, int offset, int count)
    {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive
        ValidateBufferArguments(buffer, offset, count);
#pragma warning restore CA1062 // Validate arguments of public methods
        EnsureNotClosed();

        var n = _data.Length - _position;
        if (n > count)
        {
            n = count;
        }

        if (n <= 0)
        {
            return 0;
        }

        var span = _data.Span.Slice(_position, count);
        span.CopyTo(buffer.AsSpan(offset));
        _position += span.Length;
        return span.Length;
    }

    /// <summary>
    /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
    /// </summary>
    /// <param name="buffer">A region of memory. When this method returns, the contents of this span are replaced by the bytes read from the current stream.</param>
    /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes allocated in the buffer if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <remarks>
    /// <para>This method overrides the corresponding method of <see cref="Stream"/> on runtimes where it is available (.NET Core 2.1 and newer,
    /// .NET 5 and newer, .NET Standard 2.1 and newer). On older runtimes, this method is present as a method of <see cref="ReadOnlyMemoryStream"/>
    /// and depends on the <c>System.Memory</c> NuGet package.</para>
    /// </remarks>
    public
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    override
#endif
    int Read(Span<byte> buffer)
    {
        EnsureNotClosed();

        var n = Math.Min(_data.Length - _position, buffer.Length);
        if (n <= 0)
        {
            return 0;
        }

        _data.Span.Slice(_position, n).CopyTo(buffer);
        _position += n;
        return n;
    }

    /// <summary>
    /// Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.
    /// </summary>
    /// <param name="buffer">The buffer to write the data into.</param>
    /// <param name="offset">The byte offset in <paramref name="buffer"/> at which to begin writing data from the stream.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// <para>A task that represents the asynchronous read operation.</para>
    /// <para>The <see cref="Task{T}.Result">Result</see> of the returned task will be the total number of bytes read into the buffer.</para>
    /// <para>The result value can be less than the number of bytes requested if the number of bytes currently available
    /// is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</para>
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
    /// <exception cref="ArgumentException"><paramref name="offset"/> subtracted from the length of <paramref name="buffer"/> is less than <paramref name="count"/>.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled. This exception is stored into the returned task.</exception>
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        EnsureNotClosed();
#pragma warning disable CA1062 // Validate arguments of public methods - False positive
        ValidateBufferArguments(buffer, offset, count);
#pragma warning restore CA1062 // Validate arguments of public methods

        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<int>(cancellationToken);
        }

        try
        {
            var n = Read(buffer, offset, count);
            return GetCompletedInt32Task(n);
        }
        catch (OperationCanceledException e)
        {
            return Task.FromCanceled<int>(e.CancellationToken);
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            return Task.FromException<int>(e);
        }
    }

    /// <summary>
    /// Asynchronously reads a sequence of bytes from the current stream, writes the sequence into <paramref name="buffer"/>,
    /// advances the position within the stream by the number of bytes read, and monitors cancellation requests.
    /// </summary>
    /// <param name="buffer">The region of memory to write the data into.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// <para>A task that represents the asynchronous read operation.</para>
    /// <para>The <see cref="Task{T}.Result">Result</see> of the returned task will be the total number of bytes read into <paramref name="buffer"/>.</para>
    /// <para>The result value can be less than the number of bytes requested if the number of bytes currently available
    /// is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</para>
    /// </returns>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled. This exception is stored into the returned task.</exception>
    /// <remarks>
    /// <para>This method overrides the corresponding method of <see cref="Stream"/> on runtimes where it is available (.NET Core 2.1 and newer,
    /// .NET 5 and newer, .NET Standard 2.1 and newer). On older runtimes, this method is present as a method of <see cref="ReadOnlyMemoryStream"/>
    /// and depends on the <c>System.Memory</c> NuGet package.</para>
    /// </remarks>
    public
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    override
#endif
    ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
    {
        EnsureNotClosed();
        if (cancellationToken.IsCancellationRequested)
        {
            return new(Task.FromCanceled<int>(cancellationToken));
        }

        try
        {
            return new(Read(buffer.Span));
        }
        catch (OperationCanceledException e)
        {
            return new(Task.FromCanceled<int>(e.CancellationToken));
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            return new(Task.FromException<int>(e));
        }
    }

    /// <summary>
    /// Reads a byte from the current stream.
    /// </summary>
    /// <returns>The byte cast to an <see cref="int"/>, or -1 if the end of the stream has been reached.</returns>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override int ReadByte()
    {
        EnsureNotClosed();

        if (_position >= _data.Length)
        {
            return -1;
        }

        return _data.Span[_position++];
    }

    /// <summary>
    /// Reads the bytes from the current stream and writes them to another stream, using a specified buffer size.
    /// </summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size of the buffer. This value must be greater than zero.</param>
    /// <exception cref="ArgumentNullException"><paramref name="destination"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is zero or negative.</exception>
    /// <exception cref="ObjectDisposedException">Either this stream or <paramref name="destination"/> is closed.</exception>
    /// <remarks>
    /// <para>This method overrides the corresponding method of <see cref="Stream"/> on runtimes where it is <c>virtual</c> (.NET Core 2.0 and newer,
    /// .NET 5 and newer, .NET Standard 2.1 and newer). On older runtimes, this method can be accessed as a method of <see cref="ReadOnlyMemoryStream"/>,
    /// while a <see cref="Stream"/> reference will use the standard implementation, which works but is slower and may allocate more memory.</para>
    /// <para>Since this method copies all the contents of the stream (starting from the current <see cref="Position"/>) in one go,
    /// the <paramref name="bufferSize"/> parameter is not used; it is still validated for backward compatibility.</para>
    /// </remarks>
    public
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    override
#else
    new
#endif
    void CopyTo(Stream destination, int bufferSize)
    {
        // Validate arguments for compatibility,
        // although bufferSize is only for show, as we're going to copy all data in one go.
#pragma warning disable CA1062 // Validate arguments of public methods - False positive
        ValidateCopyToArguments(destination, bufferSize);
#pragma warning restore CA1062 // Validate arguments of public methods
        EnsureNotClosed();

        // EmulateRead will increment _position, so save its value
        var originalPosition = _position;
        var remaining = EmulateRead(_data.Length - originalPosition);
        if (remaining <= 0)
        {
            return;
        }

#pragma warning disable CA1062 // Validate arguments of public methods - False positive
        CopyToInternal(destination, originalPosition);
#pragma warning restore CA1062 // Validate arguments of public methods
    }

    /// <summary>
    /// Asynchronously reads all the bytes from the current stream and writes them to another stream, using a specified buffer size,
    /// and monitors cancellation requests.
    /// </summary>
    /// <param name="destination">The stream to which the contents of the current stream will be copied.</param>
    /// <param name="bufferSize">The size, in bytes, of the buffer. This value must be greater than zero.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="destination"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is zero or negative.</exception>
    /// <exception cref="ObjectDisposedException">Either this stream or <paramref name="destination"/> is closed.</exception>
    /// <exception cref="OperationCanceledException">The cancellation token was canceled. This exception is stored into the returned task.</exception>
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
    {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive
        ValidateCopyToArguments(destination, bufferSize);
#pragma warning restore CA1062 // Validate arguments of public methods
        EnsureNotClosed();

        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled(cancellationToken);
        }

        // EmulateRead will increment _position, so save its value
        var originalPosition = _position;
        var remaining = EmulateRead(_data.Length - _position);
        if (remaining <= 0)
        {
            return Task.CompletedTask;
        }

        // If destination is not a MemoryStream, write to it asynchronously.
        if (destination is not MemoryStream destinationMemoryStream)
        {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            // On runtimes that have Stream.WriteAsync(ReadOnlyMemory<byte>, CancellationToken), just use that.
#pragma warning disable CA1062 // Validate arguments of public methods - False positive
            return destination.WriteAsync(_data.Slice(originalPosition, remaining), cancellationToken).AsTask();
#pragma warning restore CA1062 // Validate arguments of public methods
#else
            // If we cannot use WriteAsync(ReadOnlyMemory<byte>, CancellationToken), we need an array.
            // Our data, being a ReadOnlyMemory<byte>, could (and most times will) be backed by an array or a portion thereof.
            // Fortunately, MemoryMarshal can tell us whether this is the case
            // and provide us with a reference to the array and the starting index
            // of the ReadOnlyMemory, sparing us the use of ArrayPool for a temporary array.
            if (MemoryMarshal.TryGetArray(_data, out var dataArray))
            {
                // If our data is actually in an array, write directly from that.
                return destination.WriteAsync(dataArray.Array, dataArray.Offset + originalPosition, remaining, cancellationToken);
            }

            // In the worst case, we have to use a temporary array as a buffer.
            var buffer = ArrayPool<byte>.Shared.Rent(remaining);
            try
            {
                _data.Span[originalPosition.._position].CopyTo(buffer);
                return destination.WriteAsync(buffer, 0, remaining, cancellationToken);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
#endif
        }

        // If destination is a MemoryStream, there's no need to go asynchronous.
        try
        {
            CopyToInternal(destination, originalPosition);
            return Task.CompletedTask;
        }
        catch (Exception e) when (!e.IsCriticalError())
        {
            return Task.FromException(e);
        }
    }

    /// <summary>
    /// Sets the position within the current stream to the specified value.
    /// </summary>
    /// <param name="offset">The new position within the stream. This is relative to the <paramref name="origin"/> parameter, and can be positive or negative.</param>
    /// <param name="origin">A value of type <see cref="SeekOrigin"/>, which acts as the seek reference point.</param>
    /// <returns>The new position within the stream, calculated by combining the initial reference point and the offset.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is greater than <see cref="int.MaxValue"/>.</exception>
    /// <exception cref="IOException">Seeking is attempted before the beginning of the stream.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    public override long Seek(long offset, SeekOrigin origin)
    {
        EnsureNotClosed();
        Guard.IsLessThanOrEqualTo(offset, int.MaxValue);

        switch (origin)
        {
            case SeekOrigin.Begin:
                {
                    var tempPosition = unchecked((int)offset);
                    if (offset < 0 || tempPosition < 0)
                    {
                        ThrowSeekBeforeBegin();
                    }

                    _position = tempPosition;
                    break;
                }

            case SeekOrigin.Current:
                {
                    var tempPosition = unchecked(_position + (int)offset);
                    if (unchecked(_position + offset) < 0 || tempPosition < 0)
                    {
                        ThrowSeekBeforeBegin();
                    }

                    _position = tempPosition;
                    break;
                }

            case SeekOrigin.End:
                {
                    var tempPosition = unchecked(_data.Length + (int)offset);
                    if (unchecked(_data.Length + offset) < 0 || tempPosition < 0)
                    {
                        ThrowSeekBeforeBegin();
                    }

                    _position = tempPosition;
                    break;
                }

            default:
                ThrowHelper.ThrowArgumentException("Invalid seek origin.");
                break;
        }

        return _position;

        [DoesNotReturn]
        static void ThrowSeekBeforeBegin()
            => throw new IOException("An attempt was made to move the position before the beginning of the stream.");
    }

    /// <summary>
    /// <para>Sets the length of the current stream to the specified value.</para>
    /// <para>Since a <see cref="ReadOnlyMemoryStream"/> cannot be written to, this method will always throw a <see cref="NotSupportedException"/>.</para>
    /// </summary>
    /// <param name="value">The value at which to set the length.</param>
    /// <exception cref="NotSupportedException">Always thrown, because instances of <see cref="ReadOnlyMemoryStream"/> are read-only.</exception>
    public override void SetLength(long value) => ThrowUnwritableStream();

    /// <summary>
    /// <para>Writes a block of bytes to the current stream using data read from a buffer.</para>
    /// <para>Since a <see cref="ReadOnlyMemoryStream"/> cannot be written to, this method will always throw a <see cref="NotSupportedException"/>.</para>
    /// </summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <exception cref="NotSupportedException">Always thrown, because instances of <see cref="ReadOnlyMemoryStream"/> are read-only.</exception>
    public override void Write(byte[] buffer, int offset, int count) => ThrowUnwritableStream();

    /// <summary>
    /// <para>Writes a sequence of bytes into the current stream and advances the current position within this stream by the number of bytes written.</para>
    /// <para>Since a <see cref="ReadOnlyMemoryStream"/> cannot be written to, this method will always throw a <see cref="NotSupportedException"/>.</para>
    /// </summary>
    /// <param name="buffer">A region of memory.</param>
    /// <exception cref="NotSupportedException">Always thrown, because instances of <see cref="ReadOnlyMemoryStream"/> are read-only.</exception>
    /// <remarks>
    /// <para>This method overrides the corresponding method of <see cref="Stream"/> on runtimes where it is available (.NET Core 2.0 and newer,
    /// .NET 5 and newer, .NET Standard 2.1 and newer). On older runtimes, this method is present as a method of <see cref="ReadOnlyMemoryStream"/>
    /// and depends on the <c>System.Memory</c> NuGet package.</para>
    /// </remarks>
    public
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    override
#endif
#pragma warning disable CA1822 // Mark members as static - false positive, as this is an override
    void Write(ReadOnlySpan<byte> buffer) => ThrowUnwritableStream();
#pragma warning disable CA1822 // Mark members as static

    /// <summary>
    /// <para>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</para>
    /// <para>Since a <see cref="ReadOnlyMemoryStream"/> cannot be written to, this method will always throw a <see cref="NotSupportedException"/>.</para>
    /// </summary>
    /// <param name="buffer">The buffer to write data from.</param>
    /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> from which to begin copying bytes to the stream.</param>
    /// <param name="count">The maximum number of bytes to write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <exception cref="NotSupportedException">Always thrown, because instances of <see cref="ReadOnlyMemoryStream"/> are read-only.</exception>
#pragma warning disable CA1822 // Mark members as static - false positive, as this is an override
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => ThrowUnwritableStream<Task>();
#pragma warning disable CA1822 // Mark members as static

    /// <summary>
    /// <para>Asynchronously writes a sequence of bytes into the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</para>
    /// <para>Since a <see cref="ReadOnlyMemoryStream"/> cannot be written to, this method will always throw a <see cref="NotSupportedException"/>.</para>
    /// </summary>
    /// <param name="buffer">The region of memory to write data from.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    /// <exception cref="NotSupportedException">Always thrown, because instances of <see cref="ReadOnlyMemoryStream"/> are read-only.</exception>
    /// <remarks>
    /// <para>This method overrides the corresponding method of <see cref="Stream"/> on runtimes where it is available (.NET Core 2.0 and newer,
    /// .NET 5 and newer, .NET Standard 2.1 and newer). On older runtimes, this method is present as a method of <see cref="ReadOnlyMemoryStream"/>
    /// and depends on the <c>System.Memory</c> NuGet package.</para>
    /// </remarks>
#pragma warning disable CA1822 // Mark members as static - false positive, as this is an override
    public
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    override
#endif
    ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken) => ThrowUnwritableStream<ValueTask>();
#pragma warning disable CA1822 // Mark members as static

    /// <summary>
    /// <para>Writes a byte to the current stream at the current position.</para>
    /// <para>Since a <see cref="ReadOnlyMemoryStream"/> cannot be written to, this method will always throw a <see cref="NotSupportedException"/>.</para>
    /// </summary>
    /// <param name="value">The byte to write.</param>
    /// <exception cref="NotSupportedException">Always thrown, because instances of <see cref="ReadOnlyMemoryStream"/> are read-only.</exception>
#pragma warning disable CA1822 // Mark members as static - false positive, as this is an override
    public override void WriteByte(byte value) => ThrowUnwritableStream();
#pragma warning disable CA1822 // Mark members as static

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="ReadOnlyMemoryStream"/> class and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to release both managed and unmanaged resources;
    /// <see langword="false"/> to release only unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _isOpen = false;
            _lastCompletedInt32Task = null;
        }

        base.Dispose(disposing);
    }

#if !NET6_0_OR_GREATER

    private static void ValidateBufferArguments([ValidatedNotNull] byte[]? buffer, int offset, int count)
    {
        Guard.IsNotNull(buffer);
        Guard.IsGreaterThanOrEqualTo(offset, 0);
        if ((uint)count > buffer.Length - offset)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(count), "Offset and length were out of bounds for the array.");
        }
    }

    private static void ValidateCopyToArguments([ValidatedNotNull] Stream? destination, int bufferSize)
    {
        Guard.IsNotNull(destination);
        Guard.IsGreaterThan(bufferSize, 0);
        if (!destination.CanWrite)
        {
            if (destination.CanRead)
            {
                ThrowUnwritableStream();
            }

            ThrowClosedStream();
        }
    }

#endif

    [DoesNotReturn]
    private static void ThrowUnwritableStream()
        => ThrowHelper.ThrowNotSupportedException("Stream does not support writing.");

    [DoesNotReturn]
    private static T ThrowUnwritableStream<T>()
        => ThrowHelper.ThrowNotSupportedException<T>("Stream does not support writing.");

    [DoesNotReturn]
    private static void ThrowClosedStream()
        => ThrowHelper.ThrowObjectDisposedException("Cannot access a closed Stream.");

    private void EnsureNotClosed()
    {
        if (!_isOpen)
        {
            ThrowClosedStream();
        }
    }

    private int EmulateRead(int count)
    {
        EnsureNotClosed();
        var n = RangeCheck.Clamp(_data.Length - _position, 0, count);
        _position += n;
        return n;
    }

    private void CopyToInternal(Stream destination, int originalPosition)
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        // On runtimes that have Stream.Write(ReadOnlySpan<byte>), just use that.
        destination.Write(_data.Span[originalPosition.._position]);
#else
        // If we cannot use Write(ReadOnlySpan<byte>), we need an array.
        // Our data, being a ReadOnlyMemory<byte>, could (and most times will) be backed by an array or a portion thereof.
        // Fortunately, MemoryMarshal can tell us whether this is the case
        // and provide us with a reference to the array and the starting index
        // of the ReadOnlyMemory, sparing us the use of ArrayPool for a temporary array.
        var remaining = _position - originalPosition;
        if (MemoryMarshal.TryGetArray(_data, out var dataArray))
        {
            // If our data is actually in an array, write directly from that.
            destination.Write(dataArray.Array, dataArray.Offset + originalPosition, remaining);
            return;
        }

        // In the worst case, we have to use a temporary array as a buffer.
        var buffer = ArrayPool<byte>.Shared.Rent(remaining);
        try
        {
            _data.Span[originalPosition.._position].CopyTo(buffer);
            destination.Write(buffer, 0, remaining);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
#endif
    }

    private Task<int> GetCompletedInt32Task(int result)
    {
#pragma warning disable CA1849 // Call async methods when in an async method - Result will not block here, because the task is completed
        if (_lastCompletedInt32Task is not null && _lastCompletedInt32Task.Result == result)
        {
            return _lastCompletedInt32Task;
        }
#pragma warning restore CA1849 // Call async methods when in an async method

        return _lastCompletedInt32Task = Task.FromResult(result);
    }
}

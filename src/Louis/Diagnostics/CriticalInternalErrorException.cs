// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Louis.Diagnostics;

/// <summary>
/// The exception that is thrown by methods of the <see cref="SelfCheck"/> class
/// to signal an unrecoverable condition, most probably caused by an internal error in a library
/// or application, that should cause an application to terminate immediately.
/// </summary>
[Serializable]
public class CriticalInternalErrorException : InternalErrorException, ICriticalError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CriticalInternalErrorException"/> class.
    /// </summary>
    public CriticalInternalErrorException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CriticalInternalErrorException"/> class
    /// with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public CriticalInternalErrorException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CriticalInternalErrorException"/> class
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception,
    /// or a <see langword="null"/> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.</param>
    public CriticalInternalErrorException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CriticalInternalErrorException"/> class
    /// with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data
    /// about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information
    /// about the source or destination.</param>
    protected CriticalInternalErrorException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}

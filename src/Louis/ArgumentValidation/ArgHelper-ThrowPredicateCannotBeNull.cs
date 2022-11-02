// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Louis.Diagnostics;

namespace Louis.ArgumentValidation;

partial class ArgHelper
{
    private const string PredicateCannotBeNullMessage = "Predicate cannot be null.";

    /// <summary>
    /// Throws a new <see cref="InternalErrorException"/> with a message
    /// stating that a predicate passed to the calling validation method should not be <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument being validated.</typeparam>
    /// <param name="arg">The <see cref="Arg{T}"/> struct being used for evaluation.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="InternalErrorException">Always thrown.</exception>
    [DoesNotReturn]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static Arg<T> ThrowPredicateCannotBeNull<T>(Arg<T> arg)
        where T : class
        => throw new InternalErrorException(PredicateCannotBeNullMessage);

    /// <summary>
    /// Throws a new <see cref="InternalErrorException"/> with a message
    /// stating that a predicate passed to the calling validation method should not be <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument being validated.</typeparam>
    /// <param name="arg">The <see cref="NullableArg{T}"/> struct being used for evaluation.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="InternalErrorException">Always thrown.</exception>
    [DoesNotReturn]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static NullableArg<T> ThrowPredicateCannotBeNull<T>(NullableArg<T> arg)
        where T : class
        => throw new InternalErrorException(PredicateCannotBeNullMessage);

    /// <summary>
    /// Throws a new <see cref="InternalErrorException"/> with a message
    /// stating that a predicate passed to the calling validation method should not be <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument being validated.</typeparam>
    /// <param name="arg">The <see cref="ValueArg{T}"/> struct being used for evaluation.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="InternalErrorException">Always thrown.</exception>
    [DoesNotReturn]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static ValueArg<T> ThrowPredicateCannotBeNull<T>(ValueArg<T> arg)
        where T : struct
        => throw new InternalErrorException(PredicateCannotBeNullMessage);

    /// <summary>
    /// Throws a new <see cref="InternalErrorException"/> with a message
    /// stating that a predicate passed to the calling validation method should not be <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument being validated.</typeparam>
    /// <param name="arg">The <see cref="NullableValueArg{T}"/> struct being used for evaluation.</param>
    /// <returns>This method never returns.</returns>
    /// <exception cref="InternalErrorException">Always thrown.</exception>
    [DoesNotReturn]
    [DebuggerHidden]
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static NullableValueArg<T> ThrowPredicateCannotBeNull<T>(NullableValueArg<T> arg)
        where T : struct
        => throw new InternalErrorException(PredicateCannotBeNullMessage);
}

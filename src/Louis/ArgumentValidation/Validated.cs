// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Louis.ArgumentValidation.Internal;
using Louis.Diagnostics;
using PolyKit.Diagnostics.CodeAnalysis;

namespace Louis.ArgumentValidation;

/// <summary>
/// Provides methods to validate non-nullable arguments.
/// </summary>
[StackTraceHidden]
public static class Validated
{
    /// <summary>
    /// Checks that an argument whose type is non-nullable, ensuring that it is not <see langword="null"/>.
    /// This method does not initiate a chain of validations, but is faster and consumes less stack
    /// than <see cref="Require.NotNull{T}(T,string)"/> or <see cref="Require.NotNull{T}(T?,string)"/>.
    /// More importantly, this method can be used when <typeparamref name="T"/> is an open generic type
    /// with neither a <c>class</c> nor <c>struct</c> constraint.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>The value of the argument as passed.</returns>
    /// <exception cref="InternalErrorException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>The <paramref name="name"/> parameter, if omitted, will be equal to the text of the expression
    /// passed as <paramref name="value"/>. This allows for shorter and cleaner code in the great majority
    /// of use cases.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Validated.NotNull(bar);
    ///     // bar is now guaranteed to be non-null
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Validated.NotNull(bar, nameof(bar));
    ///     // bar is now guaranteed to be non-null
    /// }
    /// </code>
    /// </remarks>
    public static T NotNull<T>([ValidatedNotNull] T? value, [CallerArgumentExpression("value")] string name = "")
        where T : notnull
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAs<T>()
            : value is null ? ThrowHelper.ThrowArgumentNullAs<T>(name)
            : value;

    /// <summary>
    /// Checks a <see langword="string"/> argument, ensuring that it is neither <see langword="null"/> nor the empty string (<c>""</c>).
    /// This method does not initiate a chain of validations like <see cref="Require.NotNullOrEmpty"/>,
    /// but is faster and consumes less stack.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>The value of the argument as passed.</returns>
    /// <exception cref="InternalErrorException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <para><paramref name="value"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="value"/> is an empty string (<c>""</c>).</para>
    /// </exception>
    /// <remarks>
    /// <para>The <paramref name="name"/> parameter, if omitted, will be equal to the text of the expression
    /// passed as <paramref name="value"/>. This allows for shorter and cleaner code in the great majority
    /// of use cases.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Validated.NotNullOrEmpty(bar);
    ///     // bar is now guaranteed to be non-null and contain at least one character
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Validated.NotNullOrEmpty(bar, nameof(bar));
    ///     // bar is now guaranteed to be non-null and contain at least one character
    /// }
    /// </code>
    /// </remarks>
    public static string NotNullOrEmpty([ValidatedNotNull] string? value, [CallerArgumentExpression("value")] string name = "")
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAs<string>()
            : value is null ? ThrowHelper.ThrowArgumentNullAs<string>(name)
            : value.Length == 0 ? ThrowHelper.ThrowArgumentEmptyAsString(name)
            : value;

    /// <summary>
    /// Checks a <see langword="string"/> argument, ensuring that it is neither <see langword="null"/>, an empty string (<c>""</c>),
    /// nor consisting entirely of white-space characters.
    /// This method does not initiate a chain of validations like <see cref="Require.NotNullOrWhiteSpace"/>,
    /// but is faster and consumes less stack.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>The value of the argument as passed.</returns>
    /// <exception cref="InternalErrorException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">
    /// <para><paramref name="value"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="value"/> is an empty string (<c>""</c>).</para>
    /// <para>- or -</para>
    /// <para><paramref name="value"/> only contains white-space characters.</para>
    /// </exception>
    /// <remarks>
    /// <para>The <paramref name="name"/> parameter, if omitted, will be equal to the text of the expression
    /// passed as <paramref name="value"/>. This allows for shorter and cleaner code in the great majority
    /// of use cases.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Validated.NotNullOrWhiteSpace(bar);
    ///     // bar is now guaranteed to be non-null and contain at least one non-white-space character
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Validated.NotNullOrWhiteSpace(bar, nameof(bar));
    ///     // bar is now guaranteed to be non-null and contain at least one non-white-space character
    /// }
    /// </code>
    /// </remarks>
    public static string NotNullOrWhiteSpace([ValidatedNotNull] string? value, [CallerArgumentExpression("value")] string name = "")
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAs<string>()
            : value is null ? ThrowHelper.ThrowArgumentNullAs<string>(name)
            : value.Length == 0 ? ThrowHelper.ThrowArgumentEmptyAsString(name)
            : string.IsNullOrWhiteSpace(value) ? ThrowHelper.ThrowArgumentWhiteSpaceAsString(name)
            : value;
}

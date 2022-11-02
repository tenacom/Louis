// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Louis.ArgumentValidation.Internal;
using Louis.Diagnostics;
using PolyKit.Diagnostics.CodeAnalysis;

namespace Louis.ArgumentValidation;

/// <summary>
/// Provides methods to check that non-nullable arguments are not null,
/// as well as initiate argument checking via the <see cref="Arg{T}"/> struct.
/// </summary>
[StackTraceHidden]
public static class Require
{
    /// <summary>
    /// Initiates checks on an argument whose type is a non-nullable value type (<see langword="struct"/>).
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>A <see cref="ValueArg{T}"/> struct that can be used to perform checks.</returns>
    /// <exception cref="InternalErrorException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>The <paramref name="name"/> parameter, if omitted, will be equal to the text of the expression
    /// passed as <paramref name="value"/>. This allows for shorter and cleaner code in the great majority
    /// of use cases.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(int bar)
    /// {
    ///     _ = Require.Of(bar).GreaterThanZero();
    ///     // bar is now guaranteed to be > 0
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(int bar)
    /// {
    ///     _ = Require.Of(bar, nameof(bar)).GreaterThanZero();
    ///     // bar is now guaranteed to be > 0
    /// }
    /// </code>
    /// </remarks>
    public static ValueArg<T> Of<T>(T value, [CallerArgumentExpression("value")] string name = "")
        where T : struct
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAsValueArg<T>() : new(name, value);

    /// <summary>
    /// Initiates checks on an argument whose type is a nullable reference type (<see langword="class"/>).
    /// </summary>
    /// <typeparam name="T">The type of the argument (e.g. <c>string</c> if the type of the argument is <c>string?</c>).</typeparam>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>A <see cref="NullableArg{T}"/> struct that can be used to perform further checks.</returns>
    /// <exception cref="InternalErrorException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>The <paramref name="name"/> parameter, if omitted, will be equal to the text of the expression
    /// passed as <paramref name="value"/>. This allows for shorter and cleaner code in the great majority
    /// of use cases.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(string? bar)
    /// {
    ///     _ = Require.Nullable(bar).NotEmpty();
    ///     // bar is now guaranteed to not be the empty string (but it can be null)
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string? bar)
    /// {
    ///     _ = Require.Nullable(bar, nameof(bar)).NotEmpty();
    ///     // bar is now guaranteed to not be the empty string (but it can be null)
    /// }
    /// </code>
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - False positive: name is optional because of CallerArgumentExpressionAttribute.
    public static NullableArg<T> Nullable<T>(T? value, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : class
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAsNullableArg<T>() : new(name, value);

    /// <summary>
    /// Initiates checks on an argument whose type is a nullable value type (<see cref="System.Nullable{T}"/>).
    /// </summary>
    /// <typeparam name="T">The underlying type of the argument (e.g. <c>int</c> if the type of the argument is <c>int?</c>).</typeparam>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>A <see cref="NullableValueArg{T}"/> struct that can be used to perform further checks.</returns>
    /// <exception cref="InternalErrorException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>The <paramref name="name"/> parameter, if omitted, will be equal to the text of the expression
    /// passed as <paramref name="value"/>. This allows for shorter and cleaner code in the great majority
    /// of use cases.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(int? bar)
    /// {
    ///     _ = Require.Nullable(bar).GreaterThanZero();
    ///     // bar is now guaranteed to not be either null or > 0
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(int? bar)
    /// {
    ///     _ = Require.Nullable(bar, nameof(bar)).GreaterThanZero();
    ///     // bar is now guaranteed to not be either null or > 0
    /// }
    /// </code>
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - False positive: name is optional because of CallerArgumentExpressionAttribute.
    public static NullableValueArg<T> Nullable<T>(T? value, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : struct
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAsNullableValueArg<T>() : new(name, value);

    /// <summary>
    /// Initiates checks on an argument whose type is a reference type (<see langword="class"/>),
    /// ensuring that it is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the argument.</typeparam>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>An <see cref="Arg{T}"/> struct that can be used to perform further checks.</returns>
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
    ///     _ = Require.NotNull(bar);
    ///     // bar is now guaranteed to be non-null
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Require.NotNull(bar, nameof(bar));
    ///     // bar is now guaranteed to be non-null
    /// }
    /// </code>
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - False positive: name is optional because of CallerArgumentExpressionAttribute.
    public static Arg<T> NotNull<T>([ValidatedNotNull] T? value, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : class
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAsArg<T>()
            : value is null ? ThrowHelper.ThrowArgumentNullAsArg<T>(name)
            : new(name, value);

    /// <summary>
    /// Initiates checks on an argument whose type is a nullable value type (<see cref="System.Nullable{T}"/>),
    /// ensuring that it is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The underlying type of the argument (e.g. <c>int</c> if the type of the argument is <c>int?</c>).</typeparam>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>A <see cref="ValueArg{T}"/> struct that can be used to perform further checks.</returns>
    /// <exception cref="InternalErrorException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// <para>The <paramref name="name"/> parameter, if omitted, will be equal to the text of the expression
    /// passed as <paramref name="value"/>. This allows for shorter and cleaner code in the great majority
    /// of use cases.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(int? bar)
    /// {
    ///     _ = Require.NotNull(bar).GreaterThanZero();
    ///     // bar is now guaranteed to be non-null and > 0
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(int? bar)
    /// {
    ///     _ = Require.NotNull(bar, nameof(bar)).GreaterThanZero();
    ///     // bar is now guaranteed to be non-null and > 0
    /// }
    /// </code>
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - False positive: name is optional because of CallerArgumentExpressionAttribute.
    public static ValueArg<T> NotNull<T>([ValidatedNotNull] T? value, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : struct
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAsValueArg<T>()
            : value.HasValue ? new(name, value.Value)
            : ThrowHelper.ThrowArgumentNullAsValueArg<T>(name);

    /// <summary>
    /// Initiates checks on a <see langword="string"/> argument,
    /// ensuring that it is neither <see langword="null"/> nor an empty string (<c>""</c>).
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>An <see cref="Arg{T}">Arg&lt;string&gt;</see> struct that can be used to perform further checks.</returns>
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
    ///     _ = Require.NotNullOrEmpty(bar);
    ///     // bar is now guaranteed to be non-null and contain at least one character
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Require.NotNullOrEmpty(bar, nameof(bar));
    ///     // bar is now guaranteed to be non-null and contain at least one character
    /// }
    /// </code>
    /// </remarks>
    public static Arg<string> NotNullOrEmpty([ValidatedNotNull] string? value, [CallerArgumentExpression("value")] string name = "")
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAsArg<string>()
            : value is null ? ThrowHelper.ThrowArgumentNullAsArg<string>(name)
            : value.Length == 0 ? ThrowHelper.ThrowArgumentEmptyAsArgOfString(name)
            : new(name, value);

    /// <summary>
    /// Initiates checks on a <see langword="string"/> argument,
    /// ensuring that it is neither <see langword="null"/>, an empty string (<c>""</c>),
    /// nor consisting entirely of white-space characters.
    /// </summary>
    /// <param name="value">The value of the argument.</param>
    /// <param name="name">
    /// <para>The name of the argument.</para>
    /// <para>This parameter can be omitted; see the "Remarks" section for details.</para>
    /// </param>
    /// <returns>An <see cref="Arg{T}">Arg&lt;string&gt;</see> struct that can be used to perform further checks.</returns>
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
    ///     _ = Require.NotNullOrWhiteSpace(bar);
    ///     // bar is now guaranteed to be non-null and contain at least one non-white-space character
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Require.NotNullOrWhiteSpace(bar, nameof(bar));
    ///     // bar is now guaranteed to be non-null and contain at least one non-white-space character
    /// }
    /// </code>
    /// </remarks>
    public static Arg<string> NotNullOrWhiteSpace([ValidatedNotNull] string? value, [CallerArgumentExpression("value")] string name = "")
        => name is null ? ThrowHelper.ThrowArgumentNameCannotBeNullAsArg<string>()
            : value is null ? ThrowHelper.ThrowArgumentNullAsArg<string>(name)
            : value.Length == 0 ? ThrowHelper.ThrowArgumentEmptyAsArgOfString(name)
            : string.IsNullOrWhiteSpace(value) ? ThrowHelper.ThrowArgumentWhiteSpaceAsArgOfString(name)
            : new(name, value);
}

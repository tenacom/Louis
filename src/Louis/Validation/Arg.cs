// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Louis.Diagnostics;
using PolyKit.Diagnostics.CodeAnalysis;

namespace Louis.ArgumentValidation;

/// <summary>
/// Provides methods to check that non-nullable arguments are not null,
/// as well as initiate argument checking via the <see cref="Arg{T}"/> struct.
/// </summary>
[StackTraceHidden]
public static partial class Arg
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
    /// <para>The <paramref name="name"/> parameter can be omitted if <paramref name="value"/>
    /// is specified using the name of a parameter of the calling method.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(int bar)
    /// {
    ///     _ = Arg.Value(bar).GreaterThanZero();
    ///
    ///     // Do something with bar, which is guaranteed to be > 0
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(int bar)
    /// {
    ///     _ = Arg.Value(bar, nameof(bar)).GreaterThanZero();
    ///
    ///     // Do something with bar, which is guaranteed to be > 0
    /// }
    /// </code>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueArg<T> Value<T>(T value, [CallerArgumentExpression("value")] string name = "")
        where T : struct
        => name is null ? throw ArgumentNameCannotBeNull() : new(name, value);

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
    /// <para>The <paramref name="name"/> parameter can be omitted if <paramref name="value"/>
    /// is specified using the name of a parameter of the calling method.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(string? bar)
    /// {
    ///     _ = Arg.Nullable(bar).NotEmpty();
    ///
    ///     // Do something with bar, which is guaranteed to not be an empty string
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string? bar)
    /// {
    ///     _ = Arg.Nullable(bar, nameof(bar)).NotEmpty();
    ///
    ///     // Do something with bar, which is guaranteed to not be an empty string
    /// }
    /// </code>
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - False positive: name is optional because of CallerArgumentExpressionAttribute.
    public static NullableArg<T> Nullable<T>(T? value, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : class
        => name is null ? throw ArgumentNameCannotBeNull() : new(name, value);

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
    /// <para>The <paramref name="name"/> parameter can be omitted if <paramref name="value"/>
    /// is specified using the name of a parameter of the calling method.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(int? bar)
    /// {
    ///     _ = Arg.Nullable(bar).GreaterThanZero();
    ///
    ///     // Do something with bar, which is guaranteed to be either null or > 0
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(int? bar)
    /// {
    ///     _ = Arg.Nullable(bar, nameof(bar)).GreaterThanZero();
    ///
    ///     // Do something with bar, which is guaranteed to be either null or > 0
    /// }
    /// </code>
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - False positive: name is optional because of CallerArgumentExpressionAttribute.
    public static NullableValueArg<T> Nullable<T>(T? value, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : struct
        => name is null ? throw ArgumentNameCannotBeNull() : new(name, value);

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
    /// <para>The <paramref name="name"/> parameter can be omitted if <paramref name="value"/>
    /// is specified using the name of a parameter of the calling method.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Arg.NotNull(bar);
    ///
    ///     // Do something with bar, which is guaranteed to be non-null
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Arg.NotNull(bar, nameof(bar));
    ///
    ///     // Do something with bar, which is guaranteed to be non-null
    /// }
    /// </code>
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - False positive: name is optional because of CallerArgumentExpressionAttribute.
    public static Arg<T> NotNull<T>([ValidatedNotNull] T? value, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : class
        => name is null ? throw ArgumentNameCannotBeNull()
            : value is null ? throw new ArgumentNullException(name)
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
    /// <para>The <paramref name="name"/> parameter can be omitted if <paramref name="value"/>
    /// is specified using the name of a parameter of the calling method.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(int? bar)
    /// {
    ///     _ = Arg.NotNull(bar).GreaterThanZero();
    ///
    ///     // Do something with bar, which is guaranteed to be non-null and > 0
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(int? bar)
    /// {
    ///     _ = Arg.NotNull(bar, nameof(bar)).GreaterThanZero();
    ///
    ///     // Do something with bar, which is guaranteed to be non-null and > 0
    /// }
    /// </code>
    /// </remarks>
#pragma warning disable RS0026 // Do not add multiple public overloads with optional parameters - False positive: name is optional because of CallerArgumentExpressionAttribute.
    public static ValueArg<T> NotNull<T>([ValidatedNotNull] T? value, [CallerArgumentExpression("value")] string name = "")
#pragma warning restore RS0026 // Do not add multiple public overloads with optional parameters
        where T : struct
        => name is null ? throw ArgumentNameCannotBeNull()
            : value.HasValue ? new(name, value.Value)
            : throw new ArgumentNullException(name);

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
    /// <para>The <paramref name="name"/> parameter can be omitted if <paramref name="value"/>
    /// is specified using the name of a parameter of the calling method.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Arg.NotNullOrEmpty(bar);
    ///
    ///     // Do something with bar, which is guaranteed to be non-null and contain at least one character
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Arg.NotNullOrEmpty(bar, nameof(bar));
    ///
    ///     // Do something with bar, which is guaranteed to be non-null and contain at least one character
    /// }
    /// </code>
    /// </remarks>
    public static Arg<string> NotNullOrEmpty([ValidatedNotNull] string? value, [CallerArgumentExpression("value")] string name = "")
        => name is null ? throw ArgumentNameCannotBeNull()
            : value is null ? throw new ArgumentNullException(name)
            : value.Length == 0 ? throw new ArgumentException($"{name} cannot be the empty string.", name)
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
    /// <para>The <paramref name="name"/> parameter can be omitted if <paramref name="value"/>
    /// is specified using the name of a parameter of the calling method.</para>
    /// <para>For example, the following code:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Arg.NotNullOrWhiteSpace(bar);
    ///
    ///     // Do something with bar, which is guaranteed to be non-null and contain at least one non-white-space character
    /// }
    /// </code>
    /// <para>is equivalent to:</para>
    /// <code>
    /// void Foo(string bar)
    /// {
    ///     _ = Arg.NotNullOrWhiteSpace(bar, nameof(bar));
    ///
    ///     // Do something with bar, which is guaranteed to be non-null and contain at least one non-white-space character
    /// }
    /// </code>
    /// </remarks>
    public static Arg<string> NotNullOrWhiteSpace([ValidatedNotNull] string? value, [CallerArgumentExpression("value")] string name = "")
        => name is null ? throw ArgumentNameCannotBeNull()
            : value is null ? throw new ArgumentNullException(name)
            : value.Length == 0 ? throw new ArgumentException($"{name} cannot be the empty string.", name)
            : string.IsNullOrWhiteSpace(value) ? throw new ArgumentException($"{name} cannot consist only of white space.", name)
            : new(name, value!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Exception ArgumentNameCannotBeNull() => SelfCheck.Failure("Argument name cannot be null.");
}

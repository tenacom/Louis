// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Louis.Diagnostics;

namespace Louis.ArgumentValidation.Internal;

[StackTraceHidden]
internal static class ThrowHelper
{
    [DoesNotReturn]
    public static T ThrowArgumentNameCannotBeNullAs<T>()
        where T : notnull
        => throw ArgumentNameCannotBeNull();

    [DoesNotReturn]
    public static Arg<T> ThrowArgumentNameCannotBeNullAsArg<T>()
        where T : class
        => throw ArgumentNameCannotBeNull();

    [DoesNotReturn]
    public static NullableArg<T> ThrowArgumentNameCannotBeNullAsNullableArg<T>()
        where T : class
        => throw ArgumentNameCannotBeNull();

    [DoesNotReturn]
    public static ValueArg<T> ThrowArgumentNameCannotBeNullAsValueArg<T>()
        where T : struct
        => throw ArgumentNameCannotBeNull();

    [DoesNotReturn]
    public static NullableValueArg<T> ThrowArgumentNameCannotBeNullAsNullableValueArg<T>()
        where T : struct
        => throw ArgumentNameCannotBeNull();

    [DoesNotReturn]
    public static T ThrowArgumentNullAs<T>(string paramName)
        where T : notnull
        => throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    public static Arg<T> ThrowArgumentNullAsArg<T>(string paramName)
        where T : class
        => throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    public static ValueArg<T> ThrowArgumentNullAsValueArg<T>(string paramName)
        where T : struct
        => throw new ArgumentNullException(paramName);

    [DoesNotReturn]
    public static string ThrowArgumentEmptyAsString(string paramName)
        => throw ArgumentEmpty(paramName);

    [DoesNotReturn]
    public static Arg<string> ThrowArgumentEmptyAsArgOfString(string paramName)
        => throw ArgumentEmpty(paramName);

    [DoesNotReturn]
    public static string ThrowArgumentWhiteSpaceAsString(string paramName)
        => throw ArgumentWhiteSpace(paramName);

    [DoesNotReturn]
    public static Arg<string> ThrowArgumentWhiteSpaceAsArgOfString(string paramName)
        => throw ArgumentWhiteSpace(paramName);

    private static Exception ArgumentNameCannotBeNull()
        => SelfCheck.Failure("Argument name cannot be null.");

    private static Exception ArgumentEmpty(string paramName)
        => new ArgumentException($"{paramName} cannot be the empty string.", paramName);

    private static Exception ArgumentWhiteSpace(string paramName)
        => new ArgumentException($"{paramName} cannot consist only of white space.", paramName);
}

// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Louis.ArgumentValidation;

namespace Louis.Fluency;

/// <summary>
/// Provides generic extension methods to perform simple operations with fluent syntax.
/// </summary>
public static class FluentExtensions
{
    /// <summary>
    /// Invokes an action on an object and returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="action">The action to perform on <paramref name="this"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="action"/> returns.</returns>
    public static T Invoke<T>(this T @this, Action<T> action)
    {
        Arg.NotNull(action).Value(@this);
        return @this;
    }

    /// <summary>
    /// Invokes an action on an object if a condition is verified, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="condition">The condition to test.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after <paramref name="then"/>, if called, returns.</returns>
    public static T If<T>(this T @this, bool condition, FluentAction<T> then)
        => condition ? Arg.NotNull(then).Value(@this) : @this;

    /// <summary>
    /// Invokes one of two actions on an object according to a boolean condition, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="condition">The condition to test.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <param name="else">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="false"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after either <paramref name="then"/> or <paramref name="else"/> returns.</returns>
    public static T IfElse<T>(this T @this, bool condition, FluentAction<T> then, FluentAction<T> @else)
        => condition ? Arg.NotNull(then).Value(@this) : Arg.NotNull(@else).Value(@this);

    /// <summary>
    /// Selects an action to invoke on an object by comparing a given value against a list of comparands, then returns the object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TValue">The type of the value being compared.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="cases">
    /// <para>An array of pairs, where each element consists of a value to compare against <paramref name="value"/>
    /// and an optional action to perform on <paramref name="this"/> if they are equal.</para>
    /// <para>If the action is <see langword="null"/>, no action is performed if the value is equal to <paramref name="value"/>;
    /// instead, the method returns immediately.</para>
    /// </param>
    /// <returns>A reference to <paramref name="this"/> after one of the actions in <paramref name="cases"/>, if any, returns.</returns>
    /// <remarks>
    /// <para>If <paramref name="value"/> is not equal to any of the values in <paramref name="cases"/>, this method returns immediately.</para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Switch<T, TValue>(this T @this, TValue value, params (TValue Comparand, FluentAction<T>? Action)[] cases)
        where TValue : IEquatable<TValue>
        => Switch(@this, value, null, cases);

    /// <summary>
    /// Selects an action to invoke on an object by comparing a given value against a list of comparands, then returns the object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TValue">The type of the value being compared.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="value">The value to compare.</param>
    /// <param name="default">
    /// <para>An optional action to perform on <paramref name="this"/> if <paramref name="value"/> is not equal
    /// to any of the values in <paramref name="cases"/>.</para>
    /// <para>If this parameter is <see langword="null"/> and <paramref name="value"/> is not equal
    /// to any of the values in <paramref name="cases"/>, this method returns immediately.</para>
    /// </param>
    /// <param name="cases">
    /// <para>An array of pairs, where each element consists of a value to compare against <paramref name="value"/>
    /// and an optional action to perform on <paramref name="this"/> if they are equal.</para>
    /// <para>If the action is <see langword="null"/>, no action is performed if the value is equal to <paramref name="value"/>;
    /// instead, the method returns immediately.</para>
    /// </param>
    /// <returns>A reference to <paramref name="this"/> after one of the actions in <paramref name="cases"/>, if any, returns.</returns>
    public static T Switch<T, TValue>(this T @this, TValue value, FluentAction<T>? @default, params (TValue Comparand, FluentAction<T>? Action)[] cases)
        where TValue : IEquatable<TValue>
    {
        foreach (var (comparand, action) in Arg.NotNull(cases).Value)
        {
            if (value.Equals(comparand))
            {
                return action == null ? @this : action(@this);
            }
        }

        return @default == null ? @this : @default(@this);
    }

    /// <summary>
    /// Performs a specified action on an object and each element of a sequence,
    /// each time taking the result of the action as the object to pass together with the next element,
    /// then returns the result of the last action.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TElement">The type of each element of the sequence.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="sequence">The sequence of elements.</param>
    /// <param name="action">The action to perform.</param>
    /// <returns>The result of the last call to <paramref name="action"/>.</returns>
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement,int})"/>
    public static T ForEach<T, TElement>(this T @this, IEnumerable<TElement> sequence, FluentAction<T, TElement> action)
    {
        _ = Arg.NotNull(action);
        foreach (var item in Arg.NotNull(sequence).Value)
        {
            @this = action(@this, item);
        }

        return @this;
    }

    /// <summary>
    /// Performs a specified action on an object and each element of a sequence,
    /// each time taking the result of the action as the object to pass together with the next element,
    /// then returns the result of the last action.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TElement">The type of each element of the sequence.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="sequence">The sequence of elements.</param>
    /// <param name="action">The action to perform.</param>
    /// <returns>The result of the last call to <paramref name="action"/>.</returns>
    /// <remarks>
    /// <para>This method is essentially the same as <see cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement})"/>,
    /// but it also passes an index to <paramref name="action"/>. The passed index is 0 for the first call and is incremented by 1
    /// for each subsequent call.</para>
    /// </remarks>
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement})"/>
    public static T ForEach<T, TElement>(this T @this, IEnumerable<TElement> sequence, FluentAction<T, TElement, int> action)
    {
        _ = Arg.NotNull(action);
        var index = 0;
        foreach (var item in Arg.NotNull(sequence).Value)
        {
            @this = action(@this, item, index++);
        }

        return @this;
    }
}

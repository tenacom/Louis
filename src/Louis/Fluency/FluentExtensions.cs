// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using CommunityToolkit.Diagnostics;

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
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="action"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Invoke<T>(this T @this, Action<T> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);

        action(@this);
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
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T If<T>(this T @this, bool condition, FluentAction<T> then)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);

        return condition ? then(@this) : @this;
    }

    /// <summary>
    /// Invokes one of two actions on an object according to a boolean condition, then returns the same object.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="condition">The condition to test.</param>
    /// <param name="then">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="true"/>.</param>
    /// <param name="else">The action to perform on <paramref name="this"/> if <paramref name="condition"/> is <see langword="false"/>.</param>
    /// <returns>A reference to <paramref name="this"/> after either <paramref name="then"/> or <paramref name="else"/> returns.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="then"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="else"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T IfElse<T>(this T @this, bool condition, FluentAction<T> then, FluentAction<T> @else)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(then);
        Guard.IsNotNull(@else);

        return condition ? then(@this) : @else(@this);
    }

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
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="cases"/> is <see langword="null"/>.</para>
    /// </exception>
    /// <remarks>
    /// <para>If <paramref name="value"/> is not equal to any of the values in <paramref name="cases"/>, this method returns immediately.</para>
    /// </remarks>
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
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="cases"/> is <see langword="null"/>.</para>
    /// </exception>
    public static T Switch<T, TValue>(this T @this, TValue value, FluentAction<T>? @default, params (TValue Comparand, FluentAction<T>? Action)[] cases)
        where TValue : IEquatable<TValue>
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(cases);

        foreach (var (comparand, action) in cases)
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
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="action"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="sequence"/> is <see langword="null"/>.</para>
    /// </exception>
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},FluentAction{T,TElement})"/>
    public static T ForEach<T, TElement>(this T @this, IEnumerable<TElement> sequence, FluentAction<T, TElement> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);
        Guard.IsNotNull(sequence);

        foreach (var item in sequence)
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
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="action"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="sequence"/> is <see langword="null"/>.</para>
    /// </exception>
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},FluentAction{T,TElement,int})"/>
    public static T ForEach<T, TElement>(this T @this, IEnumerable<TElement> sequence, FluentAction<T, TElement, int> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);
        Guard.IsNotNull(sequence);

        var index = 0;
        foreach (var item in sequence)
        {
            @this = action(@this, item, index++);
        }

        return @this;
    }

    /// <summary>
    /// Performs a specified action on an object and each element of a span,
    /// each time taking the result of the action as the object to pass together with the next element,
    /// then returns the result of the last action.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TElement">The type of each element of the span.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="span">The span of elements.</param>
    /// <param name="action">The action to perform.</param>
    /// <returns>The result of the last call to <paramref name="action"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="action"/> is <see langword="null"/>.</para>
    /// </exception>
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement})"/>
    public static T ForEach<T, TElement>(this T @this, ReadOnlySpan<TElement> span, FluentAction<T, TElement> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);

        foreach (var item in span)
        {
            @this = action(@this, item);
        }

        return @this;
    }

    /// <summary>
    /// Performs a specified action on an object and each element of a span,
    /// each time taking the result of the action as the object to pass together with the next element,
    /// then returns the result of the last action.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <typeparam name="TElement">The type of each element of the span.</typeparam>
    /// <param name="this">The object on which this method was called.</param>
    /// <param name="span">The span of elements.</param>
    /// <param name="action">The action to perform.</param>
    /// <returns>The result of the last call to <paramref name="action"/>.</returns>
    /// <remarks>
    /// <para>This method is essentially the same as <see cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},FluentAction{T,TElement})"/>,
    /// but it also passes the index of each element to <paramref name="action"/>.</para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <para><paramref name="this"/> is <see langword="null"/>.</para>
    /// <para>- or -</para>
    /// <para><paramref name="action"/> is <see langword="null"/>.</para>
    /// </exception>
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement,int})"/>
    public static T ForEach<T, TElement>(this T @this, ReadOnlySpan<TElement> span, FluentAction<T, TElement, int> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);

        for (var i = 0; i < span.Length; i++)
        {
            @this = action(@this, span[i], i);
        }

        return @this;
    }
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using CommunityToolkit.Diagnostics;

namespace Louis.Fluency;

partial class FluentExtensions
{
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
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},Action{T,TElement})"/>
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},FluentAction{T,TElement})"/>
    public static T ForEach<T, TElement>(this T @this, IEnumerable<TElement> sequence, FluentAction<T, TElement> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);
        Guard.IsNotNull(sequence);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        foreach (var item in sequence)
        {
            @this = action(@this, item);
        }
#pragma warning restore CA1062 // Validate arguments of public methods

        return @this;
    }

    /// <summary>
    /// Performs a specified action on an object and each element of a sequence,
    /// then returns the object.
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
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement})"/>
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},Action{T,TElement})"/>
    public static T ForEach<T, TElement>(this T @this, IEnumerable<TElement> sequence, Action<T, TElement> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);
        Guard.IsNotNull(sequence);

#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        foreach (var item in sequence)
        {
            action(@this, item);
        }
#pragma warning restore CA1062 // Validate arguments of public methods

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
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},Action{T,TElement,int})"/>
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},FluentAction{T,TElement,int})"/>
    public static T ForEach<T, TElement>(this T @this, IEnumerable<TElement> sequence, FluentAction<T, TElement, int> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);
        Guard.IsNotNull(sequence);

        var index = 0;
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        foreach (var item in sequence)
        {
            @this = action(@this, item, index++);
        }
#pragma warning restore CA1062 // Validate arguments of public methods

        return @this;
    }

    /// <summary>
    /// Performs a specified action on an object and each element of a sequence,
    /// then returns the object.
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
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement,int})"/>
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},Action{T,TElement,int})"/>
    public static T ForEach<T, TElement>(this T @this, IEnumerable<TElement> sequence, Action<T, TElement, int> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);
        Guard.IsNotNull(sequence);

        var index = 0;
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
        foreach (var item in sequence)
        {
            action(@this, item, index++);
        }
#pragma warning restore CA1062 // Validate arguments of public methods

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
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},Action{T,TElement})"/>
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement})"/>
    public static T ForEach<T, TElement>(this T @this, ReadOnlySpan<TElement> span, FluentAction<T, TElement> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);

        foreach (var item in span)
        {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
            @this = action(@this, item);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        return @this;
    }

    /// <summary>
    /// Performs a specified action on an object and each element of a span,
    /// then returns the object.
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
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},FluentAction{T,TElement})"/>
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},Action{T,TElement})"/>
    public static T ForEach<T, TElement>(this T @this, ReadOnlySpan<TElement> span, Action<T, TElement> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);

        foreach (var item in span)
        {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
            action(@this, item);
#pragma warning restore CA1062 // Validate arguments of public methods
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
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},Action{T,TElement,int})"/>
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},FluentAction{T,TElement,int})"/>
    public static T ForEach<T, TElement>(this T @this, ReadOnlySpan<TElement> span, FluentAction<T, TElement, int> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);

        for (var i = 0; i < span.Length; i++)
        {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
            @this = action(@this, span[i], i);
#pragma warning restore CA1062 // Validate arguments of public methods
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
    /// <seealso cref="ForEach{T,TElement}(T,ReadOnlySpan{TElement},FluentAction{T,TElement,int})"/>
    /// <seealso cref="ForEach{T,TElement}(T,IEnumerable{TElement},Action{T,TElement,int})"/>
    public static T ForEach<T, TElement>(this T @this, ReadOnlySpan<TElement> span, Action<T, TElement, int> action)
    {
        Guard.IsNotNull(@this);
        Guard.IsNotNull(action);

        for (var i = 0; i < span.Length; i++)
        {
#pragma warning disable CA1062 // Validate arguments of public methods - False positive, see https://github.com/CommunityToolkit/dotnet/issues/843
            action(@this, span[i], i);
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        return @this;
    }
}

// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace Louis.Diagnostics;

partial class ExceptionExtensions
{
    /// <summary>
    /// Rethrows an exception, preserving the original stack trace. This method never returns.
    /// </summary>
    /// <param name="this">The exception to rethrow.</param>
    [DoesNotReturn]
#if NETSTANDARD2_0 || NETFRAMEWORK
#pragma warning disable CS8763 // A method marked [DoesNotReturn] should not return - ExceptionDispatchInfo.Throw does not return but is not marked [DoesNotReturn].
    public static void Rethrow(this Exception @this) => ExceptionDispatchInfo.Capture(@this).Throw();
#pragma warning restore CS8763 // A method marked [DoesNotReturn] should not return
#else
    public static void Rethrow(this Exception @this) => ExceptionDispatchInfo.Throw(@this);
#endif
}

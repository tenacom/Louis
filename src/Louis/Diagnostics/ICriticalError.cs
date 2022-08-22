// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

namespace Louis.Diagnostics;

/// <summary>
/// Identifies an exception as critical. Critical exceptions indicate an unrecoverable error
/// that should terminate an application immediately; therefore they should never be caught.
/// </summary>
/// <remarks>
/// <para>Exceptions implementing this interface will be recognized as critical by the
/// <see cref="ExceptionExtensions.IsCriticalError"/> method.</para>
/// <para>While using an empty interface as a marker
/// <see href="https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1040#rule-description">is usually considered bad practice</see>,
/// in this case it is the best alternative, because:</para>
/// <list type="bullet">
/// <item><description>an attribute to mark exception classes as critical would make checking a lot more expensive,
/// as the type of every exception (and of its causing exceptions) should be checked for the presence of the attribute;</description></item>
/// <item><description>a <c>CriticalException</c> base class would be straightforward to check for, but would make it hard
/// (if at all possible) to mark exceptions in existing code bases as critical, as they could be part of existing class hierarchies
/// that have to be preserved.</description></item>
/// </list>
/// </remarks>
#pragma warning disable CA1040 // Avoid empty interfaces - See XML docs above.
public interface ICriticalError
#pragma warning restore CA1040 // Avoid empty interfaces
{
}

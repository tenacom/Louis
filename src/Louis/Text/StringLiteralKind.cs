// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

namespace Louis.Text;

/// <summary>
/// Represents the kind of string literal produced by <c>ToLiteral</c> methods.
/// </summary>
/// <seealso cref="StringExtensions.ToLiteral"/>
/// <seealso cref="CharReadOnlySpanExtensions.ToLiteral"/>
public enum StringLiteralKind
{
    /// <summary>
    /// Represents a <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#quoted-string-literals">quoted string literal</see>.
    /// </summary>
    Quoted,

    /// <summary>
    /// Represents a <see href="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#verbatim-string-literals">verbatim string literal</see>.
    /// </summary>
    Verbatim,
}

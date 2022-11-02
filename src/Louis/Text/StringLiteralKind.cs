// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

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

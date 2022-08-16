// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

namespace Louis.Diagnostics;

/// <summary>
/// Provides helper methods to throw commonly-used exceptions efficiently.
/// </summary>
/// <remarks>
/// <para>The name of this class is also a Visual Basic keyword.
/// While this has no consequences when using this class in C# code,
/// Visual Basic users will have to surround the class name with square brackets,
/// like this:</para>
/// <code lang="VB">[Throw].InvalidOperation(message)</code>
/// </remarks>
#pragma warning disable CA1716 // Identifiers should not match keywords - Throw makes sense here; VB code can use [Throw]
public static partial class Throw
#pragma warning restore CA1716 // Identifiers should not match keywords
{
}

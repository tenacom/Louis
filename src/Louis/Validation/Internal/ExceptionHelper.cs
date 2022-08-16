// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. All rights reserved.
// Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using Louis.Diagnostics;

namespace Louis.ArgumentValidation.Internal;

internal static class ExceptionHelper
{
    public static Exception PredicateCannotBeNull() => SelfCheck.Failure("Predicate cannot be null.");

    public static Exception CallbackCannotBeNull() => SelfCheck.Failure("Callback cannot be null.");
}

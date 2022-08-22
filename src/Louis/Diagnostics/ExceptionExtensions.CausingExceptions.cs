// ---------------------------------------------------------------------------------------
// Copyright (C) Tenacom and L.o.U.I.S. contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.
//
// Part of this file may be third-party code, distributed under a compatible license.
// See the THIRD-PARTY-NOTICES file in the project root for third-party copyright notices.
// ---------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;

namespace Louis.Diagnostics;

partial class ExceptionExtensions
{
    private class CausingExceptions : IEnumerable<Exception>
    {
        private readonly Exception _exception;

        public CausingExceptions(Exception exception)
        {
            _exception = exception;
        }

        public IEnumerator<Exception> GetEnumerator() => GetCausingExceptionsCore(_exception);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Louis.Diagnostics;

partial class ExceptionExtensions
{
    private sealed class CausingExceptions : IEnumerable<Exception>
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

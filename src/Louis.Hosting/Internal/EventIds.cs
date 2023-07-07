// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Louis.Hosting.Internal;

internal static class EventIds
{
    public static class AsyncHostedService
    {
        public const int StateChanged = 0;
        public const int SetupCanceled = 1;
        public const int SetupFailed = 2;
        public const int ExecuteCanceled = 3;
        public const int ExecuteFailed = 4;
        public const int TeardownFailed = 5;
        public const int StopRequested = 6;
    }
}

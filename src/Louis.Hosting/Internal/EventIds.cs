// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Louis.Hosting.Internal;

internal static class EventIds
{
    public static class AsyncHostedService
    {
        public const int StateChanged = 0;
        public const int BeforeSetup = 1;
        public const int SetupCompleted = 2;
        public const int SetupCanceled = 3;
        public const int SetupFailed = 4;
        public const int BeforeExecute = 5;
        public const int ExecuteCompleted = 6;
        public const int ExecuteCanceled = 7;
        public const int ExecuteFailed = 8;
        public const int BeforeTeardown = 9;
        public const int TeardownCompleted = 10;
        public const int TeardownFailed = 11;
        public const int StopRequested = 12;
        public const int HostedServiceStarting = 13;
        public const int HostedServiceStopping = 14;
    }
}

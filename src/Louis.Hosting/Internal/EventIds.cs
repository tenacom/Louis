// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Louis.Hosting.Internal;

internal static class EventIds
{
    public static class AsyncHostedService
    {
        public const int StateChanged = 1;
        public const int BeforeSetup = 2;
        public const int SetupCompleted = 3;
        public const int SetupCanceled = 4;
        public const int SetupFailed = 5;
        public const int BeforeExecute = 6;
        public const int ExecuteCompleted = 7;
        public const int ExecuteCanceled = 8;
        public const int ExecuteFailed = 9;
        public const int BeforeTeardown = 10;
        public const int TeardownCompleted = 11;
        public const int TeardownFailed = 12;
        public const int StopRequested = 13;
        public const int HostedServiceStarting = 14;
        public const int HostedServiceStopping = 15;
    }
}

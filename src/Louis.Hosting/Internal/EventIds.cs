// Copyright (c) Tenacom and contributors. Licensed under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Louis.Hosting.Internal;

internal static class EventIds
{
    public static class AsyncHostedService
    {
        public const int StateChanged = 1;
        public const int BeforeSetup = 2;
        public const int SetupSuccessful = 3;
        public const int SetupNotSuccessful = 4;
        public const int SetupCanceled = 5;
        public const int SetupFailed = 6;
        public const int BeforeExecute = 7;
        public const int ExecuteCompleted = 8;
        public const int ExecuteCanceled = 9;
        public const int ExecuteFailed = 10;
        public const int BeforeTeardown = 11;
        public const int TeardownCompleted = 12;
        public const int TeardownFailed = 13;
        public const int StopRequested = 14;
        public const int HostedServiceStarting = 15;
        public const int HostedServiceStopping = 16;
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;

namespace XFS4IoTServer
{
    /// <summary>
    /// Exception thrown on Timeout or Cancel requested.
    /// </summary>
    public class TimeoutCanceledException : OperationCanceledException
    {

        public TimeoutCanceledException(string message, Exception innerException, bool isCancelRequested)
            : base(message, innerException)
        {
            IsCancelRequested = isCancelRequested;
        }

        public TimeoutCanceledException(bool isCancelRequested)
        {
            IsCancelRequested = isCancelRequested;
        }

        public TimeoutCanceledException()
        {
            IsCancelRequested = false;
        }

        public bool IsCancelRequested { get; init; }

    }
}

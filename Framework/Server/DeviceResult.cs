/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using XFS4IoT;
using XFS4IoT.Completions;

namespace XFS4IoTServer
{
    /// <summary>
    /// Common base class for the result of an operation processed by the device specific class
    /// </summary>
    public class DeviceResult
    {
        [Obsolete("This constructor is obsolete, use constructor has a first parameter MessageHeader." +
            "CompletionCodeEnum. This class will not be supported in the package version 3.0. " +
            "Please migrate changes in the device class before applying 3.0 package.", false)]
        public DeviceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                            string ErrorDescription = null)
        {
            this.CompletionCode = (MessageHeader.CompletionCodeEnum)CompletionCode;
            this.ErrorDescription = ErrorDescription;
        }

        public DeviceResult(MessageHeader.CompletionCodeEnum CompletionCode,
                            string ErrorDescription = null)
        {
            this.CompletionCode = CompletionCode;
            this.ErrorDescription = ErrorDescription;
        }

        public MessageHeader.CompletionCodeEnum CompletionCode;

        public string ErrorDescription = null;
    }

}

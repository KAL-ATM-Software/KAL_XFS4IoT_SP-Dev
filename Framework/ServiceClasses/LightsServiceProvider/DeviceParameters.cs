/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/

using System;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Lights.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Lights
{
    public sealed class SetLightRequest
    {
        public SetLightRequest(string UnitName, LightsStatusClass.LightOperation Operation)
        {
            this.Operation = Operation;
        }

        /// <summary>
        /// Name of device unit to change light state
        /// </summary>
        public string UnitName;

        /// <summary>
        /// Parameters to change light
        /// </summary>
        public LightsStatusClass.LightOperation Operation;
    }

    public sealed class SetLightResult : DeviceResult
    {
        public SetLightResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              SetLightCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public SetLightCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }
}

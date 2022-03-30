/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/

using System;
using System.Collections.Generic;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Lights.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Lights
{
    public sealed class SetLightRequest
    {
        public SetLightRequest(Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsStatusClass.LightOperation> StdLights,
                               Dictionary<string, LightsStatusClass.LightOperation> CustomLights)
        {
            this.StdLights = StdLights;
            this.CustomLights = CustomLights;
        }

        /// <summary>
        /// standard name of device unit to change lights
        /// </summary>
        public Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsStatusClass.LightOperation> StdLights;

        /// <summary>
        /// Vendor specific device unit name to change lights
        /// </summary>
        public Dictionary<string, LightsStatusClass.LightOperation> CustomLights;
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

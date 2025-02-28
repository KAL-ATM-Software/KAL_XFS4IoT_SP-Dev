/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/
using System;
using System.Collections.Generic;
using XFS4IoTServer;
using XFS4IoT;
using XFS4IoT.Completions;
using XFS4IoT.Lights.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Lights
{
    public sealed class SetLightRequest
    {
        [Obsolete("No longer used since package version 3. This constructor will be removed after package version 4.")]
        public SetLightRequest(Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsStatusClass.LightOperation> StdLights,
                               Dictionary<string, LightsStatusClass.LightOperation> CustomLights)
        {
            StdLightOperations = null;
            CustomLightOperations = null;
        }

        public SetLightRequest(Dictionary<LightsCapabilitiesClass.DeviceEnum, List<LightsStatusClass.LightOperation>> StdLightOperations,
                               Dictionary<string, List<LightsStatusClass.LightOperation>> CustomLightOperations)
        {
            this.StdLightOperations = StdLightOperations;
            this.CustomLightOperations = CustomLightOperations;
        }

        /// <summary>
        /// standard name of device unit to change lights
        /// </summary>
        [Obsolete("No longer used since package version 3. This property will be removed after package version 4.")]
        public Dictionary<LightsCapabilitiesClass.DeviceEnum, LightsStatusClass.LightOperation> StdLights;

        /// <summary>
        /// Vendor specific device unit name to change lights
        /// </summary>
        [Obsolete("No longer used since package version 3. This property will be removed after package version 4.")]
        public Dictionary<string, LightsStatusClass.LightOperation> CustomLights;

        /// <summary>
        /// Standard name of device unit to change lights, it covers multiple positions of one named device unit.
        /// </summary>
        public Dictionary<LightsCapabilitiesClass.DeviceEnum, List<LightsStatusClass.LightOperation>> StdLightOperations;

        /// <summary>
        /// Vendor specific device unit to change lights, it covers multiple positions of one named device unit.
        /// </summary>
        public Dictionary<string, List<LightsStatusClass.LightOperation>> CustomLightOperations;
    }

    public sealed class SetLightResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        SetLightCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) 
        : DeviceResult(CompletionCode, ErrorDescription)
    {
        public SetLightCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }
}

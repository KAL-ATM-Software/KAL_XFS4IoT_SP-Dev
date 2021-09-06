/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * GetQueryPCIPTSDeviceId_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.PinPad.Completions
{
    [DataContract]
    [Completion(Name = "PinPad.GetQueryPCIPTSDeviceId")]
    public sealed class GetQueryPCIPTSDeviceIdCompletion : Completion<GetQueryPCIPTSDeviceIdCompletion.PayloadData>
    {
        public GetQueryPCIPTSDeviceIdCompletion(int RequestId, GetQueryPCIPTSDeviceIdCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string ManufacturerIdentifier = null, string ModelIdentifier = null, string HardwareIdentifier = null, string FirmwareIdentifier = null, string ApplicationIdentifier = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ManufacturerIdentifier = ManufacturerIdentifier;
                this.ModelIdentifier = ModelIdentifier;
                this.HardwareIdentifier = HardwareIdentifier;
                this.FirmwareIdentifier = FirmwareIdentifier;
                this.ApplicationIdentifier = ApplicationIdentifier;
            }

            /// <summary>
            /// Returns the manufacturer identifier of the PIN device.
            /// This value is not set if the manufacturer identifier is not available. 
            /// This property is distinct from the HSM key pair that may be reported in the extra property by the 
            /// [Capabilities](#common.capabilities.completion.properties.pinpad) command.
            /// </summary>
            [DataMember(Name = "manufacturerIdentifier")]
            public string ManufacturerIdentifier { get; init; }

            /// <summary>
            /// Returns the model identifier of the PIN device. 
            /// This value is not set if the model identifier is not available.
            /// </summary>
            [DataMember(Name = "modelIdentifier")]
            public string ModelIdentifier { get; init; }

            /// <summary>
            /// Returns the hardware identifier of the PIN device. 
            /// This value is not set if the hardware identifier is not available.
            /// </summary>
            [DataMember(Name = "hardwareIdentifier")]
            public string HardwareIdentifier { get; init; }

            /// <summary>
            /// Returns the firmware identifier of the PIN device. 
            /// This value is not set if the firmware identifier is not available.
            /// </summary>
            [DataMember(Name = "firmwareIdentifier")]
            public string FirmwareIdentifier { get; init; }

            /// <summary>
            /// Returns the application identifier of the PIN device.
            /// This value is not set if the application identifier is not available.
            /// </summary>
            [DataMember(Name = "applicationIdentifier")]
            public string ApplicationIdentifier { get; init; }

        }
    }
}

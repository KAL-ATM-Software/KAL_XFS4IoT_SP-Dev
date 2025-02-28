/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT IntelligentBanknoteNeutralization interface.
 * SetProtection_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.IntelligentBanknoteNeutralization.Completions
{
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Completion(Name = "IntelligentBanknoteNeutralization.SetProtection")]
    public sealed class SetProtectionCompletion : Completion<SetProtectionCompletion.PayloadData>
    {
        public SetProtectionCompletion(int RequestId, SetProtectionCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                SensorNotReady
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```sensorNotReady``` - The protection cannot be changed if the banknote neutralization is not in the appropriate state. For example, to arm it, the ATM safe door must be closed and locked, otherwise this error will be returned.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}

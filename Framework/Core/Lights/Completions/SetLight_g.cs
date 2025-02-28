/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Lights interface.
 * SetLight_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Lights.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Lights.SetLight")]
    public sealed class SetLightCompletion : Completion<SetLightCompletion.PayloadData>
    {
        public SetLightCompletion(int RequestId, SetLightCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                InvalidLight,
                LightError
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```invalidLight``` - An attempt to set a light to a new value was invalid because the light does not exist.
            /// * ```lightError``` - A hardware error occurred while executing the command.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}

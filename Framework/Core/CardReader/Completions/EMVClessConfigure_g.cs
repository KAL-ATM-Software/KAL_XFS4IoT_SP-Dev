/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessConfigure_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.EMVClessConfigure")]
    public sealed class EMVClessConfigureCompletion : Completion<EMVClessConfigureCompletion.PayloadData>
    {
        public EMVClessConfigureCompletion(int RequestId, EMVClessConfigureCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
            }

            public enum ErrorCodeEnum
            {
                InvalidTerminalData,
                InvalidAidData,
                InvalidKeyData
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```invalidTerminalData``` - Input data
            ///   [terminalData](#cardreader.emvclessconfigure.command.properties.terminaldata) was invalid.
            ///   Contactless chip card reader could not be configured successfully.
            /// * ```invalidAidData``` - Input data
            ///   [aidData](#cardreader.emvclessconfigure.command.properties.aiddata) was invalid. Contactless chip
            ///   card reader could not be configured successfully.
            /// * ```invalidKeyData``` - Input data
            ///   [keyData](#cardreader.emvclessconfigure.command.properties.keydata) was invalid. Contactless chip
            ///   card reader could not be configured successfully.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}

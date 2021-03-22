/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessConfigure_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
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
        public EMVClessConfigureCompletion(string RequestId, EMVClessConfigureCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                InvalidTerminalData,
                InvalidAidData,
                InvalidKeyData,
            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(EMVClessConfigureCompletion.PayloadData)}");

                this.ErrorCode = ErrorCode;
            }

            /// <summary>
            ///Specifies the error code if applicable. The following values are possible:**invalidTerminalData**
            ////Input data *terminalData* was invalid. Contactless chip card reader could not be configured successfully.**invalidAidData**
            ////Input data *aidData* was invalid. Contactless chip card reader could not be configured successfully.**invalidKeyData**
            ////Input data *keyData* was invalid. Contactless chip card reader could not be configured successfully.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}

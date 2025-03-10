/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * SetMatch_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Biometric.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Biometric.SetMatch")]
    public sealed class SetMatchCompletion : Completion<SetMatchCompletion.PayloadData>
    {
        public SetMatchCompletion(int RequestId, SetMatchCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
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
                InvalidIdentifier,
                ModeNotSupported,
                NoImportedData,
                InvalidThreshold
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```invalidIdentifier``` - The command failed because data was imported but *identifier* was not found.
            /// * ```modeNotSupported``` - The type of match specified in *compareMode* is not supported.
            /// * ```noImportedData``` - The command failed because no data was imported previously using the
            ///                           [Biometric.ImportData](#biometric.import).
            /// * ```invalidThreshold``` - The *threshold* input parameter is greater than the maximum allowed of 100.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}

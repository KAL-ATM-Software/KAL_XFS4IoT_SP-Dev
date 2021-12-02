/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * SetAuxiliaries_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Auxiliaries.Completions
{
    [DataContract]
    [Completion(Name = "Auxiliaries.SetAuxiliaries")]
    public sealed class SetAuxiliariesCompletion : Completion<SetAuxiliariesCompletion.PayloadData>
    {
        public SetAuxiliariesCompletion(int RequestId, SetAuxiliariesCompletion.PayloadData Payload)
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
                InvalidAuxiliary,
                SyntaxError
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```invalidAuxiliary``` - An attempt to set a auxiliary to a new value was invalid because the auxiliary does not 
            ///   exist or the auxiliary is pre-configured as an input port.
            /// * ```syntaxError``` - The command was invoked with incorrect input data.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

        }
    }
}

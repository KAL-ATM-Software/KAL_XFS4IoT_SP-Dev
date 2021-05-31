/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * Read_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [Completion(Name = "TextTerminal.Read")]
    public sealed class ReadCompletion : Completion<ReadCompletion.PayloadData>
    {
        public ReadCompletion(int RequestId, ReadCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string Input = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Input = Input;
            }

            public enum ErrorCodeEnum
            {
                KeyInvalid,
                KeyNotSupported,
                NoActiveKeys
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```keyInvalid``` - At least one of the specified keys is invalid.
            /// * ```keyNotSupported``` - At least one of the specified keys is not supported by the Service Provider.
            /// * ```noActiveKeys``` - There are no active keys specified.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

            /// <summary>
            /// Specifies a zero terminated string containing all the printable characters (numeric and alphanumeric) 
            /// read from the text terminal unit key pad.
            /// </summary>
            [DataMember(Name = "input")]
            public string Input { get; private set; }

        }
    }
}

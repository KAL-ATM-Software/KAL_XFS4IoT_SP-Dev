/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "TextTerminal.Read")]
    public sealed class ReadCompletion : Completion<ReadCompletion.PayloadData>
    {
        public ReadCompletion(int RequestId, ReadCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, string Input = null)
                : base()
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
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```keyInvalid``` - At least one of the specified keys is invalid.
            /// * ```keyNotSupported``` - At least one of the specified keys is not supported by the Service.
            /// * ```noActiveKeys``` - There are no active keys specified.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Specifies a string containing all the printable characters (numeric and alphanumeric)
            /// read from the text terminal unit key pad. This property is null if no characters are read.
            /// <example>12345</example>
            /// </summary>
            [DataMember(Name = "input")]
            public string Input { get; init; }

        }
    }
}

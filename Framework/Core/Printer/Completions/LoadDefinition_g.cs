/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * LoadDefinition_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.LoadDefinition")]
    public sealed class LoadDefinitionCompletion : Completion<LoadDefinitionCompletion.PayloadData>
    {
        public LoadDefinitionCompletion(int RequestId, LoadDefinitionCompletion.PayloadData Payload)
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
                FileNotFound,
                FormInvalid,
                MediaInvalid,
                DefinitionExists
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```fileNotFound``` - The specified file cannot be found.
            /// * ```formInvalid``` - The form is invalid.
            /// * ```mediaInvalid``` - The media definition is invalid.
            /// * ```definitionExists``` - The specified form or media definition already exists and the *overwrite*
            ///   flag was false.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

        }
    }
}

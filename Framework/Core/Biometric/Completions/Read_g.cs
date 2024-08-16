/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * Read_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Biometric.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Biometric.Read")]
    public sealed class ReadCompletion : Completion<ReadCompletion.PayloadData>
    {
        public ReadCompletion(int RequestId, ReadCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, List<BioDataClass> DataRead = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.DataRead = DataRead;
            }

            public enum ErrorCodeEnum
            {
                ReadFailed,
                ModeNotSupported,
                FormatNotSupported,
                KeyNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```readFailed``` -\tModule was unable to complete the scan operation.
            /// * ```modeNotSupported``` -\t*mode* is not supported.
            /// * ```formatNotSupported``` - The format specified is valid but not supported.
            ///                         A list of the supported values can be obtained through the [dataFormats](#common.capabilities.completion.properties.biometric.dataformats).
            /// * ```keyNotFound``` -\tThe specified key name is not found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// This property is used to indicate the biometric data type of the template data contained.
            /// This property is not required if *dataTypes* property is null.
            /// </summary>
            [DataMember(Name = "dataRead")]
            public List<BioDataClass> DataRead { get; init; }

        }
    }
}

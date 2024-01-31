/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * GetStorageInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Biometric.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Biometric.GetStorageInfo")]
    public sealed class GetStorageInfoCompletion : Completion<GetStorageInfoCompletion.PayloadData>
    {
        public GetStorageInfoCompletion(int RequestId, GetStorageInfoCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, Dictionary<string, DataTypeClass> Templates = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Templates = Templates;
            }

            public enum ErrorCodeEnum
            {
                NoImportedData
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```noImportedData``` -\tNo data to return. Typically means that no data has been imported using the 
            ///                           [Biometric.Import](#biometric.import).
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// A list of biometric templates that were successfully imported.
            /// The object name of each biometric data type can be used in the *identifier* property for the
            /// [Biometric.Match](#biometric.match) command.
            /// If no template data was imported, this property is null.
            /// </summary>
            [DataMember(Name = "templates")]
            public Dictionary<string, DataTypeClass> Templates { get; init; }

        }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * Import_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Biometric.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Biometric.Import")]
    public sealed class ImportCompletion : Completion<ImportCompletion.PayloadData>
    {
        public ImportCompletion()
            : base()
        { }

        public ImportCompletion(int RequestId, ImportCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, DataTypeClass> Templates = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Templates = Templates;
            }

            public enum ErrorCodeEnum
            {
                InvalidData,
                FormatNotSupported,
                CapacityExceeded,
                KeyNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```invalidData``` - The data that was imported was malformed or invalid. No data has been imported into the device.
            ///                       The presence of any previously loaded templates can be checked for using the [Biometric.Read](#biometric.getstorageinfo).
            /// * ```formatNotSupported``` - The format of the biometric data that was specified is not supported.
            ///                         No data has been imported into the device.
            ///                         A list of the supported values can be obtained through the [dataFormats](#common.capabilities.completion.properties.biometric.dataformats).
            /// * ```capacityExceeded``` - An attempt has been made to import more templates than the maximum reserved storage space available.
            ///                             The maximum storage space available is reported in the capability [templateStorage](#common.capabilities.completion.properties.biometric.templatestorage). No data has been
            ///                             imported into the device.
            ///                             The amount of storage remaining is reported in the [remainingStorage](#common.status.completion.properties.biometric.remainingstorage).
            /// * ```keyNotFound``` - The specified key name is not found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// A list of the biometric template data that were successfully imported. If there are no template data imported, this property can be null.
            /// </summary>
            [DataMember(Name = "templates")]
            public Dictionary<string, DataTypeClass> Templates { get; init; }

        }
    }
}

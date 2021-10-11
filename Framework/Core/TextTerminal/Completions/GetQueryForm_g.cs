/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetQueryForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [Completion(Name = "TextTerminal.GetQueryForm")]
    public sealed class GetQueryFormCompletion : Completion<GetQueryFormCompletion.PayloadData>
    {
        public GetQueryFormCompletion(int RequestId, GetQueryFormCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string FormName = null, string Width = null, string Height = null, string VersionMajor = null, string VersionMinor = null, List<string> Fields = null, string LanguageId = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.FormName = FormName;
                this.Width = Width;
                this.Height = Height;
                this.VersionMajor = VersionMajor;
                this.VersionMinor = VersionMinor;
                this.Fields = Fields;
                this.LanguageId = LanguageId;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FormInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```formNotFound``` - The specified form cannot be found.
            /// * ```formInvalid``` - The specified form is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Specifies the name of the form.
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// Specifies the width of the form in columns.
            /// </summary>
            [DataMember(Name = "width")]
            public string Width { get; init; }

            /// <summary>
            /// Specifies the height of the form in rows. 
            /// </summary>
            [DataMember(Name = "height")]
            public string Height { get; init; }

            /// <summary>
            /// Specifies the major version.If version is not specifies in the form then zero is returned.
            /// </summary>
            [DataMember(Name = "versionMajor")]
            public string VersionMajor { get; init; }

            /// <summary>
            /// Specifies the minor version. 
            /// If the version is not specified in the form then zero is returned.
            /// </summary>
            [DataMember(Name = "versionMinor")]
            public string VersionMinor { get; init; }

            /// <summary>
            /// Object to a list of the field names.
            /// </summary>
            [DataMember(Name = "fields")]
            public List<string> Fields { get; init; }

            /// <summary>
            /// Specifies the language identifier for the form.
            /// </summary>
            [DataMember(Name = "languageId")]
            public string LanguageId { get; init; }

        }
    }
}

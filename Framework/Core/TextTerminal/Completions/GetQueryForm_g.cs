/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "TextTerminal.GetQueryForm")]
    public sealed class GetQueryFormCompletion : Completion<GetQueryFormCompletion.PayloadData>
    {
        public GetQueryFormCompletion(int RequestId, GetQueryFormCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, string FormName = null, int? Width = null, int? Height = null, int? VersionMajor = null, int? VersionMinor = null, List<string> Fields = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.FormName = FormName;
                this.Width = Width;
                this.Height = Height;
                this.VersionMajor = VersionMajor;
                this.VersionMinor = VersionMinor;
                this.Fields = Fields;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FormInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// * ```formNotFound``` - The specified form cannot be found.
            /// * ```formInvalid``` - The specified form is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Specifies the name of the form. This property is null if the form is not loaded.
            /// <example>Example form</example>
            /// </summary>
            [DataMember(Name = "formName")]
            public string FormName { get; init; }

            /// <summary>
            /// Specifies the width of the form in columns.
            /// </summary>
            [DataMember(Name = "width")]
            [DataTypes(Minimum = 0)]
            public int? Width { get; init; }

            /// <summary>
            /// Specifies the height of the form in rows.
            /// </summary>
            [DataMember(Name = "height")]
            [DataTypes(Minimum = 0)]
            public int? Height { get; init; }

            /// <summary>
            /// Specifies the major version.
            /// </summary>
            [DataMember(Name = "versionMajor")]
            [DataTypes(Minimum = 0)]
            public int? VersionMajor { get; init; }

            /// <summary>
            /// Specifies the minor version.
            /// </summary>
            [DataMember(Name = "versionMinor")]
            [DataTypes(Minimum = 0)]
            public int? VersionMinor { get; init; }

            /// <summary>
            /// A list of the field names. This property is null if the specified form is not loaded or no fields were defined.
            /// <example>["Field1", "Field2"]</example>
            /// </summary>
            [DataMember(Name = "fields")]
            public List<string> Fields { get; init; }

        }
    }
}

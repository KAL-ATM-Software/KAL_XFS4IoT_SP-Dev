/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetQueryField_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [Completion(Name = "TextTerminal.GetQueryField")]
    public sealed class GetQueryFieldCompletion : Completion<GetQueryFieldCompletion.PayloadData>
    {
        public GetQueryFieldCompletion(int RequestId, GetQueryFieldCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<FieldsClass> Fields = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Fields = Fields;
            }

            [DataContract]
            public sealed class FieldsClass
            {
                public FieldsClass(string FieldName = null, TypeEnum? Type = null, ClassEnum? Class = null, AccessClass Access = null, OverflowEnum? Overflow = null, string Format = null, string LanguageId = null)
                {
                    this.FieldName = FieldName;
                    this.Type = Type;
                    this.Class = Class;
                    this.Access = Access;
                    this.Overflow = Overflow;
                    this.Format = Format;
                    this.LanguageId = LanguageId;
                }

                /// <summary>
                /// Specifies the field name.
                /// </summary>
                [DataMember(Name = "fieldName")]
                public string FieldName { get; private set; }

                public enum TypeEnum
                {
                    Text,
                    Invisible,
                    Password
                }

                /// <summary>
                /// Specifies the type of field.
                /// </summary>
                [DataMember(Name = "type")]
                public TypeEnum? Type { get; private set; }

                public enum ClassEnum
                {
                    Static,
                    Optional,
                    Required
                }

                /// <summary>
                /// Specifies the class of the field.
                /// </summary>
                [DataMember(Name = "class")]
                public ClassEnum? Class { get; private set; }

                [DataContract]
                public sealed class AccessClass
                {
                    public AccessClass(string Read = null, string Write = null)
                    {
                        this.Read = Read;
                        this.Write = Write;
                    }

                    /// <summary>
                    /// The Field is used for input from the physical device.
                    /// </summary>
                    [DataMember(Name = "read")]
                    public string Read { get; private set; }

                    /// <summary>
                    /// The Field is used for output to the physical device.
                    /// </summary>
                    [DataMember(Name = "write")]
                    public string Write { get; private set; }

                }

                /// <summary>
                /// Specifies whether the field is to be used for input, output or both.
                /// </summary>
                [DataMember(Name = "access")]
                public AccessClass Access { get; private set; }

                public enum OverflowEnum
                {
                    Terminate,
                    Truncate,
                    Overwrite
                }

                /// <summary>
                /// Specifies how an overflow of field data should be handled.
                /// </summary>
                [DataMember(Name = "overflow")]
                public OverflowEnum? Overflow { get; private set; }

                /// <summary>
                /// Format string as defined in the form for this field.
                /// </summary>
                [DataMember(Name = "format")]
                public string Format { get; private set; }

                /// <summary>
                /// Specifies the language identifier for the field.
                /// </summary>
                [DataMember(Name = "languageId")]
                public string LanguageId { get; private set; }

            }

            /// <summary>
            /// Array of Fields.
            /// </summary>
            [DataMember(Name = "fields")]
            public List<FieldsClass> Fields { get; private set; }

        }
    }
}

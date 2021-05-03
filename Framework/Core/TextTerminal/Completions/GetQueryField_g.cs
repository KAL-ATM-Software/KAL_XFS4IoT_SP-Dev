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
        public GetQueryFieldCompletion(string RequestId, GetQueryFieldCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            [DataContract]
            public sealed class FieldsClass
            {
                /// <summary>
                /// Specifies the type of field.
                /// </summary>
                public enum TypeEnum
                {
                    Text,
                    Invisible,
                    Password,
                }

                /// <summary>
                /// Specifies the class of the field.
                /// </summary>
                public enum ClassEnum
                {
                    Static,
                    Optional,
                    Required,
                }

                /// <summary>
                /// Specifies how an overflow of field data should be handled.
                /// </summary>
                public enum OverflowEnum
                {
                    Terminate,
                    Truncate,
                    Overwrite,
                }

                public FieldsClass(string FieldName = null, TypeEnum? Type = null, ClassEnum? Class = null, string Access = null, OverflowEnum? Overflow = null, string Format = null, string LanguageId = null)
                    : base()
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

                /// <summary>
                /// Specifies the type of field.
                /// </summary>
                [DataMember(Name = "type")] 
                public TypeEnum? Type { get; private set; }

                /// <summary>
                /// Specifies the class of the field.
                /// </summary>
                [DataMember(Name = "class")] 
                public ClassEnum? Class { get; private set; }

                /// <summary>
                /// Specifies whether the field is to be used for input, output or both.
                /// </summary>
                [DataMember(Name = "access")] 
                public string Access { get; private set; }

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


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<FieldsClass> Fields = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Fields = Fields;
            }

            /// <summary>
            /// Array of Fields.
            /// </summary>
            [DataMember(Name = "fields")] 
            public List<FieldsClass> Fields{ get; private set; }

        }
    }
}

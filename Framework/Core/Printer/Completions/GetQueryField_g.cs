/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryField_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.GetQueryField")]
    public sealed class GetQueryFieldCompletion : Completion<GetQueryFieldCompletion.PayloadData>
    {
        public GetQueryFieldCompletion(int RequestId, GetQueryFieldCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, Dictionary<string, FieldsClass> Fields = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Fields = Fields;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FieldNotFound,
                FormInvalid,
                FieldInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```formNotFound``` - The specified form cannot be found.
            /// * ```fieldNotFound``` - The specified field cannot be found.
            /// * ```formInvalid``` - The specified form is invalid.
            /// * ```fieldInvalid``` - The specified field is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class FieldsClass
            {
                public FieldsClass(int? IndexCount = null, TypeEnum? Type = null, ClassEnum? Class = null, AccessEnum? Access = null, OverflowEnum? Overflow = null, string InitialValue = null, string Format = null, CoercivityEnum? Coercivity = null)
                {
                    this.IndexCount = IndexCount;
                    this.Type = Type;
                    this.Class = Class;
                    this.Access = Access;
                    this.Overflow = Overflow;
                    this.InitialValue = InitialValue;
                    this.Format = Format;
                    this.Coercivity = Coercivity;
                }

                /// <summary>
                /// Specifies the number of entries for an index field. A value of zero indicates that this field is
                /// not an index field. Index fields are typically used to present information in a tabular fashion.
                /// </summary>
                [DataMember(Name = "indexCount")]
                public int? IndexCount { get; init; }

                public enum TypeEnum
                {
                    Text,
                    Micr,
                    Ocr,
                    Msf,
                    Barcode,
                    Graphic,
                    Pagemark
                }

                /// <summary>
                /// Specifies the type of field as one of the following:
                /// 
                /// * ```text``` - The field is a text field.
                /// * ```micr``` - The field is a Magnetic Ink Character Recognition field.
                /// * ```ocr``` - The field is an Optical Character Recognition field.
                /// * ```msf``` - The field is a Magnetic Stripe Facility field.
                /// * ```barcode``` - The field is a barcode field.
                /// * ```graphic``` - The field is a Graphic field.
                /// * ```pagemark``` - The field is a Page Mark field.
                /// </summary>
                [DataMember(Name = "type")]
                public TypeEnum? Type { get; init; }

                public enum ClassEnum
                {
                    Static,
                    Optional,
                    Required
                }

                /// <summary>
                /// Specifies the class of the field as one of the following:
                /// 
                /// * ```static``` - The field data cannot be set by the application.
                /// * ```optional``` - The field data can be set by the application.
                /// * ```required``` - The field data must be set by the application.
                /// </summary>
                [DataMember(Name = "class")]
                public ClassEnum? Class { get; init; }

                public enum AccessEnum
                {
                    Read,
                    Write,
                    ReadWrite
                }

                /// <summary>
                /// Specifies the field access as one of the following:
                /// 
                /// * ```read``` - The field is used for input.
                /// * ```write``` - The field is used for output.
                /// * ```readWrite``` - The field is used for both input and output.
                /// </summary>
                [DataMember(Name = "access")]
                public AccessEnum? Access { get; init; }

                public enum OverflowEnum
                {
                    Terminate,
                    Truncate,
                    BestFit,
                    Overwrite,
                    WordWrap
                }

                /// <summary>
                /// Specifies how an overflow of field data should be handled as one of the following:
                /// 
                /// * ```terminate``` - Return an error and terminate printing of the form.
                /// * ```truncate``` - Truncate the field data to fit in the field.
                /// * ```bestFit``` - Fit the text in the field.
                /// * ```overwrite``` - Print the field data beyond the extents of the field boundary.
                /// * ```wordWrap``` - If the field can hold more than one line the text is wrapped around. Wrapping
                ///   is performed, where possible, by splitting the line on a space character or a hyphen character
                ///   or any other character which is used to join two words together.
                /// </summary>
                [DataMember(Name = "overflow")]
                public OverflowEnum? Overflow { get; init; }

                /// <summary>
                /// The initial value of the field. When the form is printed (using
                /// [Printer.PrintForm](#printer.printform)), this value will be used if another value is not
                /// provided. This value will be omitted if the parameter is not specified in the field definition.
                /// </summary>
                [DataMember(Name = "initialValue")]
                public string InitialValue { get; init; }

                /// <summary>
                /// Format string as defined in the form for this field. This value will be omitted if the parameter
                /// is not specified in the field definition.
                /// </summary>
                [DataMember(Name = "format")]
                public string Format { get; init; }

                public enum CoercivityEnum
                {
                    Auto,
                    Low,
                    High
                }

                /// <summary>
                /// Specifies the coercivity to be used for writing the magnetic stripe as one of the following:
                /// 
                /// * ```auto``` - The coercivity is decided by the Service Provider or the hardware.
                /// * ```low``` - A low coercivity is to be used for writing the magnetic stripe.
                /// * ```high``` - A high coercivity is to be used for writing the magnetic stripe.
                /// </summary>
                [DataMember(Name = "coercivity")]
                public CoercivityEnum? Coercivity { get; init; }

            }

            /// <summary>
            /// Details of the field(s) requested. For each object, the key is the field name.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, FieldsClass> Fields { get; init; }

        }
    }
}

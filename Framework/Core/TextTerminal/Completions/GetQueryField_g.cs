/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "TextTerminal.GetQueryField")]
    public sealed class GetQueryFieldCompletion : Completion<GetQueryFieldCompletion.PayloadData>
    {
        public GetQueryFieldCompletion(int RequestId, GetQueryFieldCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, FieldsClass> Fields = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Fields = Fields;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FieldNotFound
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```formNotFound``` - The specified form cannot be found.
            /// * ```fieldNotFound``` - The specified field cannot be found.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class FieldsClass
            {
                public FieldsClass(PositionClass Position = null, SizeClass Size = null, FieldTypeEnum? FieldType = null, ScalingEnum? Scaling = null, ClassEnum? Class = null, KeysEnum? Keys = null, AccessEnum? Access = null, OverflowEnum? Overflow = null, StyleClass Style = null, HorizontalEnum? Horizontal = null, string Format = null, string InitialValue = null, List<byte> InitialGraphic = null)
                {
                    this.Position = Position;
                    this.Size = Size;
                    this.FieldType = FieldType;
                    this.Scaling = Scaling;
                    this.Class = Class;
                    this.Keys = Keys;
                    this.Access = Access;
                    this.Overflow = Overflow;
                    this.Style = Style;
                    this.Horizontal = Horizontal;
                    this.Format = Format;
                    this.InitialValue = InitialValue;
                    this.InitialGraphic = InitialGraphic;
                }

                [DataMember(Name = "position")]
                public PositionClass Position { get; init; }

                [DataMember(Name = "size")]
                public SizeClass Size { get; init; }

                public enum FieldTypeEnum
                {
                    Text,
                    Invisible,
                    Password,
                    Graphic
                }

                /// <summary>
                /// The type of the field as one of the following:
                /// 
                /// * ```text``` - A text field.
                /// * ```invisible``` - An invisible text field.
                /// * ```password``` - A password field, input is echoed as '\\*'.
                /// * ```graphic``` - A graphic field.
                /// </summary>
                [DataMember(Name = "fieldType")]
                public FieldTypeEnum? FieldType { get; init; }

                public enum ScalingEnum
                {
                    BestFit,
                    AsIs,
                    MaintainAspect
                }

                /// <summary>
                /// Information on how to size the graphic within the field as one of the following. Only relevant where
                /// *fieldType* is *graphic*, therefore will be ignored for other cases:
                /// 
                /// * ```bestFit``` - scale to size indicated.
                /// * ```asIs``` - render at native size.
                /// * ```maintainAspect``` - scale as close as possible to size indicated while maintaining
                ///   the aspect ratio and not losing graphic information.
                /// </summary>
                [DataMember(Name = "scaling")]
                public ScalingEnum? Scaling { get; init; }

                public enum ClassEnum
                {
                    Optional,
                    Static,
                    Required
                }

                /// <summary>
                /// Field class as one of the following:
                /// 
                /// * ```optional``` - The field is optional. The field data can be set by the [TextTerminal.WriteForm](#textterminal.writeform) command.
                /// * ```static``` - The field is static. The field data cannot be set by the [TextTerminal.WriteForm](#textterminal.writeform) command.
                /// * ```required``` - The field is required. The field data must be set by the [TextTerminal.WriteForm](#textterminal.writeform) command.
                /// </summary>
                [DataMember(Name = "class")]
                public ClassEnum? Class { get; init; }

                public enum KeysEnum
                {
                    Numeric,
                    Hexadecimal,
                    Alphanumeric
                }

                /// <summary>
                /// Accepted input key types. If null, the input key type is vendor dependent.
                /// The possible values are as one of the following:
                /// 
                /// * ```numeric``` - The input key is a number.
                /// * ```hexadecimal``` - The input key is a hexadecimal.
                /// * ```alphanumeric``` - The input key is either a letter or a number.
                /// </summary>
                [DataMember(Name = "keys")]
                public KeysEnum? Keys { get; init; }

                public enum AccessEnum
                {
                    Read,
                    Write,
                    ReadWrite
                }

                /// <summary>
                /// Specifies the field access as one of the following:
                /// 
                /// * ```read``` - The field is used for input from the physical device.
                /// * ```write``` - The field is used for output from the physical device.
                /// * ```readWrite``` - The field is used for both input and output from the physical device.
                /// </summary>
                [DataMember(Name = "access")]
                public AccessEnum? Access { get; init; }

                public enum OverflowEnum
                {
                    Terminate,
                    Truncate,
                    Overwrite
                }

                /// <summary>
                /// Specifies how an overflow of field data should be handled as one of the following:
                /// 
                /// * ```terminate``` - Return an error and terminate display of the form.
                /// * ```truncate``` - Truncate the field data to fit in the field.
                /// * ```overwrite``` - Display the field data beyond the extents of the field boundary.
                /// </summary>
                [DataMember(Name = "overflow")]
                public OverflowEnum? Overflow { get; init; }

                [DataContract]
                public sealed class StyleClass
                {
                    public StyleClass(bool? Underline = null, bool? Inverted = null, bool? Flashing = null)
                    {
                        this.Underline = Underline;
                        this.Inverted = Inverted;
                        this.Flashing = Flashing;
                    }

                    /// <summary>
                    /// Single underline.
                    /// </summary>
                    [DataMember(Name = "underline")]
                    public bool? Underline { get; init; }

                    /// <summary>
                    /// Text inverted.
                    /// </summary>
                    [DataMember(Name = "inverted")]
                    public bool? Inverted { get; init; }

                    /// <summary>
                    /// Flashing text.
                    /// </summary>
                    [DataMember(Name = "flashing")]
                    public bool? Flashing { get; init; }

                }

                /// <summary>
                /// Style attributes using a combination of the following. Some of the styles may be mutually exclusive, or may
                /// combine to provide unexpected results. This field is optional and may be null, in which case the default
                /// normal style will be used.
                /// </summary>
                [DataMember(Name = "style")]
                public StyleClass Style { get; init; }

                public enum HorizontalEnum
                {
                    Left,
                    Right,
                    Center
                }

                /// <summary>
                /// Horizontal alignment of field contents as one of the following:
                /// 
                /// * ```left``` - Align to the left.
                /// * ```right``` - Align to the right.
                /// * ```center``` - Align to the center.
                /// </summary>
                [DataMember(Name = "horizontal")]
                public HorizontalEnum? Horizontal { get; init; }

                /// <summary>
                /// This is an application-defined input field describing how the application should format the data.
                /// This may be interpreted by the Service Provider.
                /// <example>Application defined information</example>
                /// </summary>
                [DataMember(Name = "format")]
                public string Format { get; init; }

                /// <summary>
                /// Initial value, which may be changed dynamically at run-time. May be null if no initial value is required.
                /// Ignored for *graphic* fields, which are specified in *initialGraphic*.
                /// <example>Default text</example>
                /// </summary>
                [DataMember(Name = "initialValue")]
                public string InitialValue { get; init; }

                /// <summary>
                /// Initial graphic in Base64 format, which may be changed dynamically at run-time. May be null if no initial
                /// graphic is required. Ignored for non-*graphic* fields, which are specified in *initialValue*.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "initialGraphic")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> InitialGraphic { get; init; }

            }

            /// <summary>
            /// Details of the field(s) requested. For each object, the property name is the field name. This property
            /// is null if the form or field(s) cannot be found.
            /// </summary>
            [DataMember(Name = "fields")]
            public Dictionary<string, FieldsClass> Fields { get; init; }

        }
    }
}

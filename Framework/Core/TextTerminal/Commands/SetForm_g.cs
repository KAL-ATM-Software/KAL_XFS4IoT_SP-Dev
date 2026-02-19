/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * SetForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.TextTerminal.Commands
{
    //Original name = SetForm
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "TextTerminal.SetForm")]
    public sealed class SetFormCommand : Command<SetFormCommand.PayloadData>
    {
        public SetFormCommand()
            : base()
        { }

        public SetFormCommand(int RequestId, SetFormCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string Name = null, FormClass Form = null)
                : base()
            {
                this.Name = Name;
                this.Form = Form;
            }

            /// <summary>
            /// The name of the form.
            /// <example>Form10</example>
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

            [DataContract]
            public sealed class FormClass
            {
                public FormClass(SizeClass Size = null, VersionClass Version = null, string Copyright = null, string Title = null, string Comment = null, Dictionary<string, FieldsClass> Fields = null)
                {
                    this.Size = Size;
                    this.Version = Version;
                    this.Copyright = Copyright;
                    this.Title = Title;
                    this.Comment = Comment;
                    this.Fields = Fields;
                }

                [DataMember(Name = "size")]
                public SizeClass Size { get; init; }

                [DataContract]
                public sealed class VersionClass
                {
                    public VersionClass(string Version = null, string Date = null, string Author = null)
                    {
                        this.Version = Version;
                        this.Date = Date;
                        this.Author = Author;
                    }

                    /// <summary>
                    /// Specifies the major and optional minor version of the form. This may be null if the version is not
                    /// required in the form.
                    /// <example>1.2</example>
                    /// </summary>
                    [DataMember(Name = "version")]
                    [DataTypes(Pattern = @"^[1-9][0-9]*(\.[0-9]+)?$")]
                    public string Version { get; init; }

                    /// <summary>
                    /// The creation/modification date of the form. May be null if not required.
                    /// <example>2024-05-30</example>
                    /// </summary>
                    [DataMember(Name = "date")]
                    public string Date { get; init; }

                    /// <summary>
                    /// The author of the form. May be null if not required.
                    /// <example>S. Nakano</example>
                    /// </summary>
                    [DataMember(Name = "author")]
                    public string Author { get; init; }

                }

                /// <summary>
                /// The version of the form. May be null if not specified or required.
                /// </summary>
                [DataMember(Name = "version")]
                public VersionClass Version { get; init; }

                /// <summary>
                /// Copyright entry. May be null if not required.
                /// <example>Copyright (c) XYZ Corp.</example>
                /// </summary>
                [DataMember(Name = "copyright")]
                public string Copyright { get; init; }

                /// <summary>
                /// The title of the form. May be null if not required.
                /// <example>Form 1</example>
                /// </summary>
                [DataMember(Name = "title")]
                public string Title { get; init; }

                /// <summary>
                /// Additional comments about the form. May be null if not required.
                /// <example>This form is for purpose x</example>
                /// </summary>
                [DataMember(Name = "comment")]
                public string Comment { get; init; }

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
                /// One field definition for each field in the form. For each object, the property name is the field name.
                /// The field name within a form must be unique.
                /// </summary>
                [DataMember(Name = "fields")]
                public Dictionary<string, FieldsClass> Fields { get; init; }

            }

            /// <summary>
            /// The details of the form. May be null if the form is to be deleted.
            /// </summary>
            [DataMember(Name = "form")]
            public FormClass Form { get; init; }

        }
    }
}

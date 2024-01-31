/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryMedia_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Printer.GetQueryMedia")]
    public sealed class GetQueryMediaCompletion : Completion<GetQueryMediaCompletion.PayloadData>
    {
        public GetQueryMediaCompletion(int RequestId, GetQueryMediaCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, MediaTypeEnum? MediaType = null, BaseEnum? Base = null, int? UnitX = null, int? UnitY = null, int? SizeWidth = null, int? SizeHeight = null, int? PageCount = null, int? LineCount = null, int? PrintAreaX = null, int? PrintAreaY = null, int? PrintAreaWidth = null, int? PrintAreaHeight = null, int? RestrictedAreaX = null, int? RestrictedAreaY = null, int? RestrictedAreaWidth = null, int? RestrictedAreaHeight = null, int? Stagger = null, FoldTypeEnum? FoldType = null, PaperSourcesClass PaperSources = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.MediaType = MediaType;
                this.Base = Base;
                this.UnitX = UnitX;
                this.UnitY = UnitY;
                this.SizeWidth = SizeWidth;
                this.SizeHeight = SizeHeight;
                this.PageCount = PageCount;
                this.LineCount = LineCount;
                this.PrintAreaX = PrintAreaX;
                this.PrintAreaY = PrintAreaY;
                this.PrintAreaWidth = PrintAreaWidth;
                this.PrintAreaHeight = PrintAreaHeight;
                this.RestrictedAreaX = RestrictedAreaX;
                this.RestrictedAreaY = RestrictedAreaY;
                this.RestrictedAreaWidth = RestrictedAreaWidth;
                this.RestrictedAreaHeight = RestrictedAreaHeight;
                this.Stagger = Stagger;
                this.FoldType = FoldType;
                this.PaperSources = PaperSources;
            }

            public enum ErrorCodeEnum
            {
                MediaNotFound,
                MediaInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```mediaNotFound``` - The specified media definition cannot be found.
            /// * ```mediaInvalid``` - The specified media definition is invalid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            public enum MediaTypeEnum
            {
                Generic,
                Passbook,
                Multipart
            }

            /// <summary>
            /// Specifies the type of media as one of the following:
            /// This property is null if the specified media definition cannot be found or is invalid.
            /// 
            /// * ```generic``` - The media is generic, i.e. a single sheet.
            /// * ```passbook``` - The media is a passbook.
            /// * ```multipart``` - The media is a multi-part.
            /// </summary>
            [DataMember(Name = "mediaType")]
            public MediaTypeEnum? MediaType { get; init; }

            public enum BaseEnum
            {
                Inch,
                Mm,
                Rowcolumn
            }

            /// <summary>
            /// Specifies the base unit of measurement of the form and can be one of the following values:
            /// This property is null if the specified media definition cannot be found or is invalid.
            /// 
            /// * ```inch``` - The base unit is inches.
            /// * ```mm``` - The base unit is millimeters.
            /// * ```rowcolumn``` - The base unit is rows and columns.
            /// </summary>
            [DataMember(Name = "base")]
            public BaseEnum? Base { get; init; }

            /// <summary>
            /// Specifies the horizontal resolution of the base units as a fraction of the
            /// [base](#printer.getquerymedia.completion.properties.base) value. For example, a value of 16 applied to
            /// the base unit *inch* means that the base horizontal resolution is 1/16th inch.
            /// </summary>
            [DataMember(Name = "unitX")]
            [DataTypes(Minimum = 0)]
            public int? UnitX { get; init; }

            /// <summary>
            /// Specifies the vertical resolution of the base units as a fraction of the *base* value. For example, a
            /// value of 10 applied to the base unit *mm* means that the base vertical resolution is 0.1 mm.
            /// </summary>
            [DataMember(Name = "unitY")]
            [DataTypes(Minimum = 0)]
            public int? UnitY { get; init; }

            /// <summary>
            /// Specifies the width of the media in terms of the base horizontal resolution.
            /// </summary>
            [DataMember(Name = "sizeWidth")]
            [DataTypes(Minimum = 0)]
            public int? SizeWidth { get; init; }

            /// <summary>
            /// Specifies the height of the media in terms of the base vertical resolution.
            /// </summary>
            [DataMember(Name = "sizeHeight")]
            [DataTypes(Minimum = 0)]
            public int? SizeHeight { get; init; }

            /// <summary>
            /// Specifies the number of pages in media of type *passbook*. This will be null if not applicable.
            /// </summary>
            [DataMember(Name = "pageCount")]
            [DataTypes(Minimum = 0)]
            public int? PageCount { get; init; }

            /// <summary>
            /// Specifies the number of lines on a page for a media of type *passbook*. This will be null if not applicable.
            /// </summary>
            [DataMember(Name = "lineCount")]
            [DataTypes(Minimum = 0)]
            public int? LineCount { get; init; }

            /// <summary>
            /// Specifies the horizontal offset of the printable area relative to the top left corner of the media in
            /// terms of the base horizontal resolution.
            /// </summary>
            [DataMember(Name = "printAreaX")]
            [DataTypes(Minimum = 0)]
            public int? PrintAreaX { get; init; }

            /// <summary>
            /// Specifies the vertical offset of the printable area relative to the top left corner of the media in
            /// terms of the base vertical resolution.
            /// </summary>
            [DataMember(Name = "printAreaY")]
            [DataTypes(Minimum = 0)]
            public int? PrintAreaY { get; init; }

            /// <summary>
            /// Specifies the printable area width of the media in terms of the base horizontal resolution.
            /// </summary>
            [DataMember(Name = "printAreaWidth")]
            [DataTypes(Minimum = 0)]
            public int? PrintAreaWidth { get; init; }

            /// <summary>
            /// Specifies the printable area height of the media in terms of the base vertical resolution.
            /// </summary>
            [DataMember(Name = "printAreaHeight")]
            [DataTypes(Minimum = 0)]
            public int? PrintAreaHeight { get; init; }

            /// <summary>
            /// Specifies the horizontal offset of the restricted area relative to the top left corner of the media in
            /// terms of the base horizontal resolution.
            /// </summary>
            [DataMember(Name = "restrictedAreaX")]
            [DataTypes(Minimum = 0)]
            public int? RestrictedAreaX { get; init; }

            /// <summary>
            /// Specifies the vertical offset of the restricted area relative to the top left corner of the media in
            /// terms of the base vertical resolution.
            /// </summary>
            [DataMember(Name = "restrictedAreaY")]
            [DataTypes(Minimum = 0)]
            public int? RestrictedAreaY { get; init; }

            /// <summary>
            /// Specifies the restricted area width of the media in terms of the base horizontal resolution.
            /// </summary>
            [DataMember(Name = "restrictedAreaWidth")]
            [DataTypes(Minimum = 0)]
            public int? RestrictedAreaWidth { get; init; }

            /// <summary>
            /// Specifies the restricted area height of the media in terms of the base vertical resolution.
            /// </summary>
            [DataMember(Name = "restrictedAreaHeight")]
            [DataTypes(Minimum = 0)]
            public int? RestrictedAreaHeight { get; init; }

            /// <summary>
            /// Specifies the staggering from the top in terms of the base vertical resolution for media of type
            /// *passbook*. This will be null if not applicable.
            /// </summary>
            [DataMember(Name = "stagger")]
            [DataTypes(Minimum = 0)]
            public int? Stagger { get; init; }

            public enum FoldTypeEnum
            {
                None,
                Horizontal,
                Vertical
            }

            /// <summary>
            /// Specified the type of fold for media of type *passbook* as one of the following. This will be null
            /// if not applicable.
            /// 
            /// * ```none``` - Passbook has no fold.
            /// * ```horizontal``` - Passbook has a horizontal fold.
            /// * ```vertical``` - Passbook has a vertical fold.
            /// </summary>
            [DataMember(Name = "foldType")]
            public FoldTypeEnum? FoldType { get; init; }

            [DataContract]
            public sealed class PaperSourcesClass
            {
                public PaperSourcesClass(bool? Upper = null, bool? Lower = null, bool? External = null, bool? Aux = null, bool? Aux2 = null, bool? Park = null)
                {
                    this.Upper = Upper;
                    this.Lower = Lower;
                    this.External = External;
                    this.Aux = Aux;
                    this.Aux2 = Aux2;
                    this.Park = Park;
                }

                /// <summary>
                /// The upper paper source.
                /// </summary>
                [DataMember(Name = "upper")]
                public bool? Upper { get; init; }

                /// <summary>
                /// The lower paper source.
                /// </summary>
                [DataMember(Name = "lower")]
                public bool? Lower { get; init; }

                /// <summary>
                /// The external paper source.
                /// </summary>
                [DataMember(Name = "external")]
                public bool? External { get; init; }

                /// <summary>
                /// The auxiliary paper source.
                /// </summary>
                [DataMember(Name = "aux")]
                public bool? Aux { get; init; }

                /// <summary>
                /// The second auxiliary paper source.
                /// </summary>
                [DataMember(Name = "aux2")]
                public bool? Aux2 { get; init; }

                /// <summary>
                /// The parking station.
                /// </summary>
                [DataMember(Name = "park")]
                public bool? Park { get; init; }

                [DataTypes(Pattern = @"^[a-zA-Z]([a-zA-Z0-9]*)$")]
                [System.Text.Json.Serialization.JsonExtensionData]
                public Dictionary<string, System.Text.Json.JsonElement> ExtensionData { get; set; } = new();

                [DataTypes(Pattern = @"^[a-zA-Z]([a-zA-Z0-9]*)$")]
                [System.Text.Json.Serialization.JsonIgnore]
                public Dictionary<string, bool> ExtendedProperties
                {
                    get => MessageBase.ParseExtendedProperties<bool>(ExtensionData);
                    set => ExtensionData = MessageBase.CreateExtensionData<bool>(value);
                }

            }

            /// <summary>
            /// Specifies the paper sources to use when printing forms using this media. If null, the paper source
            /// is determined by the Service.
            /// </summary>
            [DataMember(Name = "paperSources")]
            public PaperSourcesClass PaperSources { get; init; }

        }
    }
}

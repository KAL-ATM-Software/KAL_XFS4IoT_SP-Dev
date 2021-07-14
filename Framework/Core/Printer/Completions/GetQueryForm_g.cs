/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetQueryForm_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.GetQueryForm")]
    public sealed class GetQueryFormCompletion : Completion<GetQueryFormCompletion.PayloadData>
    {
        public GetQueryFormCompletion(int RequestId, GetQueryFormCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string FormName = null, BaseEnum? Base = null, int? UnitX = null, int? UnitY = null, int? Width = null, int? Height = null, AlignmentEnum? Alignment = null, OrientationEnum? Orientation = null, int? OffsetX = null, int? OffsetY = null, int? VersionMajor = null, int? VersionMinor = null, string UserPrompt = null, List<string> Fields = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.FormName = FormName;
                this.Base = Base;
                this.UnitX = UnitX;
                this.UnitY = UnitY;
                this.Width = Width;
                this.Height = Height;
                this.Alignment = Alignment;
                this.Orientation = Orientation;
                this.OffsetX = OffsetX;
                this.OffsetY = OffsetY;
                this.VersionMajor = VersionMajor;
                this.VersionMinor = VersionMinor;
                this.UserPrompt = UserPrompt;
                this.Fields = Fields;
            }

            public enum ErrorCodeEnum
            {
                FormNotFound,
                FormInvalid
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
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

            public enum BaseEnum
            {
                Inch,
                Mm,
                RowColumn
            }

            /// <summary>
            /// Specifies the base unit of measurement of the form as one of the following:
            /// 
            /// * ```inch``` - The base unit is inches.
            /// * ```mm``` - The base unit is millimeters.
            /// * ```rowColumn``` - The base unit is rows and columns.
            /// </summary>
            [DataMember(Name = "base")]
            public BaseEnum? Base { get; init; }

            /// <summary>
            /// Specifies the horizontal resolution of the base units as a fraction of the
            /// [base](#printer.getqueryform.completion.properties.base) value. For example, a value of 16 applied to
            /// the base unit *inch* means that the base horizontal resolution is 1/16 inch.
            /// </summary>
            [DataMember(Name = "unitX")]
            public int? UnitX { get; init; }

            /// <summary>
            /// Specifies the vertical resolution of the base units as a fraction of the *base* value. For example, a
            /// value of 10 applied to the base unit *mm* means that the base vertical resolution is 0.1 mm.
            /// </summary>
            [DataMember(Name = "unitY")]
            public int? UnitY { get; init; }

            /// <summary>
            /// Specifies the width of the form in terms of the base horizontal resolution.
            /// </summary>
            [DataMember(Name = "width")]
            public int? Width { get; init; }

            /// <summary>
            /// Specifies the height of the form in terms of the base vertical resolution.
            /// </summary>
            [DataMember(Name = "height")]
            public int? Height { get; init; }

            public enum AlignmentEnum
            {
                TopLeft,
                TopRight,
                BottomLeft,
                BottomRight
            }

            /// <summary>
            /// Specifies the relative alignment of the form on the media and can be one of the following values:
            /// 
            /// * ```topLeft``` - The form is aligned relative to the top and left edges of the media.
            /// * ```topRight``` - The form is aligned relative to the top and right edges of the media.
            /// * ```bottomLeft``` - The form is aligned relative to the bottom and left edges of the media.
            /// * ```bottomRight``` - The form is aligned relative to the bottom and right edges of the media.
            /// </summary>
            [DataMember(Name = "alignment")]
            public AlignmentEnum? Alignment { get; init; }

            public enum OrientationEnum
            {
                Portrait,
                Landscape
            }

            /// <summary>
            /// Specifies the orientation of the form as one of the following values:
            /// 
            /// * ```portrait``` - The orientation of the form is portrait.
            /// * ```landscape``` - The orientation of the form is landscape.
            /// </summary>
            [DataMember(Name = "orientation")]
            public OrientationEnum? Orientation { get; init; }

            /// <summary>
            /// Specifies the horizontal offset of the position of the top-left corner of the form, relative to the
            /// left or right edge specified by [alignment](#printer.getqueryform.completion.properties.alignment).
            /// This value is specified in terms of the base horizontal resolution and is always positive.
            /// </summary>
            [DataMember(Name = "offsetX")]
            public int? OffsetX { get; init; }

            /// <summary>
            /// Specifies the vertical offset of the position of the top-left corner of the form, relative to the top
            /// or bottom edge specified by *alignment*. This value is specified in terms of the base vertical
            /// resolution and is always positive.
            /// </summary>
            [DataMember(Name = "offsetY")]
            public int? OffsetY { get; init; }

            /// <summary>
            /// Specifies the major version of the form. If the version is not specified in the form, then zero is
            /// returned.
            /// </summary>
            [DataMember(Name = "versionMajor")]
            public int? VersionMajor { get; init; }

            /// <summary>
            /// Specifies the minor version of the form. If the version is not specified in the form, then zero is
            /// returned.
            /// </summary>
            [DataMember(Name = "versionMinor")]
            public int? VersionMinor { get; init; }

            /// <summary>
            /// The user prompt string. This will be omitted if the form does not define a value for the user prompt.
            /// </summary>
            [DataMember(Name = "userPrompt")]
            public string UserPrompt { get; init; }

            /// <summary>
            /// The field names.
            /// </summary>
            [DataMember(Name = "fields")]
            public List<string> Fields { get; init; }

        }
    }
}

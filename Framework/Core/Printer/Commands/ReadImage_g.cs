/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * ReadImage_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Printer.Commands
{
    //Original name = ReadImage
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Command(Name = "Printer.ReadImage")]
    public sealed class ReadImageCommand : Command<ReadImageCommand.PayloadData>
    {
        public ReadImageCommand()
            : base()
        { }

        public ReadImageCommand(int RequestId, ReadImageCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(FrontImageClass FrontImage = null, BackImageClass BackImage = null, bool? PassportDataGroup1 = null, bool? PassportDataGroup2 = null)
                : base()
            {
                this.FrontImage = FrontImage;
                this.BackImage = BackImage;
                this.PassportDataGroup1 = PassportDataGroup1;
                this.PassportDataGroup2 = PassportDataGroup2;
            }

            [DataContract]
            public sealed class FrontImageClass
            {
                public FrontImageClass(ImageTypeEnum? ImageType = null, ColorFormatEnum? ColorFormat = null)
                {
                    this.ImageType = ImageType;
                    this.ColorFormat = ColorFormat;
                }

                public enum ImageTypeEnum
                {
                    Tif,
                    Wmf,
                    Bmp,
                    Jpg,
                    Png,
                    Gif,
                    Svg
                }

                /// <summary>
                /// Specifies the format of the image as one of the following:
                /// 
                /// * ```tif``` - TIF 6.0 format.
                /// * ```wmf``` - WMF (Windows Metafile) format.
                /// * ```bmp``` - BMP format.
                /// * ```jpg``` - JPG format.
                /// * ```png``` - Portable Network Graphics format.
                /// * ```gif``` - Graphics Interchange Format format.
                /// * ```svg``` - Scalable Vector Graphics format.
                /// <example>png</example>
                /// </summary>
                [DataMember(Name = "imageType")]
                public ImageTypeEnum? ImageType { get; init; }

                public enum ColorFormatEnum
                {
                    Binary,
                    Grayscale,
                    Fullcolor
                }

                /// <summary>
                /// Specifies the color format of the image as one of the following:
                /// 
                /// * ```binary``` - Binary (image contains two colors, usually the colors black and white).
                /// * ```grayscale``` - Gray scale (image contains multiple gray colors).
                /// * ```fullcolor``` - Full color (image contains colors like red, green, blue etc.).
                /// <example>fullcolor</example>
                /// </summary>
                [DataMember(Name = "colorFormat")]
                public ColorFormatEnum? ColorFormat { get; init; }

            }

            /// <summary>
            /// Specifies the format of the front image returned by this command. This can be null if no front image is
            /// requested.
            /// </summary>
            [DataMember(Name = "frontImage")]
            public FrontImageClass FrontImage { get; init; }

            [DataContract]
            public sealed class BackImageClass
            {
                public BackImageClass(ImageTypeEnum? ImageType = null, ColorFormatEnum? ColorFormat = null)
                {
                    this.ImageType = ImageType;
                    this.ColorFormat = ColorFormat;
                }

                public enum ImageTypeEnum
                {
                    Tif,
                    Wmf,
                    Bmp,
                    Jpg,
                    Png,
                    Gif,
                    Svg
                }

                /// <summary>
                /// Specifies the format of the image as one of the following:
                /// 
                /// * ```tif``` - TIF 6.0 format.
                /// * ```wmf``` - WMF (Windows Metafile) format.
                /// * ```bmp``` - BMP format.
                /// * ```jpg``` - JPG format.
                /// * ```png``` - Portable Network Graphics format.
                /// * ```gif``` - Graphics Interchange Format format.
                /// * ```svg``` - Scalable Vector Graphics format.
                /// <example>png</example>
                /// </summary>
                [DataMember(Name = "imageType")]
                public ImageTypeEnum? ImageType { get; init; }

                public enum ColorFormatEnum
                {
                    Binary,
                    Grayscale,
                    Fullcolor
                }

                /// <summary>
                /// Specifies the color format of the image as one of the following:
                /// 
                /// * ```binary``` - Binary (image contains two colors, usually the colors black and white).
                /// * ```grayscale``` - Gray scale (image contains multiple gray colors).
                /// * ```fullcolor``` - Full color (image contains colors like red, green, blue etc.).
                /// <example>fullcolor</example>
                /// </summary>
                [DataMember(Name = "colorFormat")]
                public ColorFormatEnum? ColorFormat { get; init; }

            }

            /// <summary>
            /// Specifies the format of the back image returned by this command. This can be null if no back image is
            /// requested.
            /// </summary>
            [DataMember(Name = "backImage")]
            public BackImageClass BackImage { get; init; }

            /// <summary>
            /// Specifies whether Data Group 1 from a passport should be returned using RFID
            /// (see [[Ref. printer-1](#ref-printer-1)]).
            /// </summary>
            [DataMember(Name = "passportDataGroup1")]
            public bool? PassportDataGroup1 { get; init; }

            /// <summary>
            /// Specifies whether Data Group 2 from a passport should be returned using RFID
            /// (see [[Ref. printer-1](#ref-printer-1)]).
            /// </summary>
            [DataMember(Name = "passportDataGroup2")]
            public bool? PassportDataGroup2 { get; init; }

        }
    }
}

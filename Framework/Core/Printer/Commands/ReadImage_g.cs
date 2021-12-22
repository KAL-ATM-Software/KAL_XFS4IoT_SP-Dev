/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "Printer.ReadImage")]
    public sealed class ReadImageCommand : Command<ReadImageCommand.PayloadData>
    {
        public ReadImageCommand(int RequestId, ReadImageCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, FrontImageTypeEnum? FrontImageType = null, BackImageTypeEnum? BackImageType = null, FrontImageColorFormatEnum? FrontImageColorFormat = null, BackImageColorFormatEnum? BackImageColorFormat = null, CodelineFormatEnum? CodelineFormat = null, ImageSourceClass ImageSource = null)
                : base(Timeout)
            {
                this.FrontImageType = FrontImageType;
                this.BackImageType = BackImageType;
                this.FrontImageColorFormat = FrontImageColorFormat;
                this.BackImageColorFormat = BackImageColorFormat;
                this.CodelineFormat = CodelineFormat;
                this.ImageSource = ImageSource;
            }

            public enum FrontImageTypeEnum
            {
                Tif,
                Wmf,
                Bmp,
                Jpg
            }

            /// <summary>
            /// Specifies the format of the front image returned by this command as one of the following. If omitted,
            /// no front image is returned.
            /// 
            /// * ```tif``` - The returned image is in TIF 6.0 format.
            /// * ```wmf``` - The returned image is in WMF (Windows Metafile) format.
            /// * ```bmp``` - The returned image is in BMP format.
            /// * ```jpg``` - The returned image is in JPG format.
            /// </summary>
            [DataMember(Name = "frontImageType")]
            public FrontImageTypeEnum? FrontImageType { get; init; }

            public enum BackImageTypeEnum
            {
                Tif,
                Wmf,
                Bmp,
                Jpg
            }

            /// <summary>
            /// Specifies the format of the back image returned by this command as one of the following. If omitted,
            /// no back image is returned.
            /// 
            /// * ```tif``` - The returned image is in TIF 6.0 format.
            /// * ```wmf``` - The returned image is in WMF (Windows Metafile) format.
            /// * ```bmp``` - The returned image is in BMP format.
            /// * ```jpg``` - The returned image is in JPG format.
            /// </summary>
            [DataMember(Name = "backImageType")]
            public BackImageTypeEnum? BackImageType { get; init; }

            public enum FrontImageColorFormatEnum
            {
                Binary,
                Grayscale,
                Fullcolor
            }

            /// <summary>
            /// Specifies the color format of the requested front image as one of the following:
            /// 
            /// * ```binary``` - The scanned image has to be returned in binary (image contains two colors,  usually
            ///   the colors black and white).
            /// * ```grayscale``` - The scanned image has to be returned in gray scale (image contains multiple gray
            ///   colors).
            /// * ```fullcolor``` - The scanned image has to be returned in full color (image contains colors like
            ///   red, green, blue, etc.).
            /// </summary>
            [DataMember(Name = "frontImageColorFormat")]
            public FrontImageColorFormatEnum? FrontImageColorFormat { get; init; }

            public enum BackImageColorFormatEnum
            {
                Binary,
                Grayscale,
                Fullcolor
            }

            /// <summary>
            /// Specifies the color format of the requested back image as one of the following:
            /// 
            /// * ```binary``` - The scanned image has to be returned in binary (image contains two colors,  usually
            ///   the colors black and white).
            /// * ```grayscale``` - The scanned image has to be returned in gray scale (image contains multiple gray
            ///   colors).
            /// * ```fullcolor``` - The scanned image has to be returned in full color (image contains colors like
            ///   red, green, blue etc.).
            /// </summary>
            [DataMember(Name = "backImageColorFormat")]
            public BackImageColorFormatEnum? BackImageColorFormat { get; init; }

            public enum CodelineFormatEnum
            {
                Cmc7,
                E13b,
                Ocr
            }

            /// <summary>
            /// Specifies the code line (MICR data) format, as one of the following options (not applicable if no imageSource
            /// selected):
            /// 
            /// * ```cmc7``` - Read CMC7 code line.
            /// * ```e13b``` - Read E13B code line.
            /// * ```ocr``` - Read code line using OCR.
            /// </summary>
            [DataMember(Name = "codelineFormat")]
            public CodelineFormatEnum? CodelineFormat { get; init; }

            [DataContract]
            public sealed class ImageSourceClass
            {
                public ImageSourceClass(bool? Front = null, bool? Back = null, bool? Codeline = null)
                {
                    this.Front = Front;
                    this.Back = Back;
                    this.Codeline = Codeline;
                }

                /// <summary>
                /// The front image of the document is requested.
                /// </summary>
                [DataMember(Name = "front")]
                public bool? Front { get; init; }

                /// <summary>
                /// The back image of the document is requested.
                /// </summary>
                [DataMember(Name = "back")]
                public bool? Back { get; init; }

                /// <summary>
                /// The code line of the document is requested.
                /// </summary>
                [DataMember(Name = "codeline")]
                public bool? Codeline { get; init; }

            }

            /// <summary>
            /// Specifies the source.
            /// </summary>
            [DataMember(Name = "imageSource")]
            public ImageSourceClass ImageSource { get; init; }

        }
    }
}

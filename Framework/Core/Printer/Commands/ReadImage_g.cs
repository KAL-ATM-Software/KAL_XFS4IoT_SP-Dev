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

            public PayloadData(int Timeout, FrontImageTypeEnum? FrontImageType = null, BackImageTypeEnum? BackImageType = null, FrontImageColorFormatEnum? FrontImageColorFormat = null, BackImageColorFormatEnum? BackImageColorFormat = null, CodelineFormatEnum? CodelineFormat = null, ImageSourceClass ImageSource = null, string FrontImageFile = null, string BackImageFile = null)
                : base(Timeout)
            {
                this.FrontImageType = FrontImageType;
                this.BackImageType = BackImageType;
                this.FrontImageColorFormat = FrontImageColorFormat;
                this.BackImageColorFormat = BackImageColorFormat;
                this.CodelineFormat = CodelineFormat;
                this.ImageSource = ImageSource;
                this.FrontImageFile = FrontImageFile;
                this.BackImageFile = BackImageFile;
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
            public FrontImageTypeEnum? FrontImageType { get; private set; }

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
            public BackImageTypeEnum? BackImageType { get; private set; }

            public enum FrontImageColorFormatEnum
            {
                Binary,
                Grayscale,
                Fullcolor
            }

            /// <summary>
            /// Specifies the color format of the requested front image as one of the following:
            /// 
            /// * ```binary``` - The scanned images has to be returned in binary (image contains two colors,  usually
            ///   the colors black and white).
            /// * ```grayscale``` - The scanned images has to be returned in gray scale (image contains multiple gray
            ///   colors).
            /// * ```fullcolor``` - The scanned images has to be returned in full color (image contains colors like
            ///   red, green, blueetc.).
            /// </summary>
            [DataMember(Name = "frontImageColorFormat")]
            public FrontImageColorFormatEnum? FrontImageColorFormat { get; private set; }

            public enum BackImageColorFormatEnum
            {
                Binary,
                Grayscale,
                Fullcolor
            }

            /// <summary>
            /// Specifies the color format of the requested back image as one of the following:
            /// 
            /// * ```binary``` - The scanned images has to be returned in binary (image contains two colors,  usually
            ///   the colors black and white).
            /// * ```grayscale``` - The scanned images has to be returned in gray scale (image contains multiple gray
            ///   colors).
            /// * ```fullcolor``` - The scanned images has to be returned in full color (image contains colors like
            ///   red, green, blue etc.).
            /// </summary>
            [DataMember(Name = "backImageColorFormat")]
            public BackImageColorFormatEnum? BackImageColorFormat { get; private set; }

            public enum CodelineFormatEnum
            {
                Cmc7,
                E13b,
                Ocr
            }

            /// <summary>
            /// Specifies the code line (MICR data) format, as one of the following flags (zero if source not
            /// selected):
            /// 
            /// * ```cmc7``` - Read CMC7 code line.
            /// * ```e13b``` - Read E13B code line.
            /// * ```ocr``` - Read code line using OCR.
            /// </summary>
            [DataMember(Name = "codelineFormat")]
            public CodelineFormatEnum? CodelineFormat { get; private set; }

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
                public bool? Front { get; private set; }

                /// <summary>
                /// The back image of the document is requested.
                /// </summary>
                [DataMember(Name = "back")]
                public bool? Back { get; private set; }

                /// <summary>
                /// The code line of the document is requested.
                /// </summary>
                [DataMember(Name = "codeline")]
                public bool? Codeline { get; private set; }

            }

            /// <summary>
            /// Specifies the source as a combination of the following flags:
            /// </summary>
            [DataMember(Name = "imageSource")]
            public ImageSourceClass ImageSource { get; private set; }

            /// <summary>
            /// File specifying where to store the front image, e.g. \"C:\\\\Temp\\\\FrontImage.bmp\". If omitted or empty,
            /// the front image data will be returned in the output parameter. This value must not contain UNICODE
            /// characters.
            /// 
            /// To reduce the size of data sent between the client and service, it is recommended to use of this
            /// parameter.
            /// </summary>
            [DataMember(Name = "frontImageFile")]
            public string FrontImageFile { get; private set; }

            /// <summary>
            /// File specifying where to store the back image, e.g. \"C:\\\\Temp\\\\BackImage.bmp\". If omitted or empty,
            /// the back image data will be returned in the output parameter. This value must not contain UNICODE
            /// characters.
            /// 
            /// To reduce the size of data sent between the client and service, it is recommended to use of this
            /// parameter.
            /// </summary>
            [DataMember(Name = "backImageFile")]
            public string BackImageFile { get; private set; }

        }
    }
}

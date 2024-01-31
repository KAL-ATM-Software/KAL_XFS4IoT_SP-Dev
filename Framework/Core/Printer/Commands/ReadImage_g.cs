/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Printer.ReadImage")]
    public sealed class ReadImageCommand : Command<ReadImageCommand.PayloadData>
    {
        public ReadImageCommand(int RequestId, ReadImageCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(FrontImageTypeEnum? FrontImageType = null, BackImageTypeEnum? BackImageType = null, FrontImageColorFormatEnum? FrontImageColorFormat = null, BackImageColorFormatEnum? BackImageColorFormat = null)
                : base()
            {
                this.FrontImageType = FrontImageType;
                this.BackImageType = BackImageType;
                this.FrontImageColorFormat = FrontImageColorFormat;
                this.BackImageColorFormat = BackImageColorFormat;
            }

            public enum FrontImageTypeEnum
            {
                Tif,
                Wmf,
                Bmp,
                Jpg,
                None
            }

            /// <summary>
            /// Specifies the format of the front image returned by this command as one of the following. This can
            /// be null if no front image is requested.
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
            /// Specifies the format of the back image returned by this command as one of the following. This can
            /// be null if no back image is requested.
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
            /// Specifies the color format of the requested front image as one of the following. This can
            /// be null if no front image is requested.
            /// 
            /// * ```binary``` - The scanned image has to be returned in binary (image contains two colors, usually
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
            /// Specifies the color format of the requested back image as one of the following. This can
            /// be null if no back image is requested.
            /// 
            /// * ```binary``` - The scanned image has to be returned in binary (image contains two colors, usually
            ///   the colors black and white).
            /// * ```grayscale``` - The scanned image has to be returned in gray scale (image contains multiple gray
            ///   colors).
            /// * ```fullcolor``` - The scanned image has to be returned in full color (image contains colors like
            ///   red, green, blue etc.).
            /// </summary>
            [DataMember(Name = "backImageColorFormat")]
            public BackImageColorFormatEnum? BackImageColorFormat { get; init; }

        }
    }
}

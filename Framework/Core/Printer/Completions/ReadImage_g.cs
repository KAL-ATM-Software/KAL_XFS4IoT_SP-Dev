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
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.ReadImage")]
    public sealed class ReadImageCompletion : Completion<ReadImageCompletion.PayloadData>
    {
        public ReadImageCompletion(int RequestId, ReadImageCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, ImagesClass Images = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Images = Images;
            }

            public enum ErrorCodeEnum
            {
                ShutterFail,
                MediaJammed,
                FileIOError,
                LampInoperative,
                MediaSize,
                MediaRejected
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed; operator intervention is required.
            /// * ```fileIOError``` - Directory does not exist or a File IO error occurred while storing the image to
            ///   the hard disk.
            /// * ```lampInoperative``` - Imaging lamp is inoperative.
            /// * ```mediaSize``` - The media entered has an incorrect size and the media remains inside the device.
            /// * ```mediaRejected``` - The media was rejected during the insertion phase. The
            ///   [Printer.MediaRejectedEvent](#printer.mediarejectedevent) event is posted with the details. The
            ///   device is still operational.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

            [DataContract]
            public sealed class ImagesClass
            {
                public ImagesClass(FrontClass Front = null, BackClass Back = null, CodelineClass Codeline = null)
                {
                    this.Front = Front;
                    this.Back = Back;
                    this.Codeline = Codeline;
                }

                [DataContract]
                public sealed class FrontClass
                {
                    public FrontClass(StatusEnum? Status = null, string Data = null)
                    {
                        this.Status = Status;
                        this.Data = Data;
                    }

                    public enum StatusEnum
                    {
                        Ok,
                        NotSupp,
                        Missing
                    }

                    /// <summary>
                    /// Status of data source. Possible values are:
                    /// 
                    /// * ```ok``` - The data is OK.
                    /// * ```notSupp``` - The data source is not supported.
                    /// * ```missing``` - The data source is missing, for example, the Service is unable to get the code line.
                    /// </summary>
                    [DataMember(Name = "status")]
                    public StatusEnum? Status { get; private set; }

                    /// <summary>
                    /// If the image source is front or back and the image data has not been stored to the hard disk (file name not
                    /// provided), this will contain the Base64 encoded image.
                    /// 
                    /// If the image source is codeline, this contains characters in the ASCII range. If the code line was read
                    /// using the OCR-A font then the ASCII codes will conform to Figure E1 in ANSI X3.17-1981. If the code line was
                    /// read using the OCR-B font then the ASCII codes will conform to Figure C2 in ANSI X3.49-1975. In both these
                    /// cases unrecognized characters will be reported as the REJECT code, 0x1A. The E13B and CMC7 fonts use the
                    /// ASCII equivalents for the standard characters and use the byte values as reported by the
                    /// Printer.CodelineMapping command for the symbols that are unique to MICR fonts.
                    /// </summary>
                    [DataMember(Name = "data")]
                    public string Data { get; private set; }

                }

                /// <summary>
                /// The front image status and data.
                /// </summary>
                [DataMember(Name = "front")]
                public FrontClass Front { get; private set; }

                [DataContract]
                public sealed class BackClass
                {
                    public BackClass(StatusEnum? Status = null, string Data = null)
                    {
                        this.Status = Status;
                        this.Data = Data;
                    }

                    public enum StatusEnum
                    {
                        Ok,
                        NotSupp,
                        Missing
                    }

                    /// <summary>
                    /// Status of data source. Possible values are:
                    /// 
                    /// * ```ok``` - The data is OK.
                    /// * ```notSupp``` - The data source is not supported.
                    /// * ```missing``` - The data source is missing, for example, the Service is unable to get the code line.
                    /// </summary>
                    [DataMember(Name = "status")]
                    public StatusEnum? Status { get; private set; }

                    /// <summary>
                    /// If the image source is front or back and the image data has not been stored to the hard disk (file name not
                    /// provided), this will contain the Base64 encoded image.
                    /// 
                    /// If the image source is codeline, this contains characters in the ASCII range. If the code line was read
                    /// using the OCR-A font then the ASCII codes will conform to Figure E1 in ANSI X3.17-1981. If the code line was
                    /// read using the OCR-B font then the ASCII codes will conform to Figure C2 in ANSI X3.49-1975. In both these
                    /// cases unrecognized characters will be reported as the REJECT code, 0x1A. The E13B and CMC7 fonts use the
                    /// ASCII equivalents for the standard characters and use the byte values as reported by the
                    /// Printer.CodelineMapping command for the symbols that are unique to MICR fonts.
                    /// </summary>
                    [DataMember(Name = "data")]
                    public string Data { get; private set; }

                }

                /// <summary>
                /// The back image status and data.
                /// </summary>
                [DataMember(Name = "back")]
                public BackClass Back { get; private set; }

                [DataContract]
                public sealed class CodelineClass
                {
                    public CodelineClass(StatusEnum? Status = null, string Data = null)
                    {
                        this.Status = Status;
                        this.Data = Data;
                    }

                    public enum StatusEnum
                    {
                        Ok,
                        NotSupp,
                        Missing
                    }

                    /// <summary>
                    /// Status of data source. Possible values are:
                    /// 
                    /// * ```ok``` - The data is OK.
                    /// * ```notSupp``` - The data source is not supported.
                    /// * ```missing``` - The data source is missing, for example, the Service is unable to get the code line.
                    /// </summary>
                    [DataMember(Name = "status")]
                    public StatusEnum? Status { get; private set; }

                    /// <summary>
                    /// If the image source is front or back and the image data has not been stored to the hard disk (file name not
                    /// provided), this will contain the Base64 encoded image.
                    /// 
                    /// If the image source is codeline, this contains characters in the ASCII range. If the code line was read
                    /// using the OCR-A font then the ASCII codes will conform to Figure E1 in ANSI X3.17-1981. If the code line was
                    /// read using the OCR-B font then the ASCII codes will conform to Figure C2 in ANSI X3.49-1975. In both these
                    /// cases unrecognized characters will be reported as the REJECT code, 0x1A. The E13B and CMC7 fonts use the
                    /// ASCII equivalents for the standard characters and use the byte values as reported by the
                    /// Printer.CodelineMapping command for the symbols that are unique to MICR fonts.
                    /// </summary>
                    [DataMember(Name = "data")]
                    public string Data { get; private set; }

                }

                /// <summary>
                /// The codeline status and data.
                /// </summary>
                [DataMember(Name = "codeline")]
                public CodelineClass Codeline { get; private set; }

            }

            /// <summary>
            /// The status and data for each of the requested images.
            /// </summary>
            [DataMember(Name = "images")]
            public ImagesClass Images { get; private set; }

        }
    }
}

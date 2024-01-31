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
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
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
                LampInoperative,
                MediaSize,
                MediaRejected
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```shutterFail``` - Open or close of the shutter failed due to manipulation or hardware error.
            /// * ```mediaJammed``` - The media is jammed; operator intervention is required.
            /// * ```lampInoperative``` - Imaging lamp is inoperative.
            /// * ```mediaSize``` - The media entered has an incorrect size and the media remains inside the device.
            /// * ```mediaRejected``` - The media was rejected during the insertion phase. The
            ///   [Printer.MediaRejectedEvent](#printer.mediarejectedevent) event is posted with the details. The
            ///   device is still operational.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class ImagesClass
            {
                public ImagesClass(FrontClass Front = null, BackClass Back = null)
                {
                    this.Front = Front;
                    this.Back = Back;
                }

                [DataContract]
                public sealed class FrontClass
                {
                    public FrontClass(StatusEnum? Status = null, List<byte> Data = null)
                    {
                        this.Status = Status;
                        this.Data = Data;
                    }

                    public enum StatusEnum
                    {
                        Ok,
                        Missing
                    }

                    /// <summary>
                    /// Status of data source. This will be null if not supported, otherwise
                    /// one of the following values:
                    /// 
                    /// * ```ok``` - The data is OK.
                    /// * ```missing``` - The data source is missing.
                    /// <example>missing</example>
                    /// </summary>
                    [DataMember(Name = "status")]
                    public StatusEnum? Status { get; init; }

                    /// <summary>
                    /// This contains the Base64 encoded image.
                    /// <example>SKHFFHGOWORIUNNNLSSL ...</example>
                    /// </summary>
                    [DataMember(Name = "data")]
                    [DataTypes(Pattern = @"^[A-Za-z0-9+/]*={0,2}$")]
                    public List<byte> Data { get; init; }

                }

                /// <summary>
                /// The front image status and data.
                /// </summary>
                [DataMember(Name = "front")]
                public FrontClass Front { get; init; }

                [DataContract]
                public sealed class BackClass
                {
                    public BackClass(StatusEnum? Status = null, List<byte> Data = null)
                    {
                        this.Status = Status;
                        this.Data = Data;
                    }

                    public enum StatusEnum
                    {
                        Ok,
                        Missing
                    }

                    /// <summary>
                    /// Status of data source. This will be null if not supported, otherwise
                    /// one of the following values:
                    /// 
                    /// * ```ok``` - The data is OK.
                    /// * ```missing``` - The data source is missing.
                    /// <example>missing</example>
                    /// </summary>
                    [DataMember(Name = "status")]
                    public StatusEnum? Status { get; init; }

                    /// <summary>
                    /// This contains the Base64 encoded image.
                    /// <example>SKHFFHGOWORIUNNNLSSL ...</example>
                    /// </summary>
                    [DataMember(Name = "data")]
                    [DataTypes(Pattern = @"^[A-Za-z0-9+/]*={0,2}$")]
                    public List<byte> Data { get; init; }

                }

                /// <summary>
                /// The back image status and data.
                /// </summary>
                [DataMember(Name = "back")]
                public BackClass Back { get; init; }

            }

            /// <summary>
            /// The status and data for each of the requested images. Only requested images are returned.
            /// </summary>
            [DataMember(Name = "images")]
            public ImagesClass Images { get; init; }

        }
    }
}

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
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "Printer.ReadImage")]
    public sealed class ReadImageCompletion : Completion<ReadImageCompletion.PayloadData>
    {
        public ReadImageCompletion()
            : base()
        { }

        public ReadImageCompletion(int RequestId, ReadImageCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, ImagesClass Images = null)
                : base()
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
                public ImagesClass(FrontClass Front = null, BackClass Back = null, PassportDataGroup1Class PassportDataGroup1 = null, PassportDataGroup2Class PassportDataGroup2 = null)
                {
                    this.Front = Front;
                    this.Back = Back;
                    this.PassportDataGroup1 = PassportDataGroup1;
                    this.PassportDataGroup2 = PassportDataGroup2;
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
                    /// This contains the Base64 encoded image. This may be null if no image is returned.
                    /// <example>O2gAUACFyEARAJAC</example>
                    /// </summary>
                    [DataMember(Name = "data")]
                    [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
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
                    /// This contains the Base64 encoded image. This may be null if no image is returned.
                    /// <example>O2gAUACFyEARAJAC</example>
                    /// </summary>
                    [DataMember(Name = "data")]
                    [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                    public List<byte> Data { get; init; }

                }

                /// <summary>
                /// The back image status and data.
                /// </summary>
                [DataMember(Name = "back")]
                public BackClass Back { get; init; }

                [DataContract]
                public sealed class PassportDataGroup1Class
                {
                    public PassportDataGroup1Class(StatusEnum? Status = null, List<byte> Data = null)
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
                    /// This contains the Base64 encoded image. This may be null if no image is returned.
                    /// <example>O2gAUACFyEARAJAC</example>
                    /// </summary>
                    [DataMember(Name = "data")]
                    [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                    public List<byte> Data { get; init; }

                }

                /// <summary>
                /// The Data Group 1 status and data from a passport. The data contains the associated fields as defined
                /// in [[Ref. printer-1](#ref-printer-1)].
                /// </summary>
                [DataMember(Name = "passportDataGroup1")]
                public PassportDataGroup1Class PassportDataGroup1 { get; init; }

                [DataContract]
                public sealed class PassportDataGroup2Class
                {
                    public PassportDataGroup2Class(StatusEnum? Status = null, List<byte> Data = null)
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
                    /// This contains the Base64 encoded image. This may be null if no image is returned.
                    /// <example>O2gAUACFyEARAJAC</example>
                    /// </summary>
                    [DataMember(Name = "data")]
                    [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                    public List<byte> Data { get; init; }

                }

                /// <summary>
                /// The Data Group 2 status and data from a passport. The data contains the associated fields as defined
                /// in [[Ref. printer-1](#ref-printer-1)].
                /// </summary>
                [DataMember(Name = "passportDataGroup2")]
                public PassportDataGroup2Class PassportDataGroup2 { get; init; }

            }

            /// <summary>
            /// The status and data for each of the requested images. Only requested images are returned.
            /// </summary>
            [DataMember(Name = "images")]
            public ImagesClass Images { get; init; }

        }
    }
}

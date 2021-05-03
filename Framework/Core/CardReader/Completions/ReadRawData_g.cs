/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ReadRawData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.ReadRawData")]
    public sealed class ReadRawDataCompletion : Completion<ReadRawDataCompletion.PayloadData>
    {
        public ReadRawDataCompletion(string RequestId, ReadRawDataCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            public enum ErrorCodeEnum
            {
                MediaJam,
                ShutterFail,
                NoMedia,
                InvalidMedia,
                CardTooShort,
                CardTooLong,
                SecurityFail,
                CardCollision,
            }

            /// <summary>
            /// Contains the data read from track 1.
            /// </summary>
            public class Track1Class
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public Track1Class (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Contains the data read from track 2.
            /// </summary>
            public class Track2Class
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public Track2Class (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Contains the data read from track 3.
            /// </summary>
            public class Track3Class
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public Track3Class (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            [DataContract]
            public sealed class ChipClass
            {
                /// <summary>
                /// The status values applicable to all data sources. Possible values are:
                /// 
                /// * ```ok``` - The data is OK.
                /// * ```dataMissing``` - The track/chip/memory chip is blank.
                /// * ```dataInvalid``` - The data contained on the track/chip/memory chip is invalid. This will typically be
                ///   returned when [data](#cardreader.readrawdata.completion.properties.security.data) reports *badReadLevel* or
                ///   *dataInvalid*.
                /// * ```dataTooLong``` - The data contained on the track/chip/memory chip is too long.
                /// * ```dataTooShort``` - The data contained on the track/chip/memory chip is too short.
                /// * ```dataSourceNotSupported``` - The data source to read from is not supported by the Service Provider.
                /// * ```dataSourceMissing``` - The data source to read from is missing on the card, or is unable to be read due to
                ///   a hardware problem, or the module has not been initialized. For example, this will be returned on a request to
                ///   read a Memory Card and the customer has entered a magnetic card without associated memory chip. This will also
                ///   be reported when *data* reports *noData*, *notInitialized* or *hardwareError*. This will also be reported when
                ///   the image reader could not create a BMP file due to the state of the image reader or due to a failure.
                /// </summary>
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }

                public ChipClass(StatusEnum? Status = null, string Data = null)
                    : base()
                {
                    this.Status = Status;
                    this.Data = Data;
                }

                /// <summary>
                /// The status values applicable to all data sources. Possible values are:
                /// 
                /// * ```ok``` - The data is OK.
                /// * ```dataMissing``` - The track/chip/memory chip is blank.
                /// * ```dataInvalid``` - The data contained on the track/chip/memory chip is invalid. This will typically be
                ///   returned when [data](#cardreader.readrawdata.completion.properties.security.data) reports *badReadLevel* or
                ///   *dataInvalid*.
                /// * ```dataTooLong``` - The data contained on the track/chip/memory chip is too long.
                /// * ```dataTooShort``` - The data contained on the track/chip/memory chip is too short.
                /// * ```dataSourceNotSupported``` - The data source to read from is not supported by the Service Provider.
                /// * ```dataSourceMissing``` - The data source to read from is missing on the card, or is unable to be read due to
                ///   a hardware problem, or the module has not been initialized. For example, this will be returned on a request to
                ///   read a Memory Card and the customer has entered a magnetic card without associated memory chip. This will also
                ///   be reported when *data* reports *noData*, *notInitialized* or *hardwareError*. This will also be reported when
                ///   the image reader could not create a BMP file due to the state of the image reader or due to a failure.
                /// </summary>
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }

                /// <summary>
                /// Base64 encoded representation of the data
                /// </summary>
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

            }

            /// <summary>
            /// Contains the data returned by the security module.
            /// </summary>
            public class SecurityClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                public enum DataEnum
                {
                    ReadLevel1,
                    ReadLevel2,
                    ReadLevel3,
                    ReadLevel4,
                    ReadLevel5,
                    BadReadLevel,
                    NoData,
                    DataInvalid,
                    HardwareError,
                    NotInitialized,
                }
                [DataMember(Name = "data")] 
                public DataEnum? Data { get; private set; }

                public SecurityClass (StatusEnum? Status, DataEnum? Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Contains the data read from the Swedish Watermark track.
            /// </summary>
            public class WatermarkClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public WatermarkClass (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Memory Card Identification data read from the memory chip.
            /// </summary>
            public class MemoryChipClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                public enum DataEnum
                {
                    ChipT0,
                    ChipT1,
                    ChipProtocolNotRequired,
                    ChipTypeAPart3,
                    ChipTypeAPart4,
                    ChipTypeB,
                    ChipTypeNFC,
                }
                [DataMember(Name = "data")] 
                public DataEnum? Data { get; private set; }

                public MemoryChipClass (StatusEnum? Status, DataEnum? Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Contains the data read from the front track 1. In some countries this track is known as JIS II track.
            /// </summary>
            public class Track1FrontClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public Track1FrontClass (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Contains the full path and file name of the BMP image file for the front of the card.
            /// </summary>
            public class FrontImageClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public FrontImageClass (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Contains the the full path and file name of the BMP image file for the back of the card.
            /// </summary>
            public class BackImageClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public BackImageClass (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Contains the data read from JIS I track 1 (8bits/char).
            /// </summary>
            public class Track1JISClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public Track1JISClass (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// data read from JIS I track 3 (8bits/char).
            /// </summary>
            public class Track3JISClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public Track3JISClass (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }

            /// <summary>
            /// Contains the dynamic digital identification data read from magnetic stripe.
            /// </summary>
            public class DdiClass
            {
                public enum StatusEnum
                {
                    Ok,
                    DataMissing,
                    DataInvalid,
                    DataTooLong,
                    DataTooShort,
                    DataSourceNotSupported,
                    DataSourceMissing,
                }
                [DataMember(Name = "status")] 
                public StatusEnum? Status { get; private set; }
                [DataMember(Name = "data")] 
                public string Data { get; private set; }

                public DdiClass (StatusEnum? Status, string Data)
                {
                    this.Status = Status;
                    this.Data = Data;
                }


            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, Track1Class Track1 = null, Track2Class Track2 = null, Track3Class Track3 = null, List<ChipClass> Chip = null, SecurityClass Security = null, WatermarkClass Watermark = null, MemoryChipClass MemoryChip = null, Track1FrontClass Track1Front = null, FrontImageClass FrontImage = null, BackImageClass BackImage = null, Track1JISClass Track1JIS = null, Track3JISClass Track3JIS = null, DdiClass Ddi = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Track1 = Track1;
                this.Track2 = Track2;
                this.Track3 = Track3;
                this.Chip = Chip;
                this.Security = Security;
                this.Watermark = Watermark;
                this.MemoryChip = MemoryChip;
                this.Track1Front = Track1Front;
                this.FrontImage = FrontImage;
                this.BackImage = BackImage;
                this.Track1JIS = Track1JIS;
                this.Track3JIS = Track3JIS;
                this.Ddi = Ddi;
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// 
            /// * ```mediaJam``` - The card is jammed. Operator intervention is required.
            /// * ```shutterFail``` - The open of the shutter failed due to manipulation or hardware error. Operator
            ///   intervention is required.
            /// * ```noMedia``` - The card was removed before completion of the read action (the event
            ///   [CardReader.MediaInsertedEvent](#cardreader.mediainsertedevent) has been generated). For motor
            ///   driven devices, the read is disabled; i.e. another command has to be issued to enable the reader for
            ///   card entry.
            /// * ```invalidMedia``` - No track or chip found; card may have been inserted or pulled through the wrong
            ///   way.
            /// * ```cardTooShort``` - The card that was inserted is too short. When this error occurs the card
            ///   remains at the exit slot.
            /// * ```cardTooLong``` - The card that was inserted is too long. When this error occurs the card remains
            ///   at the exit slot.
            /// * ```securityFail``` - The security module failed reading the cards security sign.
            /// * ```cardCollision``` - There was an unresolved collision of two or more contactless card signals.
            /// </summary>
            [DataMember(Name = "errorCode")] 
            public ErrorCodeEnum? ErrorCode { get; private set; }
            /// <summary>
            /// Contains the data read from track 1.
            /// </summary>
            [DataMember(Name = "track1")] 
            public Track1Class Track1 { get; private set; }
            /// <summary>
            /// Contains the data read from track 2.
            /// </summary>
            [DataMember(Name = "track2")] 
            public Track2Class Track2 { get; private set; }
            /// <summary>
            /// Contains the data read from track 3.
            /// </summary>
            [DataMember(Name = "track3")] 
            public Track3Class Track3 { get; private set; }
            /// <summary>
            /// Contains the ATR data read from the chip. For contactless chip card readers, multiple identification
            /// information can be returned if the card reader detects more than one chip. Each chip identification
            /// information is returned as an individual *data* array element.
            /// </summary>
            [DataMember(Name = "chip")] 
            public List<ChipClass> Chip{ get; private set; }
            /// <summary>
            /// Contains the data returned by the security module.
            /// </summary>
            [DataMember(Name = "security")] 
            public SecurityClass Security { get; private set; }
            /// <summary>
            /// Contains the data read from the Swedish Watermark track.
            /// </summary>
            [DataMember(Name = "watermark")] 
            public WatermarkClass Watermark { get; private set; }
            /// <summary>
            /// Memory Card Identification data read from the memory chip.
            /// </summary>
            [DataMember(Name = "memoryChip")] 
            public MemoryChipClass MemoryChip { get; private set; }
            /// <summary>
            /// Contains the data read from the front track 1. In some countries this track is known as JIS II track.
            /// </summary>
            [DataMember(Name = "track1Front")] 
            public Track1FrontClass Track1Front { get; private set; }
            /// <summary>
            /// Contains the full path and file name of the BMP image file for the front of the card.
            /// </summary>
            [DataMember(Name = "frontImage")] 
            public FrontImageClass FrontImage { get; private set; }
            /// <summary>
            /// Contains the the full path and file name of the BMP image file for the back of the card.
            /// </summary>
            [DataMember(Name = "backImage")] 
            public BackImageClass BackImage { get; private set; }
            /// <summary>
            /// Contains the data read from JIS I track 1 (8bits/char).
            /// </summary>
            [DataMember(Name = "track1JIS")] 
            public Track1JISClass Track1JIS { get; private set; }
            /// <summary>
            /// data read from JIS I track 3 (8bits/char).
            /// </summary>
            [DataMember(Name = "track3JIS")] 
            public Track3JISClass Track3JIS { get; private set; }
            /// <summary>
            /// Contains the dynamic digital identification data read from magnetic stripe.
            /// </summary>
            [DataMember(Name = "ddi")] 
            public DdiClass Ddi { get; private set; }

        }
    }
}

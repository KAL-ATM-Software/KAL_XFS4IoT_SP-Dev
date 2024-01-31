/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CardReader.ReadRawData")]
    public sealed class ReadRawDataCompletion : Completion<ReadRawDataCompletion.PayloadData>
    {
        public ReadRawDataCompletion(int RequestId, ReadRawDataCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, CardDataNullableClass Track1 = null, CardDataNullableClass Track2 = null, CardDataNullableClass Track3 = null, List<CardDataClass> Chip = null, SecurityClass Security = null, CardDataNullableClass Watermark = null, MemoryChipClass MemoryChip = null, CardDataNullableClass Track1Front = null, List<byte> FrontImage = null, List<byte> BackImage = null, CardDataNullableClass Track1JIS = null, CardDataNullableClass Track3JIS = null, CardDataNullableClass Ddi = null)
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

            public enum ErrorCodeEnum
            {
                MediaJam,
                ShutterFail,
                NoMedia,
                InvalidMedia,
                CardTooShort,
                CardTooLong,
                SecurityFail,
                CardCollision
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
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
            /// * ```securityFail``` - The security module failed reading the card's security and no other
            ///   data source was requested.
            /// * ```cardCollision``` - There was an unresolved collision of two or more contactless card signals.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Contains the data read from track 1. The property is null if not requested.
            /// </summary>
            [DataMember(Name = "track1")]
            public CardDataNullableClass Track1 { get; init; }

            /// <summary>
            /// Contains the data read from track 2. The property is null if not requested.
            /// </summary>
            [DataMember(Name = "track2")]
            public CardDataNullableClass Track2 { get; init; }

            /// <summary>
            /// Contains the data read from track 3. The property is null if not requested.
            /// </summary>
            [DataMember(Name = "track3")]
            public CardDataNullableClass Track3 { get; init; }

            /// <summary>
            /// Contains the ATR data read from the chip. For contactless chip card readers, multiple identification
            /// information can be returned if the card reader detects more than one chip. Each chip identification
            /// information is returned as an individual *data* array element. The property is null if not requested.
            /// </summary>
            [DataMember(Name = "chip")]
            public List<CardDataClass> Chip { get; init; }

            [DataContract]
            public sealed class SecurityClass
            {
                public SecurityClass(CardDataStatusEnum? Status = null, DataEnum? Data = null)
                {
                    this.Status = Status;
                    this.Data = Data;
                }

                [DataMember(Name = "status")]
                public CardDataStatusEnum? Status { get; init; }

                public enum DataEnum
                {
                    ReadLevel1,
                    ReadLevel2,
                    ReadLevel3,
                    ReadLevel4,
                    ReadLevel5,
                    BadReadLevel,
                    DataInvalid,
                    HardwareError,
                    NotInitialized
                }

                /// <summary>
                /// The security data can be one of the following:
                /// This property is null if there is no security data on the card.
                /// 
                /// * ```readLevel1``` - The security data readability level is 1.
                /// * ```readLevel2``` - The security data readability level is 2.
                /// * ```readLevel3``` - The security data readability level is 3.
                /// * ```readLevel4``` - The security data readability level is 4.
                /// * ```readLevel5``` - The security data readability level is 5.
                /// * ```badReadLevel``` - The security data reading quality is not acceptable.
                /// * ```dataInvalid``` - The validation of the security data with the specific data on the magnetic
                ///   stripe was not successful.
                /// * ```hardwareError``` - The security module could not be used because of a hardware error.
                /// * ```notInitialized``` - The security module could not be used because it was not initialized
                ///   (e.g. CIM key is not loaded).
                /// </summary>
                [DataMember(Name = "data")]
                public DataEnum? Data { get; init; }

            }

            /// <summary>
            /// Contains the data returned by the security module (i.e. MM, CIM86). If the check could not be
            /// executed, *status* and *data* indicate the cause of the security check failure; the *errorCode* will
            /// only be *securityFail* if no other data source is requested. The property is null if not requested.
            /// </summary>
            [DataMember(Name = "security")]
            public SecurityClass Security { get; init; }

            /// <summary>
            /// Contains the data read from the Swedish Watermark track. The property is null if not requested.
            /// </summary>
            [DataMember(Name = "watermark")]
            public CardDataNullableClass Watermark { get; init; }

            [DataContract]
            public sealed class MemoryChipClass
            {
                public MemoryChipClass(CardDataStatusEnum? Status = null, ProtocolEnum? Protocol = null, string Data = null)
                {
                    this.Status = Status;
                    this.Protocol = Protocol;
                    this.Data = Data;
                }

                [DataMember(Name = "status")]
                public CardDataStatusEnum? Status { get; init; }

                public enum ProtocolEnum
                {
                    ChipT0,
                    ChipT1,
                    ChipTypeAPart3,
                    ChipTypeAPart4,
                    ChipTypeB,
                    ChipTypeNFC
                }

                /// <summary>
                /// The memory card protocol used to communicate with the card. It can be one of the following:
                /// 
                /// * ```chipT0``` - The card reader has used the T=0 protocol.
                /// * ```chipT1``` - The card reader has used the T=1 protocol.
                /// * ```chipTypeAPart3``` - The card reader has used the ISO 14443 (Part3) Type A contactless chip
                ///   card protocol.
                /// * ```chipTypeAPart4``` - The card reader has used the ISO 14443 (Part4) Type A contactless chip
                ///   card protocol.
                /// * ```chipTypeB``` - The card reader has used the ISO 14443 Type B contactless chip card
                ///   protocol.
                /// * ```chipTypeNFC``` - The card reader has used the ISO 18092 (106/212/424kbps) contactless chip
                ///   card protocol.
                /// </summary>
                [DataMember(Name = "protocol")]
                public ProtocolEnum? Protocol { get; init; }

                /// <summary>
                /// Contains the data read from the memory chip in Base64. This property is null if not read.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "data")]
                public string Data { get; init; }

            }

            /// <summary>
            /// Memory Card Identification data read from the memory chip. The property is null if not requested.
            /// </summary>
            [DataMember(Name = "memoryChip")]
            public MemoryChipClass MemoryChip { get; init; }

            /// <summary>
            /// Contains the data read from the front track 1. In some countries this track is known as JIS II track.
            /// This property is null if not read.
            /// </summary>
            [DataMember(Name = "track1Front")]
            public CardDataNullableClass Track1Front { get; init; }

            /// <summary>
            /// Base64 encoded representation of the BMP image file for the front of the card.
            /// 
            /// The property is null if not requested or not read.
            /// <example>wCAAAQgwMDAwMDAwMA==</example>
            /// </summary>
            [DataMember(Name = "frontImage")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> FrontImage { get; init; }

            /// <summary>
            /// Base64 encoded representation of the BMP image file for the back of the card.
            /// 
            /// The property is null if not requested or not read.
            /// <example>wCAAAQgwMDAwMDAwMA==</example>
            /// </summary>
            [DataMember(Name = "backImage")]
            [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}$")]
            public List<byte> BackImage { get; init; }

            /// <summary>
            /// Contains the data read from JIS I track 1 (8bits/char). The property is null if not requested.
            /// </summary>
            [DataMember(Name = "track1JIS")]
            public CardDataNullableClass Track1JIS { get; init; }

            /// <summary>
            /// Contains the data read from JIS I track 3 (8bits/char). The property is null if not requested.
            /// </summary>
            [DataMember(Name = "track3JIS")]
            public CardDataNullableClass Track3JIS { get; init; }

            /// <summary>
            /// Contains the dynamic digital identification data read from magnetic stripe.
            /// The property is null if not requested.
            /// </summary>
            [DataMember(Name = "ddi")]
            public CardDataNullableClass Ddi { get; init; }

        }
    }
}

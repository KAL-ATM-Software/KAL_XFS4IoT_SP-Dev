/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ChipIOHandler.cs uses automatically generated parts. 
 * created at 4/20/2021 12:28:05 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;


namespace XFS4IoTFramework.CardReader
{
    /// The classes used by the device interface for an Input/Output parameters

    /// <summary>
    /// Device type requested by the framework
    /// </summary>
    public enum DeviceTypeEnum
    {
        Motor,
        Swipe,
        Dip,
        LatchedDip,
        Contactless,
        IntelligentContactless,
        Permanent,
    }

    /// <summary>
    /// Media status requested by the framework
    /// </summary>
    public enum MediaStatusEnum
    {
        NotSupported,
        Unknown,
        Present,
        NotPresent,
        Jammed,
        Entering,
        Latched
    }

    /// <summary>
    /// AcceptCardRequest
    /// Information contains to perform operation for accepting card and read card data if the device can read data while accepting card
    /// </summary>
    public sealed class AcceptCardRequest
    {
        /// <summary>
        /// AcceptAndReadCardRequest
        /// Card Data types to be read after card is accepted if the device has a capability to accept and read data
        /// </summary>
        /// <param name="DataToRead">The data type to be read in bitmap flags</param>
        /// <param name="FluxInactive">If this value is true, the flux senstor to be inactive, otherwise active</param>
        /// <param name="Timeout">Timeout on waiting a card is inserted</param>
        public AcceptCardRequest(ReadCardRequest.CardDataTypesEnum DataToRead,
                                 bool FluxInactive,
                                 int Timeout)
        {
            this.DataToRead = DataToRead;
            this.FluxInactive = FluxInactive;
            this.Timeout = Timeout;
        }

        public ReadCardRequest.CardDataTypesEnum DataToRead { get; private set; }

        /// <summary>
        /// Enable flux sensor or not
        /// </summary>
        public bool FluxInactive { get; private set; }

        /// <summary>
        /// Timeout for waiting card insertion
        /// </summary>
        public int Timeout { get; private set; }
    }

    /// <summary>
    /// AcceptCardResult
    /// Return result of accepting card, the card data must be cached until ReadCardData method gets called if the firmware command has capability to read card data and accept card
    /// </summary>
    public sealed class AcceptCardResult : DeviceResult
    {
        public AcceptCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                string ErrorDescription = null,
                                ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public AcceptCardResult(MessagePayload.CompletionCodeEnum CompletionCode)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
        }

        public enum ErrorCodeEnum
        {
            MediaJam,
            ShutterFail,
            NoMedia,
            InvalidMedia,
            CardTooShort,
            CardTooLong
        }

        /// <summary>
        /// Specifies the error code on accepting card. if there are colision for the contactless card or security card read failure, 
        /// error code must be returned by the following ReadCardAsync method and this method should return success.
        /// </summary>
        public ErrorCodeEnum? ErrorCode { get; private set; }
    }

    /// <summary>
    /// ReadCardRequest
    /// Information contains to perform operation for reading card after the card is successfully inserted in read position
    /// </summary>
    public sealed class ReadCardRequest
    {
        [Flags]
        public enum CardDataTypesEnum
        {
            NoDataRead = 0,
            Track1 = 0x0001,
            Track2 = 0x0002,
            Track3 = 0x0004,
            Chip = 0x0008,
            Security = 0x0010,
            MemoryChip = 0x0040,
            Track1Front = 0x0080,
            FrontImage = 0x0100,
            BackImage = 0x0200,
            Track1JIS = 0x0400,
            Track3JIS = 0x0800,
            Ddi = 0x4000,
            Watermark = 0x8000,
        }

        /// <summary>
        /// ReadCardRequest
        /// Card Data types to be read after card is accepted
        /// </summary>
        /// <param name="DataToRead">Data type to be read in bitmap flags</param>
        public ReadCardRequest(CardDataTypesEnum DataToRead)
        {
            this.DataToRead = DataToRead;
        }

        public CardDataTypesEnum DataToRead { get; private set; }
    }

    /// <summary>
    /// ReadCardResult
    /// Return result of accepting card, the card data must be cached until ReadCardData method gets called if the firmware command has capability to read card data and accept card
    /// </summary>
    public sealed class ReadCardResult : DeviceResult
    {
        /// <summary>
        /// Contains the data read from track 2.
        /// </summary>
        public class CardData
        {
            public enum DataStatusEnum
            {
                Ok,
                DataMissing,
                DataInvalid,
                DataTooLong,
                DataTooShort,
                DataSourceNotSupported,
                DataSourceMissing,
            }

            /// <summary>
            /// CardData
            /// Store card data read by the device class
            /// </summary>
            /// <param name="DataStatus">Status of reading the card data</param>
            /// <param name="Data">Read binary data</param>
            public CardData(DataStatusEnum? DataStatus = null,
                            List<byte> Data = null)
            {
                this.DataStatus = DataStatus;
                this.MemcoryChipDataStatus = null;
                this.SecutiryDataStatus = null;
                this.Data = Data;
            }

            /// <param name="DataStatus">Status of reading the card data</param>
            /// <param name="MemcoryChipDataStatus">Status of reading the memory chip data</param>
            public CardData(DataStatusEnum? DataStatus = null,
                            ReadRawDataCompletion.PayloadData.MemoryChipClass.DataEnum? MemcoryChipDataStatus = null)
            {
                this.DataStatus = DataStatus;
                this.MemcoryChipDataStatus = MemcoryChipDataStatus;
                this.SecutiryDataStatus = null;
                this.Data = null;
            }

            /// <param name="DataStatus">Status of reading the card data</param>
            /// <param name="SecutiryDataStatus">Status of reading the security data</param>
            public CardData(DataStatusEnum? DataStatus = null,
                            ReadRawDataCompletion.PayloadData.SecurityClass.DataEnum? SecutiryDataStatus = null)
            {
                this.DataStatus = DataStatus;
                this.MemcoryChipDataStatus = null;
                this.SecutiryDataStatus = SecutiryDataStatus;
                this.Data = null;
            }

            /// <summary>
            /// This field must be set for all requested the card data types.
            /// If there are hardware error on reading data, it can be omitted.
            /// </summary>
            public DataStatusEnum? DataStatus { get; private set; }

            /// <summary>
            /// This field must be set if the card data type MemoryChip is requested to be read, otherwise omitted.
            /// </summary>
            public ReadRawDataCompletion.PayloadData.MemoryChipClass.DataEnum? MemcoryChipDataStatus { get; private set; }

            /// <summary>
            /// This field must be set if the card data type Security is requested to be read, otherwise omitted.
            /// </summary>
            public ReadRawDataCompletion.PayloadData.SecurityClass.DataEnum? SecutiryDataStatus { get; private set; }

            /// <summary>
            /// The card data read to be stored except Memory chip and Secutiry data
            /// </summary>
            public List<byte> Data { get; private set; }
        }

        /// <summary>
        /// ReadCardResult
        /// Result of card data read.
        /// </summary>
        /// <param name="CompletionCode">Generic completion codes</param>
        /// <param name="ErrorCode">Command specific error codes</param>
        /// <param name="ErrorDescription">Details of error description</param>
        /// <param name="DataRead">Card data read in binary</param>
        /// <param name="ChipATRRead">Read chip ATR received</param>
        public ReadCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              ReadRawDataCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                              string ErrorDescription = null,
                              Dictionary<ReadCardRequest.CardDataTypesEnum, CardData> DataRead = null,
                              List<CardData> ChipATRRead = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.DataRead = DataRead;
            this.ChipATRRead = ChipATRRead;
        }
        public ReadCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              Dictionary<ReadCardRequest.CardDataTypesEnum, CardData> DataRead = null,
                              List<CardData> ChipATRRead = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.DataRead = DataRead;
            this.ChipATRRead = ChipATRRead;
        }
        public ReadCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              Dictionary<ReadCardRequest.CardDataTypesEnum, CardData> DataRead = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.DataRead = DataRead;
            this.ChipATRRead = null;
        }
        public ReadCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              List<CardData> ChipATRRead = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.DataRead = null;
            this.ChipATRRead = ChipATRRead;
        }

        /// <summary>
        /// ErrorCode
        /// This error code is set if the operation is failed, otherwise omitted
        /// </summary>
        public ReadRawDataCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }

        /// <summary>
        /// ReadData
        /// All read card data to be stored except chip ATR
        /// </summary>
        public Dictionary<ReadCardRequest.CardDataTypesEnum, CardData> DataRead { get; private set; }

        /// <summary>
        /// Contains the ATR data read from the chip. For contactless chip card readers, multiple identification
        /// information can be returned if the card reader detects more than one chip.
        /// </summary>
        public List<CardData> ChipATRRead { get; private set; }
    }

    /// <summary>
    /// WriteCardRequest
    /// Information contains to perform operation for writing card data after the card is successfully inserted in write position
    /// </summary>
    public sealed class WriteCardRequest
    {
        /// <summary>
        /// Contains the data to write tracks with method
        /// </summary>
        public class CardData
        {
            /// <summary>
            /// CardDataToWrite
            /// </summary>
            /// <param name="Data">Data to write to the track</param>
            /// <param name="WriteMethod">The coercivity to write data</param>
            public CardData(List<byte> Data = null,
                            WriteRawDataCommand.PayloadData.DataClass.WriteMethodEnum? WriteMethod = null)
            {
                this.Data = Data;
                this.WriteMethod = WriteMethod;
            }

            public WriteRawDataCommand.PayloadData.DataClass.WriteMethodEnum? WriteMethod { get; private set; }
            public List<byte> Data { get; private set; }
        }

        /// <summary>
        /// WriteCardDataRequest
        /// </summary>
        /// <param name="DataToWrite">Card data to write with destination. i.e. track1, 2 or 3</param>
        public WriteCardRequest(Dictionary<WriteRawDataCommand.PayloadData.DataClass.DestinationEnum, CardData> DataToWrite)
        {
            this.DataToWrite = DataToWrite;
        }

        public Dictionary<WriteRawDataCommand.PayloadData.DataClass.DestinationEnum, CardData> DataToWrite { get; private set; }
    }

    /// <summary>
    /// WriteCardResult
    /// Return result of writing data to the card tracks
    /// </summary>
    public sealed class WriteCardResult : DeviceResult
    {
        public WriteCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                               string ErrorDescription = null,
                               WriteRawDataCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public WriteRawDataCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }
    }

    /// <summary>
    /// EjectCardRequest
    /// Eject card informatio including position where card to be moved.
    /// </summary>
    public sealed class EjectCardRequest
    {
        /// <summary>
        /// EjectCardRequest
        /// </summary>
        /// <param name="Position">Positon to move card on eject operation</param>
        public EjectCardRequest(EjectCardCommand.PayloadData.EjectPositionEnum? Position = null)
        {
            this.Position = Position;
        }

        public EjectCardCommand.PayloadData.EjectPositionEnum? Position { get; private set; }
    }

    /// <summary>
    /// EjectCardResult
    /// Return result of ejecting/returning card
    /// </summary>
    public sealed class EjectCardResult : DeviceResult
    {
        public EjectCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                               string ErrorDescription = null,
                               EjectCardCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public EjectCardCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; } = null;
    }

    /// <summary>
    /// WriteCardDataResult
    /// Return result of writing data to the card tracks
    /// </summary>
    public sealed class CaptureCardResult : DeviceResult
    {
        public CaptureCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 string ErrorDescription = null,
                                 RetainCardCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                                 int? Count = null,
                                 RetainCardCompletion.PayloadData.PositionEnum? Position = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Count = Count;
            this.Position = Position;
        }

        public CaptureCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 int? Count = null,
                                 RetainCardCompletion.PayloadData.PositionEnum? Position = null)
           : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Count = Count;
            this.Position = Position;
        }

        public RetainCardCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }

        public int? Count { get; private set; }

        public RetainCardCompletion.PayloadData.PositionEnum? Position { get; private set; }
    }

    /// <summary>
    /// ChipIORequest
    /// Provide chip IO data to communicate with the chip
    /// </summary>
    public sealed class ChipIORequest
    {
        /// <summary>
        /// Current XFS4IoT specification is specified free string for the chip protocol
        /// It should be changed to enum string. - end of April 2021 preview
        /// </summary>
        public enum ChipProtocolEnum
        {
            chipT0,
            chipT1,
            chipTypeAPart3,
            chipTypeAPart4,
            chipTypeB,
            chipTypeNFC,
        }

        /// <summary>
        /// ChipIORequest
        /// </summary>
        /// <param name="ChipData">Binary data to be sent to the chip</param>
        public ChipIORequest(ChipProtocolEnum ChipProtocol, List<byte> ChipData)
        {
            this.ChipProtocol = ChipProtocol;
            this.ChipData = ChipData;
        }

        public List<byte> ChipData { get; private set; }

        public ChipProtocolEnum ChipProtocol;
    }

    /// <summary>
    /// ChipIOResult
    /// Return result of chip communication
    /// </summary>
    public sealed class ChipIOResult : DeviceResult
    {
        public ChipIOResult(MessagePayload.CompletionCodeEnum CompletionCode,
                            string ErrorDescription = null,
                            ChipIOCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                            List<byte> ChipData = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.ChipData = ChipData;
        }

        public ChipIOResult(MessagePayload.CompletionCodeEnum CompletionCode,
                            List<byte> ChipData = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.ChipData = ChipData;
        }

        public ChipIOCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }

        public List<byte> ChipData { get; private set; }
    }

    /// <summary>
    /// ChipPowerRequest
    /// Provide chip power action
    /// </summary>
    public sealed class ChipPowerRequest
    {
        /// <summary>
        /// ChipPowerRequest
        /// Handles the power actions that can be done on the chip.
        /// </summary>
        /// <param name="Action">Chip power action could be cold, warm or off</param>
        public ChipPowerRequest(ChipPowerCommand.PayloadData.ChipPowerEnum Action)
        {
            this.Action = Action;
        }

        public ChipPowerCommand.PayloadData.ChipPowerEnum Action { get; private set; }
    }

    /// <summary>
    /// ChipPowerResult
    /// Return result of power action to the chip.
    /// </summary>
    public sealed class ChipPowerResult : DeviceResult
    {
        public ChipPowerResult(MessagePayload.CompletionCodeEnum CompletionCode,
                               string ErrorDescription = null,
                               ChipPowerCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public ChipPowerCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }
    }

    /// <summary>
    /// ResetDeviceRequest
    /// Provide reset action information
    /// </summary>
    public sealed class ResetDeviceRequest
    {
        /// <summary>
        /// ResetDeviceRequest
        /// </summary>
        /// <param name="CardAction">Card action could be eject, capture or no move. if this value is set to null, the default action to be used.</param>
        public ResetDeviceRequest(ResetCommand.PayloadData.ResetInEnum? CardAction)
        {
            this.CardAction = CardAction;
        }

        public ResetCommand.PayloadData.ResetInEnum? CardAction { get; private set; }
    }

    /// <summary>
    /// ResetDeviceResult
    /// Return result of mechanical reset operation
    /// </summary>
    public sealed class ResetDeviceResult : DeviceResult
    {
        public ResetDeviceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 string ErrorDescription = null,
                                 ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }
    }

    /// <summary>
    /// ResetCountResult
    /// Return result of resetting retain bin counters
    /// </summary>
    public sealed class ResetCountResult : DeviceResult
    {
        public ResetCountResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                string ErrorDescription = null)
            : base(CompletionCode, ErrorDescription)
        { }
    }

    /// <summary>
    /// SetKeyRequest
    /// Provide key information to be loaded into the module.
    /// </summary>
    public sealed class SetCIM86KeyRequest
    {
        /// <summary>
        /// SetKeyRequest
        /// </summary>
        /// <param name="KeyValue">Key value to be loaded into CIM86 module</param>
        public SetCIM86KeyRequest(List<byte> KeyValue = null)
        {
            this.KeyValue = KeyValue;
        }

        public List<byte> KeyValue { get; private set; }
    }

    /// <summary>
    /// SetKeyResult
    /// Return result of loading key value into the module.
    /// </summary>
    public sealed class SetCIM86KeyResult : DeviceResult
    {
        public SetCIM86KeyResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 string ErrorDescription = null,
                                 SetKeyCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public SetKeyCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }
    }

    /// <summary>
    /// ParkCardRequest
    /// Provide parking card information
    /// </summary>
    public sealed class ParkCardRequest
    {
        /// <summary>
        /// ParkCardRequest
        /// Location is provided in this object where the card to be moved in the parking station.
        /// </summary>
        /// <param name="PowerAction">Specifies which way to move the card. if this value is null, default action to be used. 
        /// In - move card from transport to parking station
        /// Out - move card from parking station to the transport.</param>
        /// <param name="ParkingStation">Specifies which which parking station should be used. if the value is null, default location to be used.</param>
        public ParkCardRequest(ParkCardCommand.PayloadData.DirectionEnum? PowerAction, int? ParkingStation)
        {
            this.PowerAction = PowerAction;
            this.ParkingStation = ParkingStation;
        }

        public ParkCardCommand.PayloadData.DirectionEnum? PowerAction { get; private set; }

        public int? ParkingStation { get; private set; }
    }

    /// <summary>
    /// ParkCardResult
    /// Return result of moving a card to the parking station.
    /// </summary>
    public sealed class ParkCardResult : DeviceResult
    {
        public ParkCardResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              ParkCardCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public ParkCardCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }
    }

    public sealed class AIDInfo
    {
        public AIDInfo(List<byte> AID, bool PartialSelection, int TransactionType, List<byte> KernelIdentifier, List<byte> ConfigData)
        {
            this.AID = AID;
            this.PartialSelection = PartialSelection;
            this.TransactionType = TransactionType;
            this.KernelIdentifier = KernelIdentifier;
            this.ConfigData = ConfigData;
        }

        /// <summary>
        /// The application identifier to be accepted by the contactless chip card reader. The
        /// CardReader.EMVClessQueryApplications command will return the list of supported application identifiers.
        /// </summary>
        public List<byte> AID { get; private set; }

        /// <summary>
        /// If PartialSelection is true, partial name selection of the specified AID is enabled. If
        /// PartialSelection is false, partial name selection is disabled. A detailed explanation for
        /// partial name selection is given in EMV 4.3 Book 1, Section 11.3.5.
        /// </summary>
        public bool PartialSelection { get; private set; }

        /// <summary>
        /// The transaction type supported by the AID. This indicates the type of financial transaction
        /// represented by the first two digits of the ISO 8583:1987 Processing Code.
        /// </summary>
        public int TransactionType { get; private set; }

        /// <summary>
        /// The EMVCo defined kernel identifier associated with the AID.
        /// This field will be ignored if the reader does not support kernel identifiers.
        /// </summary>
        public List<byte> KernelIdentifier { get; private set; }

        /// <summary>
        /// The list of BER-TLV formatted configuration data, applicable to
        /// the specific AID-Kernel ID-Transaction Type combination. The appropriate payment systems
        /// specifications define the BER-TLV tags to be configured.
        /// </summary>

        public List<byte> ConfigData { get; private set; }

    }

    public sealed class PublicKey
    {
        /// <summary>
        /// The algorithm used in the calculation of the CA Public Key checksum.A detailed
        /// description of secure hash algorithm values is given in EMV Book 2, Annex B3; see reference
        /// [2]. For example, if the EMV specification indicates the algorithm is ‘01’, the value of the
        ///  algorithm is coded as 0x01.
        /// </summary>
        public int AlgorithmIndicator { get; private set; }

        /// <summary>
        /// The CA Public Key Exponent for the specific RID.This value
        /// is represented by the minimum number of bytes required.A detailed description of public key
        /// exponent values is given in EMV Book 2, Annex B2; see reference[2]. For example,
        /// representing value ‘216 + 1’ requires 3 bytes in hexadecimal(0x01, 0x00, 0x01), while value
        /// ‘3’ is coded as 0x03.
        /// </summary>
        public List<byte> Exponent;

        /// <summary>
        /// The CA Public Key Modulus for the specific RID.
        /// </summary>
        public List<byte> Modulus;

        /// <summary>
        /// The 20 byte checksum value for the CA Public Key
        /// </summary>
        public List<byte> Checksum;
    }

    public sealed class PublicKeyInfo
    {
        public PublicKeyInfo(List<byte> RID, List<PublicKeyInfo> CAPublicKey)
        {
            this.RID = RID;
            this.CAPublicKey = CAPublicKey;
        }

        /// <summary>
        /// Specifies the payment system's Registered Identifier (RID). RID is the first 5 bytes of the AID
        /// and identifies the payments system.
        /// </summary>
        public List<byte> RID { get; private set; }

        /// <summary>
        /// CA Public Key information for the specified RID
        /// </summary>
        public List<PublicKeyInfo> CAPublicKey { get; private set; }

    }

    /// <summary>
    /// EMVContactlessConfigureRequest
    /// Provide EMV terminal configuration to be set
    /// </summary>
    public sealed class EMVContactlessConfigureRequest
    {
        /// <summary>
        /// EMVContactlessConfigureRequest
        /// </summary>
        /// <param name="TerminalData">Terminal configuration data formatted in TLV.</param>
        /// <param name="AIDs">List of AIDs</param>
        /// <param name="PublicKeys">List of the CA publc keys</param>
        public EMVContactlessConfigureRequest(List<byte> TerminalData, List<AIDInfo> AIDs, List<PublicKeyInfo> PublicKeys)
        {
            this.TerminalData = TerminalData;
            this.AIDs = AIDs;
            this.PublicKeys = PublicKeys;
        }

        /// <summary>
        /// The BER-TLV formatted data for the terminal e.g. Terminal Type,
        /// Transaction Category Code, Merchant Name & Location etc. Any terminal based data elements referenced
        /// in the Payment Systems Specifications or EMVCo Contactless Payment Systems Specifications Books may be
        /// included.
        /// </summary>
        public List<byte> TerminalData { get; private set; }

        /// <summary>
        /// Specifies the list of acceptable payment system applications. For EMVCo approved contactless card
        /// readers each AID is associated with a Kernel Identifier and a Transaction Type. Legacy approved
        /// contactless readers may use only the AID.
        /// 
        /// Each AID-Transaction Type or each AID-Kernel-Transaction Type combination will have its own unique set
        /// of configuration data. 
        /// </summary>
        public List<AIDInfo> AIDs { get; private set; }

        /// <summary>
        /// Specifies the encryption key information required by an intelligent contactless chip card reader for
        /// offline data authentication.
        /// </summary>
        public List<PublicKeyInfo> PublicKeys { get; private set; }
    }

    /// <summary>
    /// EMVContactlessConfigureResult
    /// Return result of terminal configuration setup.
    /// </summary>
    public sealed class EMVContactlessConfigureResult : DeviceResult
    {
        public EMVContactlessConfigureResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                             string ErrorDescription = null,
                                             EMVClessConfigureCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public EMVClessConfigureCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }
    }

    /// <summary>
    /// Contains the chip returned data formatted in as track 2. This value is set after the contactless
    /// transaction has been completed with mag-stripe mode.
    /// </summary>
    public class EMVContactlessTransactionDataOutput
    {
        public enum TransactionOutcomeEnum
        {
            MultipleCards,
            Approve,
            Decline,
            OnlineRequest,
            OnlineRequestCompletionRequired,
            TryAgain,
            TryAnotherInterface,
            EndApplication,
            ConfirmationRequired,
        }

        /// <summary>
        /// Specifies the contactless transaction outcome
        /// </summary>
        public TransactionOutcomeEnum TransactionOutcome { get; private set; }

        public enum CardholderActionEnum
        {
            None,
            Retap,
            HoldCard,
        }

        /// <summary>
        ///  Specifies the cardholder action
        /// </summary>
        public CardholderActionEnum CardholderAction { get; private set; }

        /// <summary>
        /// The data read from the chip after a contactless transaction has been completed successfully.
        /// If the data source is chip, the BER-TLV formatted data contains cryptogram tag (9F26) after a contactless chip transaction has been completed successfully.
        /// if the data source is track1, track2 or track3, the data read from the chip, i.e the value returned by the card reader device and no cryptogram tag (9F26). 
        /// </summary>
        public List<byte> DataRead { get; private set; }

        /// <summary>
        /// The Entry Point Outcome specified in EMVCo Specifications for Contactless Payment Systems (Book A and B).
        /// This can be omitted for contactless chip card readers that do not follow EMVCo Entry Point Specifications.
        /// </summary>
        public class EMVContactlessOutcome
        {
            public enum CvmEnum
            {
                OnlinePIN,
                ConfirmationCodeVerified,
                Sign,
                NoCVM,
                NoCVMPreference,
            }

            /// <summary>
            ///  Specifies the cardholder verification method (CVM) to be performed
            /// </summary>
            public CvmEnum Cvm { get; private set; }

            public enum AlternateInterfaceEnum
            {
                Contact,
                MagneticStripe,
            }

            /// <summary>
            /// If the TransactionOutcome property is not TryAnotherInterface, this should be ignored.
            /// If the TransactionOutcome property is TryAnotherInterface, this specifies the alternative interface to be used to complete a transaction
            /// </summary>
            public AlternateInterfaceEnum AlternateInterface { get; private set; }

            public bool Receipt { get; private set; }

            /// <summary>
            /// The user interface details required to be displayed to the cardholder after processing the outcome of a
            /// contactless transaction. If no user interface details are required, this will be omitted. Please refer
            /// to EMVCo Contactless Specifications for Payment Systems Book A, Section 6.2 for details of the data
            /// within this object.
            /// </summary>
            public class EMVContactlessUI
            {
                /// <summary>
                /// Represents the EMVCo defined message identifier that indicates the text string to be displayed, e.g., 0x1B
                /// is the “Authorising Please Wait” message(see EMVCo Contactless Specifications for Payment Systems Book A, Section 9.4).
                /// </summary>
                public int MessageId { get; private set; }

                public enum StatusEnum
                {
                    NotReady,
                    Idle,
                    ReadyToRead,
                    Processing,
                    CardReadOk,
                    ProcessingError,
                }
                /// <summary>
                /// Represents the EMVCo defined transaction status value to be indicated through the Beep/LEDs
                /// </summary>
                public StatusEnum Status { get; private set; }

                /// <summary>
                /// Represents the hold time in units of 100 milliseconds for which the application should display the message
                /// before processing the next user interface data.
                /// </summary>
                public int HoldTime { get; private set; }

                public enum ValueQualifierEnum
                {
                    Amount,
                    Balance,
                    NotApplicable,
                }

                /// <summary>
                /// This data is defined by EMVCo as either “Amount”, “Balance”, or "NotApplicable"
                /// </summary>
                public ValueQualifierEnum ValueQualifier { get; private set; }

                /// <summary>
                /// Represents the value of the amount or balance as specified by ValueQualifier to be displayed where appropriate.
                /// </summary>
                public string Value { get; private set; }

                /// <summary>
                /// Represents the numeric value of currency code as per ISO 4217.
                /// </summary>
                public string CurrencyCode { get; private set; }

                /// <summary>
                /// Represents the language preference (EMV Tag ‘5F2D’) if returned by the card. The application should use this
                /// data to display all messages in the specified language until the transaction concludes.
                /// </summary>
                public string LanguagePreferenceData { get; private set; }

                public EMVContactlessUI(int MessageId,
                                        StatusEnum Status,
                                        int HoldTime,
                                        ValueQualifierEnum ValueQualifier,
                                        string Value,
                                        string CurrencyCode,
                                        string LanguagePreferenceData)
                {
                    this.MessageId = MessageId;
                    this.Status = Status;
                    this.HoldTime = HoldTime;
                    this.ValueQualifier = ValueQualifier;
                    this.Value = Value;
                    this.CurrencyCode = CurrencyCode;
                    this.LanguagePreferenceData = LanguagePreferenceData;
                }
            }

            /// <summary>
            /// The user interface details required to be displayed to the cardholder after processing the outcome of a
            /// contactless transaction.If no user interface details are required, this will be omitted.Please refer
            /// to EMVCo Contactless Specifications for Payment Systems Book A, Section 6.2 for details of the data within this object.
            /// </summary>
            public EMVContactlessUI UiOutcome { get; private set; }

            /// <summary>
            /// The user interface details required to be displayed to the cardholder when a transaction needs to be
            /// completed with a re-tap.If no user interface details are required, this will be omitted.
            /// </summary>
            public EMVContactlessUI UiRestart { get; private set; }

            /// <summary>
            /// The application should wait for this specific hold time in units of 100 milliseconds, before re-enabling
            /// the contactless card reader by issuing either the CardReader.EMVClessPerformTransaction or CardReader.EMVClessIssuerUpdate command depending on the value of
            /// For intelligent contactless card readers, the completion of this command ensures that the contactless chip card reader field is automatically turned off, so there is no need for the application to disable the field.
            /// </summary>
            public int FieldOffHoldTime { get; private set; }

            /// <summary>
            /// Specifies a timeout value in units of 100 milliseconds for prompting the user to remove the card.
            /// </summary>
            public int CardRemovalTimeout { get; private set; }

            /// <summary>
            /// The payment system's specific discretionary data read from the chip, in a BER-TLV format, 
            /// after a contactless transaction has been completed.If discretionary data is not present, this will be omitted.
            /// </summary>
            public List<byte> DiscretionaryData { get; private set; }

            public EMVContactlessOutcome(CvmEnum Cvm,
                                         AlternateInterfaceEnum AlternateInterface,
                                         bool Receipt,
                                         EMVContactlessUI UiOutcome,
                                         EMVContactlessUI UiRestart,
                                         int FieldOffHoldTime,
                                         int CardRemovalTimeout,
                                         List<byte> DiscretionaryData)
            {
                this.Cvm = Cvm;
                this.AlternateInterface = AlternateInterface;
                this.Receipt = Receipt;
                this.UiOutcome = UiOutcome;
                this.UiRestart = UiRestart;
                this.FieldOffHoldTime = FieldOffHoldTime;
                this.CardRemovalTimeout = CardRemovalTimeout;
                this.DiscretionaryData = DiscretionaryData;
            }
        }

        public EMVContactlessOutcome ClessOutcome { get; private set; }

        public EMVContactlessTransactionDataOutput(TransactionOutcomeEnum TransactionOutcome,
                                                   CardholderActionEnum CardholderAction,
                                                   List<byte> DataRead,
                                                   EMVContactlessOutcome ClessOutcome)
        {
            this.TransactionOutcome = TransactionOutcome;
            this.CardholderAction = CardholderAction;
            this.DataRead = DataRead;
            this.ClessOutcome = ClessOutcome;
        }
    }

    /// <summary>
    /// EMVContactlessPerformTransactionRequest
    /// Provide an information to perform EMV transaction
    /// </summary>
    public sealed class EMVContactlessPerformTransactionRequest
    {
        /// <summary>
        /// EMVClessPerformTransactionRequest
        /// </summary>
        /// <param name="TerminalData"></param>
        /// <param name="Timeout"></param>
        public EMVContactlessPerformTransactionRequest(List<byte> TerminalData, int Timeout)
        {
            this.TerminalData = TerminalData;
            this.Timeout = Timeout;
        }

        public List<byte> TerminalData { get; private set; }
        public int Timeout { get; private set; }
    }

    /// <summary>
    /// EMVContactlessPerformTransactionResult
    /// Return result of EMV transaction
    /// </summary>
    public sealed class EMVContactlessPerformTransactionResult : DeviceResult
    {

        public EMVContactlessPerformTransactionResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                                      string ErrorDescription = null,
                                                      EMVClessPerformTransactionCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                                                      Dictionary<DataSourceTypeEnum, EMVContactlessTransactionDataOutput> TransactionResults = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.TransactionResults = TransactionResults;
        }

        public EMVContactlessPerformTransactionResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                                      Dictionary<DataSourceTypeEnum, EMVContactlessTransactionDataOutput> TransactionResults = null)
           : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.TransactionResults = TransactionResults;
        }

        public EMVClessPerformTransactionCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }

        /// <summary>
        /// Transaction type completed in a mag-stripe mode or an EMV mode
        /// </summary>
        public enum DataSourceTypeEnum
        {
            Track1,
            Track2,
            Track3,
            Chip,
        }

        public Dictionary<DataSourceTypeEnum, EMVContactlessTransactionDataOutput> TransactionResults { get; private set; }
    }

    /// <summary>
    /// EMVContactlessIssuerUpdateRequest
    /// Provide an information to perform EMV transaction
    /// </summary>
    public sealed class EMVContactlessIssuerUpdateRequest
    {
        /// <summary>
        /// EMVClessIssuerUpdateRequest
        /// </summary>
        /// <param name="TerminalData"></param>
        /// <param name="Timeout"></param>
        public EMVContactlessIssuerUpdateRequest(List<byte> TerminalData, int Timeout)
        {
            this.TerminalData = TerminalData;
            this.Timeout = Timeout;
        }

        public List<byte> TerminalData { get; private set; }
        public int Timeout { get; private set; }
    }

    /// <summary>
    /// EMVContactlessIssuerUpdateResult
    /// Return result of EMV transaction
    /// </summary>
    public sealed class EMVContactlessIssuerUpdateResult : DeviceResult
    {

        public EMVContactlessIssuerUpdateResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                                string ErrorDescription = null,
                                                EMVClessIssuerUpdateCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                                                EMVContactlessTransactionDataOutput TransactionResult = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.TransactionResult = TransactionResult;
        }

        public EMVContactlessIssuerUpdateResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                                EMVContactlessTransactionDataOutput TransactionResult = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.TransactionResult = TransactionResult;
        }

        public EMVClessIssuerUpdateCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; private set; }

        /// <summary>
        /// Result of the contactless transaction
        /// </summary>
        public EMVContactlessTransactionDataOutput TransactionResult { get; private set; }
    }

    /// <summary>
    /// EMVApplication
    /// Provide chip application and kernel identifier supported
    /// </summary>
    public sealed class EMVApplication
    {
        public EMVApplication(List<byte> ApplicationIdentifier,
                              List<byte> KernelIdentifier)
        {
            this.ApplicationIdentifier = ApplicationIdentifier;
            this.KernelIdentifier = KernelIdentifier;
        }

        /// <summary>
        /// Chip application identifier
        /// </summary>
        public List<byte> ApplicationIdentifier { get; private set; }
        /// <summary>
        /// The kernel identifier certified
        /// </summary>
        public List<byte> KernelIdentifier { get; private set; }
    }

    /// <summary>
    /// QueryEMVApplicationResult
    /// Return information for supported EMV applications by the device
    /// </summary>
    public sealed class QueryEMVApplicationResult : DeviceResult
    {
        public QueryEMVApplicationResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                         ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                                         string ErrorDescription = null,
                                         List<EMVApplication> EMVApplications = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.EMVApplications = EMVApplications;
        }

        public QueryEMVApplicationResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                         List<EMVApplication> EMVApplications = null)
            : base(CompletionCode, null)
        {
            this.EMVApplications = EMVApplications;
        }

        /// <summary>
        /// List of EMV applications and kernels information
        /// </summary>
        public List<EMVApplication> EMVApplications { get; private set; }
    }

    /// <summary>
    /// IFMIdentifierInfo
    /// Provide IFM identifier information
    /// </summary>
    public sealed class IFMIdentifierInfo
    {
        public IFMIdentifierInfo(QueryIFMIdentifierCompletion.PayloadData.IfmAuthorityEnum IFMAuthority,
                                 List<byte> IFMIdentifier)
        {
            this.IFMAuthority = IFMAuthority;
            this.IFMIdentifier = IFMIdentifier;
        }

        public QueryIFMIdentifierCompletion.PayloadData.IfmAuthorityEnum IFMAuthority { get; private set; }
        /// <summary>
        /// The IFM Identifier of the chip card reader (or IFM) as assigned by the specified authority.
        /// </summary>
        public List<byte> IFMIdentifier { get; private set; }
    }

    /// <summary>
    /// QueryIFMIdentifierResult
    /// Return information for IFM identifiers
    /// </summary>
    public sealed class QueryIFMIdentifierResult : DeviceResult
    {
        public QueryIFMIdentifierResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                        string ErrorDescription = null,
                                        List<IFMIdentifierInfo> IFMIdentifiers = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.IFMIdentifiers = IFMIdentifiers;
        }

        public QueryIFMIdentifierResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                        List<IFMIdentifierInfo> IFMIdentifiers = null)
            : base(CompletionCode, null)
        {
            this.IFMIdentifiers = IFMIdentifiers;
        }

        public List<IFMIdentifierInfo> IFMIdentifiers;
    }
}

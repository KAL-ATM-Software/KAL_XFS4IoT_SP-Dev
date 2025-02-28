using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;

namespace XFS4IoTFramework.GermanSpecific
{
    public class HSMTDataSet(
        string TerminalId,
        string BankCode,
        string OnlineDateAndTime)
    {

        /// <summary>
        /// Terminal ID.ISO 8583 BMP 41. A data source is the EPP.
        /// Regular expression ^[0-9]{8}$
        /// </summary>
        public string TerminalId { get; init; } = TerminalId;

        /// <summary>
        /// Bank code.ISO 8583 BMP 42(rightmost 4 bytes see[[Ref.german - 2](#ref-german-2)])
        /// Account data for terminal account. A data source is the EPP.
        /// Regular expression ^[0-9]{8}$
        /// </summary>
        public string BankCode { get; init; } = BankCode;

        /// <summary>
        /// Online date and time.ISO 8583 BMP 61(YYYYMMDDHHMMSS).
        /// A data source is the HSM.
        /// Regular expression: ^20\\d{2}(0[1-9]|1[0,1,2])(0[1-9]|[12][0-9]|3[01])(0[0-9]|1[0-9]|2[0-3])[0-5][0-9][0-5][0-9]$
        /// </summary>
        public string OnlineDateAndTime { get; init; } = OnlineDateAndTime;
    }

    public sealed class GetHSMTDataRead(
        string TerminalId,
        string BankCode,
        string OnlineDateAndTime,
        string ZAKAId,
        GetHSMTDataRead.HSMStatusEnum HSMStatus,
        string HSMManufacturerId,
        string HSMSerialNumber) : HSMTDataSet(TerminalId, BankCode, OnlineDateAndTime)
    {
        public enum HSMStatusEnum
        {
            OutOfOrder = 1, //irreversibly out of order.
            OutOfOrderMissingKey = 2, //out of order, K_UR is not loaded.
            NotPreInitialized = 3, //not pre-initialized, K_UR is loaded.
            PreInitialized = 4, //pre-initialized, K_INIT is loaded.
            Initialized = 5, //initialized/personalized, K_PERS is loaded.
        }

        /// <summary>
        /// ZKA ID(is filled during the pre-initialization of the HSM). A data source is the HSM.
        /// Regular expression ^.{16}$
        /// </summary>
        public string ZAKId { get; init; } = ZAKAId;

        /// <summary>
        /// HSM status. A data source is the HSM.
        /// OutOfOrder = 1, irreversibly out of order.
        /// OutOfOrderMissingKey = 2, out of order, K_UR is not loaded.
        /// NotPreInitialized = 3, not pre-initialized, K_UR is loaded.
        /// PreInitialized = 4, pre-initialized, K_INIT is loaded.
        /// Initialized = 5, initialized/personalized, K_PERS is loaded.
        /// </summary>
        public HSMStatusEnum HSMStatus { get; init; } = HSMStatus;

        /// <summary>
        /// HSM manufacturer ID as needed for ISO BMP 57 of a pre-initialization.A data source is the EPP.
        /// Regular expression ^.{6,}$
        /// </summary>
        public string HSMManufacturerId { get; init; } = HSMManufacturerId;

        /// <summary>
        /// HSM serial number as needed for ISO BMP 57 of a pre-initialization.A data source is the EPP.
        /// Regular expression ^.{10}$
        /// </summary>
        public string HSMSerialNumber { get; init; } = HSMSerialNumber;
    }

    public sealed class GetHSMTDataResponse : DeviceResult
    {
        public GetHSMTDataResponse(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            GetHSMTDataRead HSMTDataRead = null) : base(CompletionCode, ErrorDescription)
        {
            this.HSMTDataRead = HSMTDataRead;
        }

        public GetHSMTDataResponse(
            MessageHeader.CompletionCodeEnum CompletionCode,
            GetHSMTDataRead HSMTDataRead = null) : base(CompletionCode, null)
        {
            this.HSMTDataRead = HSMTDataRead;
        }

        public GetHSMTDataRead HSMTDataRead { get; init; }
    }

    public sealed class SetHSMTDataRequest(
        string TerminalId,
        string BankCode,
        string OnlineDateAndTime) : HSMTDataSet(TerminalId, BankCode, OnlineDateAndTime)
    {
    }

    public sealed class SetHSMTDataResponse(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        SetHSMTDataResponse.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {
        public enum ErrorCodeEnum
        {
            AccessDenied, //The encryption module is either not initialized or not ready for any vendor-specific reason.
            HSMStateInvalid, //The HSM is not in a correct state to handle this command.
        }

        /// <summary>
        /// Specifies the error code if applicable
        /// </summary>
        public ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }
    public sealed class HSMInitRequest(
        HSMInitRequest.InitModeEnum InitMode,
        string OnlineTime)
    {
        public enum InitModeEnum
        {
            Temp, //Initialize the HSM temporarily (K_UR remains loaded). For predefined key name K_UR see 'KUR'.
            Definite, //Initialize the HSM definitely(K_UR is deleted). 
            Irreversible, //Initialize the HSM irreversibly(can only be restored by the vendor).
        }

        /// <summary>
        /// Specifies the initialization mode
        /// </summary>
        public InitModeEnum InitMode { get; init; } = InitMode;

        /// <summary>
        /// Specifies the online date and time in the format YYYYMMDDHHMMSS defined in ISO 8583 BMP 61.
        /// Regular expression: ^[0-9]{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9][0-5][0-9]$
        /// </summary>
        public string OnlineTime { get; init; } = OnlineTime;
    }

    public sealed class HSMInitResponse(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        HSMInitResponse.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {
        public enum ErrorCodeEnum
        {
            ModeNotSupported, //The specified init mode is not supported.
            HSMStateInvalid, //The HSM is not in a correct state to handle this command.
        }

        /// <summary>
        /// Specifies the error code.
        /// </summary>
        public ErrorCodeEnum? ErrorCode = ErrorCode;
    }

    public sealed class SecureMsgSendRequest(
        SecureMsgSendRequest.ProtocolEnum Protocol,
        List<byte> Message)
    {
        public enum ProtocolEnum
        {
            ISOPS, //ISO 8583 protocol for the personalization system. 
            RawData, //Raw data protocol.
            HSMLDI, //HSM LDI protocol.
            GenAs, //Generic PAC/MAC for non-ISO 8583 message formats.
            PinCmp, //Protocol for comparing PINs entered in the PIN pad during a PIN Change transaction.
        }

        /// <summary>
        /// Specifies the protocol the message belongs to.
        /// </summary>
        public ProtocolEnum Protocol { get; init; } = Protocol;

        /// <summary>
        /// Specifies the message that should be sent. This property is an empty if the Protocol property is set to HSMLDI.
        /// </summary>
        public List<byte> Message { get; init; } = Message;
    }

    public sealed class SecureMsgSendResponse : DeviceResult
    {
        public enum ErrorCodeEnum
        {
            AccessDenied, //The encryption module is either not initialized or not ready for any vendor-specific reason.
            HSMStateInvalid, //The HSM is not in a correct state to handle this message.
            ProtocolInvalid, //The specified protocol is invalid.
            FormatInvalid, //The format of the message is invalid.
            ContentInvalid, //The contents of one of the security relevant properties are invalid.
            KeyNotFound, //No key was found for PAC/MAC generation.
            NoPin, //No PIN or insufficient PIN-digits have been entered.
        }

        public SecureMsgSendResponse(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ErrorCodeEnum? ErrorCode = null,
            List<byte> Message = null) : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Message = Message;
        }

        public SecureMsgSendResponse(
            MessageHeader.CompletionCodeEnum CompletionCode,
            List<byte> Message = null) : base(CompletionCode, null)
        {
            ErrorCode = null;
            this.Message = Message;
        }

        public ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// The modified message that can be sent to an authorization system or personalization system.
        /// </summary>
        public List<byte> Message { get; init; }
    }

    public sealed class SecureMsgReceiveRequest(
        SecureMsgReceiveRequest.ProtocolEnum Protocol,
        List<byte> Message)
    {
        public enum ProtocolEnum
        {
            ISOPS, //ISO 8583 protocol for the personalization system. 
            RawData, //Raw data protocol.
            GenAs, //Generic PAC/MAC for non-ISO 8583 message formats.
        }

        /// <summary>
        /// Specifies the protocol the message belongs to.
        /// </summary>
        public ProtocolEnum Protocol { get; init; } = Protocol;

        /// <summary>
        /// Specifies the message that was received.
        /// </summary>
        public List<byte> Message { get; init; } = Message;
    }

    public sealed class SecureMsgReceiveResponse(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        SecureMsgReceiveResponse.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        public enum ErrorCodeEnum
        {
            AccessDenied, //The encryption module is either not initialized or not ready for any vendor-specific reason.
            HSMStateInvalid, //The HSM is not in a correct state to handle this message.
            MACInvalid, //The MAC of the message is not correct.
            ProtocolInvalid, //The specified protocol is invalid.
            FormatInvalid, //The format of the message is invalid.
            ContentInvalid, //The contents of one of the security relevant properties are invalid.
            KeyNotFound, //No key was found for PAC/MAC generation.
        }

        /// <summary>
        /// Specifies the error code.
        /// </summary>
        public ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }
}

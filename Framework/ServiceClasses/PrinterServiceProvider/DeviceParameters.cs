/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Printer
{
    public enum CodelineFormatEnum
    {
        CMC7,
        E13B,
    }

    public enum BlackMarkModeEnum
    {
        On,
        Off
    }

    public enum PaperSourceEnum
    {
        Default,
        Upper,
        Lower,
        External,
        AUX,
        AUX2,
        Park,
    }

    public enum CommonThresholdStatusEnum
    {
        Full,
        Low,
        Out
    }

    public enum RetractThresholdStatusEnum
    {
        Full,
        High,
        OK
    }

    public enum LampThresholdEnum
    {
        Ok,
        Fading,
        Inop
    }

    public enum PhysicalBinStatusEnum
    {
        Inserted,
        Removed
    }

    public enum AutoRetractResultEnum
    {
        Ok,
        Jammed
    }

    public enum PositionEnum
    {
        Retracted,
        Present,
        Entering,
        Jammed,
        Unknown,
        Expelled
    }

    public sealed class ControlMediaRequest
    {
        /// <summary>
        /// ControlMediaRequest
        /// Request action to the device to control media
        /// </summary>
        public ControlMediaRequest(PrinterCapabilitiesClass.ControlEnum Controls)
        {
            this.Controls = Controls;
        }

        public PrinterCapabilitiesClass.ControlEnum Controls { get; init; }
    }

    public sealed class ControlMediaResult : DeviceResult
    {
        public enum ErrorCodeEnum
        {
            NoMediaPresent,
            FlushFail,
            RetractBinFull,
            StackerFull,
            PageTurnFail,
            MediaTurnFail,
            ShutterFail,
            MediaJammed,
            PaperJammed,
            PaperOut,
            InkOut,
            TonerOut,
            SequenceInvalid,
            MediaRetained,
            BlackMark,
            MediaRetracted
        }

        /// <summary>
        /// ControlMediaResult
        /// Return result of controlling media.
        /// </summary>
        public ControlMediaResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                  string ErrorDescription = null,
                                  ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on control media.
        /// </summary>
        public ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed class ControlPassbookRequest
    {

        public enum ActionEnum
        {
            TurnForward,
            TurnBackward,
            CloseForward,
            CloseBackward
        }
        /// <summary>
        /// This command can turn the pages of a passbook inserted in the printer by a specified number of pages in a
        /// specified direction and it can close the passbook.
        /// </summary>
        public ControlPassbookRequest(ActionEnum Action,
                                      int Count)
        {
            this.Action = Action;
            this.Count = Count;
        }

        /// <summary>
        /// Specifies the direction of the page turn
        /// </summary>
        public ActionEnum Action { get; init; }

        /// <summary>
        ///  Specifies the number of pages to be turned.
        /// </summary>
        public int Count { get; init; }
    }

    public sealed class ControlPassbookResult : DeviceResult
    {
        /// <summary>
        /// ControlPassbookResult
        /// Return result of controlling passbook.
        /// </summary>
        public ControlPassbookResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                     string ErrorDescription = null,
                                     ControlPassbookCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on control passbook.
        /// </summary>
        public ControlPassbookCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed class DispensePaperRequest
    {
        /// <summary>
        /// Move paper (which can also be a new passbook) from a paper source into the print position
        /// </summary>
        public DispensePaperRequest(PaperSourceEnum? Source,
                                    string CustomSource = null)
        {
            this.Source = Source;
            this.CustomSource = CustomSource;
        }

        /// <summary>
        /// Specifies the direction of the page turn
        /// </summary>
        public PaperSourceEnum? Source { get; init; }

        /// <summary>
        /// Vendor specific paper source
        /// </summary>
        public string CustomSource { get; init; }
    }

    public sealed class DispensePaperResult : DeviceResult
    {
        /// <summary>
        /// DispensePaperResult
        /// Return result of moving paper.
        /// </summary>
        public DispensePaperResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                   string ErrorDescription = null,
                                   DispensePaperCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on moving paper.
        /// </summary>
        public DispensePaperCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed class AcceptAndReadImageRequest
    {
        public enum SourceTypeEnum
        {
            Front,
            Back,
            Codeline,
        }

        public sealed class ReadData
        {
            public enum DataFormatEnum
            {
                TIF,
                WMF,
                BMP,
                JPG,
                CMC7,
                E13B,
                OCR
            }

            public enum ColorFormatEnum
            {
                None,
                Binary,
                Grayscale,
                Fullcolor,
            }

            public ReadData(SourceTypeEnum DataType,
                            DataFormatEnum DataFormat,
                            ColorFormatEnum ColorFormat)
            {
                this.DataType = DataType;
                this.DataFormat = DataFormat;
                this.ColorFormat = ColorFormat;
            }

            /// <summary>
            /// Data type to read
            /// </summary>
            public SourceTypeEnum DataType { get; init; }

            /// <summary>
            /// Data format to read
            /// </summary>
            public DataFormatEnum DataFormat { get; init; }

            /// <summary>
            /// Color to use
            /// </summary>
            public ColorFormatEnum ColorFormat { get; init; }
        }

        /// <summary>
        /// Accept media and read data
        /// </summary>
        public AcceptAndReadImageRequest(Dictionary<SourceTypeEnum, ReadData> DataToRead)
        {
            this.DataToRead = DataToRead;
        }

        /// <summary>
        /// Specifies the data to be read
        /// </summary>
        public Dictionary<SourceTypeEnum, ReadData> DataToRead { get; init; }
    }

    public sealed class AcceptAndReadImageResult : DeviceResult
    {
        public sealed class DataRead
        {
            public enum StatusEnum
            {
                Ok,
                NotSupported,
                Missing
            }

            public DataRead(StatusEnum Status,
                            List<byte> Data)
            {
                this.Status = Status;
                this.Data = Data;
            }

            /// <summary>
            /// Status of result of reading data
            /// </summary>
            public StatusEnum Status { get; init; }
            /// <summary>
            /// Data read
            /// </summary>
            public List<byte> Data { get; init; }
        }

        /// <summary>
        /// AcceptAndReadImageResult
        /// Return result of reading image
        /// </summary>
        public AcceptAndReadImageResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                        string ErrorDescription = null,
                                        ReadImageCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Data = null;
        }
        public AcceptAndReadImageResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                        Dictionary<AcceptAndReadImageRequest.SourceTypeEnum, DataRead> Data)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Data = Data;
        }

        /// <summary>
        /// Specifies the error code on reading image
        /// </summary>
        public ReadImageCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Store data read.
        /// </summary>
        public Dictionary<AcceptAndReadImageRequest.SourceTypeEnum, DataRead> Data { get; init; }
    }

    public sealed class ResetDeviceRequest
    {

        public enum MediaControlEnum
        {
            Default,
            Eject,
            Retract,
            Expel
        }
        /// <summary>
        /// Move paper (which can also be a new passbook) from a paper source into the print position
        /// </summary>
        public ResetDeviceRequest(MediaControlEnum MediaControl,
                                  int RetractBin)
        {
            this.MediaControl = MediaControl;
            this.RetractBin = RetractBin;
        }

        /// <summary>
        /// Specifies the media action while in reset operation
        /// </summary>
        public MediaControlEnum MediaControl { get; init; }

        /// <summary>
        /// Retract bin number to retract
        /// If this value is -1, device class to move default location
        /// </summary>
        public int RetractBin { get; init; }
    }

    public sealed class ResetDeviceResult : DeviceResult
    {
        /// <summary>
        /// ResetResult
        /// Return result of reset operation
        /// </summary>
        public ResetDeviceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 string ErrorDescription = null,
                                 ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on reset operation
        /// </summary>
        public ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed class RetractResult : DeviceResult
    {
        /// <summary>
        /// RetractResult
        /// Return result of retract operation
        /// </summary>
        public RetractResult(MessagePayload.CompletionCodeEnum CompletionCode,
                             string ErrorDescription = null,
                             RetractMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.BinNumber = null;
        }

        public RetractResult(MessagePayload.CompletionCodeEnum CompletionCode,
                             int BinNumber)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.BinNumber = BinNumber;
        }

        /// <summary>
        /// Specifies the error code on retract operation
        /// </summary>
        public RetractMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// The number of the retract bin where the media has actually been deposited.
        /// </summary>
        public int? BinNumber { get; init; }
    }

    public sealed class PrintFormResult : DeviceResult
    {
        /// <summary>
        /// RetractResult
        /// Return result of retract operation
        /// </summary>
        public PrintFormResult(MessagePayload.CompletionCodeEnum CompletionCode,
                               string ErrorDescription = null,
                               PrintFormCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code for print form command
        /// </summary>
        public PrintFormCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed class SupplyReplenishedRequest
    {
        [Flags]
        public enum SupplyEnum
        {
            NotSupported = 0,
            AUX = 0x0001,
            AUX2 = 0x0002,
            Upper = 0x0004,
            Lower = 0x0008,
            Ink = 0x0010,
            Toner = 0x0020,
            Lamp = 0x0040,
        }
        /// <summary>
        /// Replenish media
        /// </summary>
        public SupplyReplenishedRequest(SupplyEnum Supplies)
        {
            this.Supplies = Supplies;
        }

        /// <summary>
        /// Specifies supplies to replenish
        /// </summary>
        public SupplyEnum Supplies { get; init; }
    }

    public sealed class PrintTaskRequest
    {
        public PrintTaskRequest(PrintJobClass PrintJob,
                                PaperSourceEnum? Source,
                                string CustomSource = null)
        {
            this.PrintJob = PrintJob;
            this.Source = Source;
            this.CustomSource = CustomSource;
        }

        /// <summary>
        /// Print jobs asking the device class to print
        /// </summary>
        public PrintJobClass PrintJob { get; init; }
        /// <summary>
        /// Specifies the direction of the page turn
        /// </summary>
        public PaperSourceEnum? Source { get; init; }

        /// <summary>
        /// Vendor specific paper source
        /// </summary>
        public string CustomSource { get; init; }
    }

    public sealed class PrintTaskResult : DeviceResult
    {
        /// <summary>
        /// RetractResult
        /// Return result of retract operation
        /// </summary>
        public PrintTaskResult(MessagePayload.CompletionCodeEnum CompletionCode,
                               string ErrorDescription = null,
                               PrintFormCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on print form operation
        /// </summary>
        public PrintFormCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed class DirectFormPrintRequest
    {
        public DirectFormPrintRequest(string FormName,
                                      string MediaName,
                                      Dictionary<string, string> Fields,
                                      PaperSourceEnum? Source,
                                      string CustomSource = null)
        {
            this.FormName = FormName;
            this.MediaName = MediaName;
            this.Fields = Fields;
            this.Source = Source;
            this.CustomSource = CustomSource;
        }

        /// <summary>
        /// Form name to be used
        /// </summary>
        public string FormName { get; init; }
        /// <summary>
        /// Media name to be used
        /// </summary>
        public string MediaName { get; init; }
        /// <summary>
        /// Dynamic field values to be printed
        /// </summary>
        public Dictionary<string, string> Fields { get; init; }

        /// <summary>
        /// Specifies the direction of the page turn
        /// </summary>
        public PaperSourceEnum? Source { get; init; }

        /// <summary>
        /// Vendor specific paper source
        /// </summary>
        public string CustomSource { get; init; }
    }

    public sealed class RawPrintRequest
    {
        public RawPrintRequest(bool Input,
                               List<byte> PrintData)
        {
            this.Input = Input;
            this.PrintData = PrintData;
        }

        /// <summary>
        /// If ture. input data from the device is expected in response to sending the raw data, otherwise false
        /// </summary>
        public bool Input { get; init; }
        /// <summary>
        /// Dynamic field values to be printed
        /// </summary>
        public List<byte> PrintData { get; init; }
    }

    public sealed class RawPrintResult : DeviceResult
    {
        /// <summary>
        /// RetractResult
        /// Return result of retract operation
        /// </summary>
        public RawPrintResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              PrintRawCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Data = null;
        }
        public RawPrintResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              List<byte> Data)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Data = Data;
        }

        /// <summary>
        /// Specifies the error code on print raw data operation
        /// </summary>
        public PrintRawCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Data received from the device
        /// </summary>
        public List<byte> Data { get; init; }
    }
}


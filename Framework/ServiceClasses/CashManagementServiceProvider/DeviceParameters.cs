/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CashManagement
{
    /// <summary>
    /// This field is used if items are to be moved to internal areas of the device, including cash units, 
    /// the intermediate stacker, or the transport.
    /// </summary>
    public class Retract
    {
        public CashManagementCapabilitiesClass.RetractAreaEnum RetractArea { get; init; }

        /// <summary>
        /// Index is valid if the RetractArea is set to Retract, otherwise this value can be omitted
        /// </summary>
        public int? Index { get; init; }

        public Retract(CashManagementCapabilitiesClass.RetractAreaEnum RetractArea,
                       int? Index = null)
        {
            this.RetractArea = RetractArea;
            this.Index = Index;
        }
    }

    public sealed class ItemPosition
    {
        /// <summary>
        /// ItemPosition
        /// Specifies where the dispensed items should be moved to
        /// </summary>
         public ItemPosition(string CashUnit)
        {
            this.CashUnit = CashUnit;
            this.RetractArea = null;
            this.OutputPosition = null;
        }
        public ItemPosition(Retract RetractArea)
        {
            this.CashUnit = string.Empty;
            this.RetractArea = RetractArea;
            this.OutputPosition = null;
        }
        public ItemPosition(CashManagementCapabilitiesClass.PositionEnum? OutputPosition)
        {
            this.CashUnit = string.Empty;
            this.RetractArea = null;
            this.OutputPosition = OutputPosition;
        }

        /// <summary>
        /// This value specifies the name of the single cash unit to be used for the storage of any items found.
        /// </summary>
        public string CashUnit { get; init; }
        /// <summary>
        /// This field is used if items are to be moved to internal areas of the device, including cash units, 
        /// the intermediate stacker, or the transport.
        /// </summary>
        public Retract RetractArea { get; init; }
        /// <summary>
        /// The output position to which items are to be moved if the RetractArea is specified to OutputPosition.
        /// Following values are possible:
        /// 
        /// * ```OutDefault``` - The default configuration.
        /// * ```OutLeft``` - The left output position.
        /// * ```OutRight``` - The right output position.
        /// * ```OutCenter``` - The center output position.
        /// * ```OutTop``` - The top output position.
        /// * ```OutBottom``` - The bottom output position.
        /// * ```OutFront``` - The front output position.
        /// * ```OutRear``` - The rear output position.
        /// </summary>
        public CashManagementCapabilitiesClass.PositionEnum? OutputPosition { get; init; }
    }

    /// <summary>
    /// UnlockSafeResult
    /// Request for unlocking safe door.
    /// </summary>
    public sealed class UnlockSafeResult : DeviceResult
    {
        public UnlockSafeResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                      string ErrorDescription = null,
                                      OpenSafeDoorCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        { }

        /// <summary>
        /// Specifies the error code on end exchange
        /// </summary>
        public OpenSafeDoorCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    /// <summary>
    /// CalibrateCashUnitRequest
    /// Request to perform calibration of cash unit
    /// </summary>
    public sealed class CalibrateCashUnitRequest
    {
        public CalibrateCashUnitRequest(string CashUnit,
                                        int NumOfBills,
                                        ItemPosition Position)
        {
            this.CashUnit = CashUnit;
            this.NumOfBills = NumOfBills;
            this.Position = Position;
        }

        /// <summary>
        /// The object name of the cash unit where items to be dispensed
        /// </summary>
        public string CashUnit { get; init; }

        /// <summary>
        /// The number of bills to be dispensed during the calibration process.
        /// </summary>
        public int NumOfBills { get; init; }

        /// <summary>
        /// Specifies where the dispensed items should be moved to
        /// </summary>
        public ItemPosition Position { get; init; }
    }

    /// <summary>
    /// CalibrateCashUnitResult
    /// Return result of calibration for cash unit
    /// </summary>
    public sealed class CalibrateCashUnitResult : DeviceResult
    {
        /// <summary>
        /// CalibrateCashUnitResult
        /// Return result of calibration for cash unit
        /// </summary>
        /// <param name="CompletionCode"></param>
        /// <param name="ErrorDescription"></param>
        /// <param name="ErrorCode"></param>
        public CalibrateCashUnitResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                       string ErrorDescription = null,
                                       CalibrateCashUnitCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Position = null;
            this.MovementResult = null;
        }

        public CalibrateCashUnitResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                       ItemPosition Position,
                                       Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Position = Position;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on calibration
        /// </summary>
        public CalibrateCashUnitCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }


        /// <summary>
        /// Specifies where the dispensed items should be moved to
        /// </summary>
        public ItemPosition Position { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in calibration.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// OpenCloseShutterRequest
    /// Open or Close shutter for the specified output position
    /// </summary>
    public sealed class OpenCloseShutterRequest
    {
        public enum ActionEnum
        {
            Open,
            Close
        }

        /// <summary>
        /// OpenCloseShutterRequest
        /// Open or Close shutter for the specified output position
        /// </summary>
        /// <param name="Action">Either Open or Close for the shutter operation</param>
        /// <param name="ShutterPosition">Postion of shutter to control.</param>
        public OpenCloseShutterRequest(ActionEnum Action, CashManagementCapabilitiesClass.PositionEnum ShutterPosition)
        {
            this.Action = Action;
            this.ShutterPosition = ShutterPosition;
        }

        public ActionEnum Action { get; init; }

        public CashManagementCapabilitiesClass.PositionEnum ShutterPosition { get; init; }
    }

    /// <summary>
    /// OpenCloseShutterResult
    /// Return result of shutter operation.
    /// </summary>
    public sealed class OpenCloseShutterResult : DeviceResult
    {
        public OpenCloseShutterResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                      string ErrorDescription,
                                      ErrorCodeEnum? ErrorCode,
                                      bool Jammed)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Jammed = Jammed;
        }

        public OpenCloseShutterResult(MessagePayload.CompletionCodeEnum CompletionCode)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Jammed = false;
        }

        public enum ErrorCodeEnum
        {
            UnsupportedPosition,
            ShutterNotOpen,
            ShutterOpen,
            ShutterClosed,
            ShutterNotClosed,
            ExchangeActive,
        }

        /// <summary>
        /// Specifies the error code on closing or opening shutter
        /// </summary>
        public ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// If the shutter is not closed and jammed
        /// </summary>
        public bool Jammed { get; init; }
    }

    /// <summary>
    /// ResetDeviceRequest
    /// The parameter class for the reset device operation
    /// </summary>
    public sealed class ResetDeviceRequest
    {
        /// <summary>
        /// ResetRequest
        /// The parameter class for the reset device operation
        /// </summary>
        public ResetDeviceRequest(ItemPosition Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Specifies where the dispensed items should be moved to.
        /// If tnis value is null, the retract items to the default position.
        /// </summary>
        public ItemPosition Position { get; init; }
    }

    /// <summary>
    /// ResetDeviceResult
    /// Return result of reset device
    /// </summary>
    public sealed class ResetDeviceResult : DeviceResult
    {
        /// <summary>
        /// ResetDeviceResult
        /// Return result of reset device
        /// </summary>
        public ResetDeviceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 string ErrorDescription = null,
                                 ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                                 Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.MovementResult = MovementResult;
        }

        public ResetDeviceResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 Dictionary<string, CashUnitCountClass> MovementResult)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public ResetCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in reset operation.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// RetractRequest
    /// The parameter class for the retract operation
    /// </summary>
    public sealed class RetractRequest
    {
        /// <summary>
        /// ResetRequest
        /// The parameter class for the retract operation
        /// </summary>
        public RetractRequest(ItemPosition Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Specifies where the dispensed items should be moved to.
        /// If tnis value is null, the retract items to the default position
        /// </summary>
        public ItemPosition Position { get; init; }
    }

    /// <summary>
    /// RetractResult
    /// Return result of retract items
    /// </summary>
    public sealed class RetractResult : DeviceResult
    {
        /// <summary>
        /// ResetDeviceResult
        /// Return result of retract items
        /// </summary>
        public RetractResult(MessagePayload.CompletionCodeEnum CompletionCode,
                             string ErrorDescription = null,
                             RetractCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                             Dictionary<string, CashUnitCountClass> MovementResult = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.MovementResult = MovementResult;
        }

        public RetractResult(MessagePayload.CompletionCodeEnum CompletionCode,
                             Dictionary<string, CashUnitCountClass> MovementResult)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on reset device
        /// </summary>
        public RetractCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the detailed note movement while in reset operation.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// GetTellerInfoRequest
    /// The parameter class for get teller info from the  device
    /// </summary>
    public sealed class GetTellerInfoRequest
    {
        /// <summary>
        /// ResetRequest
        /// The parameter class for the reset device operation
        /// </summary>
        public GetTellerInfoRequest(int TellerId,
                                    string CurrencyId)
        {
            this.TellerId = TellerId;
            this.CurrencyId = CurrencyId;
        }

        /// <summary>
        /// Identification of the teller. If invalid the error InvalidTellerId is reported. If it is negative value specified, all
        /// </summary>
        public int TellerId { get; init; }

        /// <summary>
        /// Three character ISO 4217 format currency identifier. If not specified, all currencies are reported for TellerID.
        /// </summary>
        public string CurrencyId { get; init; }
    }

    /// <summary>
    /// Details of the teller information
    /// </summary>
    public sealed class TellerDetail
    {
        public TellerDetail(int TellerId,
                            CashManagementCapabilitiesClass.PositionEnum InputPosition,
                            CashManagementCapabilitiesClass.PositionEnum OutputPosition,
                            Dictionary<string, TellerTotal> Totals)
        {
            this.TellerId = TellerId;
            this.InputPosition = InputPosition;
            this.OutputPosition = OutputPosition;
            this.Totals = Totals;
        }

        public sealed class TellerTotal
        {
            /// <summary>
            /// teller totals
            /// </summary>
            public TellerTotal(double ItemsReceived,
                               double ItemsDispensed,
                               double CoinsReceived,
                               double CoinsDispensed,
                               double CashBoxReceived,
                               double CashBoxDispensed)
            {
                this.ItemsReceived = ItemsReceived;
                this.ItemsDispensed = ItemsDispensed;
                this.CoinsReceived = CoinsReceived;
                this.CoinsDispensed = CoinsDispensed;
                this.CashBoxReceived = CashBoxReceived;
                this.CashBoxDispensed = CashBoxDispensed;
            }

            /// <summary>
            /// The total absolute value of items(other than coins) of the specified currency accepted.
            /// The amount is expressed as a floating point value.
            /// </summary>
            public double ItemsReceived { get; init; }

            /// <summary>
            /// The total absolute value of items (other than coins) of the specified currency dispensed.
            /// The amount is expressed as a floating point value.
            /// </summary>
            public double ItemsDispensed { get; init; }

            /// <summary>
            /// The total absolute value of coin currency accepted. 
            /// The amount is expressed as a floating point value.
            /// </summary>
            public double CoinsReceived { get; init; }

            /// <summary>
            /// The total absolute value of coin currency dispensed. 
            /// The amount is expressed as a floating point value.
            /// </summary>
            public double CoinsDispensed { get; init; }

            /// <summary>
            /// The total absolute value of cash box currency accepted.
            /// The amount is expressed as a floating point value.
            /// </summary>
            public double CashBoxReceived { get; init; }

            /// <summary>
            /// The total absolute value of cash box currency dispensed. 
            /// The amount is expressed as a floating point value.
            /// </summary>
            public double CashBoxDispensed { get; init; }
        }
        
        /// <summary>
        /// Teller ID
        /// </summary>
        public int TellerId { get; init; }
        /// <summary>
        /// The input position assigned to the teller for cash entry
        /// </summary>
        public CashManagementCapabilitiesClass.PositionEnum InputPosition { get; init; }

        /// <summary>
        /// The output position from which cash is presented to the teller
        /// </summary>
        public CashManagementCapabilitiesClass.PositionEnum OutputPosition { get; init; }

        /// <summary>
        /// List of teller totals
        /// </summary>
        public Dictionary<string, TellerTotal> Totals { get; init; }
    }

    public sealed class GetTellerInfoResult : DeviceResult
    {
        /// <summary>
        /// GetTellerInfoResult
        /// Return result of teller info inquired
        /// </summary>
        public GetTellerInfoResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                   string ErrorDescription = null,
                                   GetTellerInfoCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
                                   List<TellerDetail> Details = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Details = Details;
        }

        public GetTellerInfoResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                   List<TellerDetail> Details = null)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Details = Details;
        }

        /// <summary>
        /// Specifies the error code on getting teller info
        /// </summary>
        public GetTellerInfoCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the details of the teller information required
        /// </summary>
        public List<TellerDetail> Details { get; init; }
    }

    public sealed class SetTellerInfoRequest
    {
        public enum ActionEnum
        {
            Create,
            Modify,
            Delete,
        }

        /// <summary>
        /// ResetRequest
        /// The parameter class for the reset device operation
        /// </summary>
        public SetTellerInfoRequest(ActionEnum Action,
                                    TellerDetail Detail)
        {
            this.Action = Action;
            this.Detail = Detail;
        }

        /// <summary>
        /// The action to be performed.
        /// </summary>
        public ActionEnum Action { get; init; }

        /// <summary>
        /// Specifies the details of the teller information to be set
        /// </summary>
        public TellerDetail Detail { get; init; }
    }

    public sealed class SetTellerInfoResult : DeviceResult
    {
        /// <summary>
        /// SetTellerInfoResult
        /// Return result of setting teller info inquired
        /// </summary>
        public SetTellerInfoResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                   string ErrorDescription = null,
                                   SetTellerInfoCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on setting teller info
        /// </summary>
        public SetTellerInfoCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }
}

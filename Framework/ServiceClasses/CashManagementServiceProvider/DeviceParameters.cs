/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
using static System.Runtime.InteropServices.JavaScript.JSType;
using static XFS4IoT.Configurations;
using static XFS4IoTFramework.Common.LightsCapabilitiesClass;
using System.Collections;
using System.Reflection;

namespace XFS4IoTFramework.CashManagement
{
    public enum NoteLevelEnum
    {
        All,
        Unrecognized,
        Counterfeit,
        Suspect,
        Fit,
        Unfit,
        Inked,
    }

    public enum OrientationEnum
    {
        NotSupported,
        FrontTop,
        FrontBottom,
        BackTop,
        BackBottom,
        Unknown,
    }

    public enum ClassificationListEnum
    {
        NotSupported,
        OnClassificationList,
        NotOnClassificationList,
        ClassificationListUnknown,
    }

    // Target desitnation item to be moved in reset operation
    public enum ItemTargetEnum
    {
        SingleUnit,
        Retract,
        Transport,
        Stacker,
        Reject,
        ItemCassette,
        CashIn,
        OutDefault,
        OutLeft,
        OutRight,
        OutCenter,
        OutTop,
        OutBottom,
        OutFront,
        OutRear,
        Default,
    }

    [Flags]
    public enum ItemInfoTypeEnum
    {
        All = 0,
        SerialNumber = 1 << 0,
        Signature = 1 << 1,
        Image = 1 << 2,
    }

    /// <summary>
    /// ItemInfoSummary
    /// Store information for the item type and counts
    /// </summary>
    public sealed record ItemInfoSummary
    {
        public ItemInfoSummary(NoteLevelEnum Level, int NumOfItems)
        {
            this.Level = Level;
            this.NumOfItems = NumOfItems;
        }

        /// <summary>
        /// Level of item
        /// </summary>
        public NoteLevelEnum Level { get; init; }

        /// <summary>
        /// Number of items classified as level which have information available.
        /// </summary>
        public int NumOfItems { get; init; }

    }

    /// <summary>
    /// This field is used if items are to be moved to internal areas of the device, including cash units, 
    /// the intermediate stacker, or the transport.
    /// </summary>
    public sealed record Retract
    {
        public Retract(CashManagementCapabilitiesClass.RetractAreaEnum RetractArea,
                       int? Index = null)
        {
            this.RetractArea = RetractArea;
            this.Index = Index;
        }

        public CashManagementCapabilitiesClass.RetractAreaEnum RetractArea { get; init; }

        /// <summary>
        /// Index is valid if the RetractArea is set to Retract, otherwise this value can be omitted
        /// </summary>
        public int? Index { get; init; }
    }

    public sealed record RetractPosition
    {
        /// <summary>
        /// RetractPosition
        /// Specifies where the dispensed items should be moved to for retract command
        /// </summary>
        public RetractPosition(string CashUnit)
        {
            this.CashUnit = CashUnit;
            RetractArea = null;
            OutputPosition = null;
        }
        public RetractPosition(Retract RetractArea)
        {
            CashUnit = string.Empty;
            this.RetractArea = RetractArea;
            OutputPosition = null;
        }
        public RetractPosition(CashManagementCapabilitiesClass.PositionEnum? OutputPosition)
        {
            CashUnit = string.Empty;
            RetractArea = null;
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

    public sealed record ItemDestination
    {
        /// <summary>
        /// ItemDestination
        /// Specifies where the dispensed items should be moved to.
        /// </summary>
        public ItemDestination()
        { }
        public ItemDestination(string CashUnit)
        {
            this.CashUnit = CashUnit;
            Destination = ItemTargetEnum.SingleUnit;
        }
        public ItemDestination(ItemTargetEnum Destination, int IndexOfRetractUnit = 1)
        {
            this.Destination = Destination;
            this.IndexOfRetractUnit = IndexOfRetractUnit;
        }

        /// <summary>
        /// This property specifies the name of the single cash unit to be used for the storage of any items found.
        /// The Destination property must be SingleUnit otherwise it is ingored.
        /// </summary>
        public string CashUnit { get; init; } = string.Empty;

        /// <summary>
        /// The output position to which items are to be moved.
        /// 
        ///  * ```SingleUnit``` - Move the items to a single storage unit.
        ///  * ```Retract``` - Move the items to a retract storage unit.
        ///  * ```Stacker``` - Move the items to the intermediate stacker area.
        ///  * ```Reject``` - Move the items to a reject storage unit.
        ///  * ```ItemCassette``` - Move the items to the storage units which would be used during a Cash In transaction including recycling storage units.
        ///  * ```CashIn``` - Move the items to the storage units which would be used during a Cash In transaction but not including recycling storage units.
        ///  * ```OutDefault``` - Default output position.
        ///  * ```OutLeft``` - Left output position.
        ///  * ```OutRight``` - Right output position.
        ///  * ```OutCenter``` - Center output position.
        ///  * ```OutTop``` - Top output position.
        ///  * ```OutBottom``` - Bottom output position.
        ///  * ```OutFront``` - Front output position.
        ///  * ```OutRear``` - Rear output position.
        ///  * ```Default``` - Default position.
        /// </summary>
        public ItemTargetEnum Destination { get; init; } = ItemTargetEnum.Default;

        /// <summary>
        /// If Destination property is set to Retract this property defines the position inside the retract storage units into
        /// which the cash is to be retracted. IndexOfRetractUnit starts with a value of 1 for the first retract position
        /// and increments by one for each subsequent position.If there are several retract storage units
        /// (of type *retractCassette* in [Storage.GetStorage](#storage.getstorage)), IndexOfRetractUnit would be incremented from the
        /// first position of the first retract storage unit to the last position of the last retract storage unit.
        /// The maximum value of IndexOfRetractUnit is the sum of maximum of each retract storage unit. If Destination property is not
        /// set to Retract the value of this property is not used.
        /// </summary>
        public int IndexOfRetractUnit { get; init; } = 1;
    }

    public sealed record ItemInfoClass
    {
        public ItemInfoClass(OrientationEnum Orientation, 
                             List<byte> Signature, 
                             NoteLevelEnum Level, 
                             string SerialNumber, 
                             List<byte> Image, 
                             ClassificationListEnum ClassificationList,
                             string ItemLocation)
        {
            this.Orientation = Orientation;
            this.Signature = new(Signature);
            this.Level = Level;
            this.SerialNumber = SerialNumber;
            this.Image = new(Image);
            this.ClassificationList = ClassificationList;
            this.ItemLocation = ItemLocation;
        }

        /// <summary>
        /// Orientation of the item
        /// </summary>
        public OrientationEnum Orientation { get; init; }

        /// <summary>
        /// Vendor specific signature data.
        /// </summary>
        public List<byte> Signature { get; init; }

        /// <summary>
        /// Level of note
        /// </summary>
        public NoteLevelEnum Level { get; init; }

        /// <summary>
        /// This property contains the serial number of the item as a string. A '?' character is used 
        /// to represent any serial number character that cannot be recognized. If no serial number is available or 
        /// has not been requested then this is omitted.
        /// </summary>
        public string SerialNumber { get; init; }

        /// <summary>
        /// Binary image data
        /// </summary>
        public List<byte> Image { get; init; }

        /// <summary>
        /// Specifies if the item is on the classification list. If the classification list reporting capability is not 
        /// supported this property can be set to NotSupported
        /// </summary>
        public ClassificationListEnum ClassificationList { get; init; }

        /// <summary>
        /// Specifies the location of the item. Following values are possible:
        /// 
        /// * ```customer``` - The item has been presented to the customer.
        /// * ```unknown``` - The item location is unknown, for example, it may have been removed manually.
        /// * ```stacker``` - The item is in the intermediate stacker.
        /// * ```output``` - The item is at the output position. The items have not been in customer access.
        /// * ```transport``` - The item is in an intermediate location in the device.
        /// * ```deviceUnknown``` - The item is in the device but its location is unknown.
        /// * ```storage unit identifier``` - The item is in a storage unit with matching
        /// </summary>
        public string ItemLocation { get; init; }
    }

    /// <summary>
    /// CalibrateCashUnitRequest
    /// Request to perform calibration of cash unit
    /// </summary>
    public sealed class CalibrateCashUnitRequest
    {
        public CalibrateCashUnitRequest(string CashUnit,
                                        int NumOfBills,
                                        ItemDestination Position)
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
        public ItemDestination Position { get; init; }
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
                                       ItemDestination Position,
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
        public ItemDestination Position { get; init; }

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
        /// <param name="ShutterPosition">Position of shutter to control.</param>
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
        public ResetDeviceRequest(ItemDestination Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Specifies where the dispensed items should be moved to.
        /// </summary>
        public ItemDestination Position { get; init; }
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
        public RetractRequest(RetractPosition Position)
        {
            this.Position = Position;
        }

        /// <summary>
        /// Specifies where the dispensed items should be moved to.
        /// If tnis value is null, the retract items to the default position
        /// </summary>
        public RetractPosition Position { get; init; }
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

    public sealed class GetItemInfoRequest
    {
        /// <summary>
        /// GetItemInfoRequest
        /// Which item information to be retrieved.
        /// </summary>
        public GetItemInfoRequest(int Index,
                                  NoteLevelEnum NoteLevel,
                                  ItemInfoTypeEnum ItemInfoType)
        {
            this.Index = Index;
            this.ItemInfoType = ItemInfoType;
        }

        /// <summary>
        /// The index being used for sending InfoAvailableEvent.
        /// If the NoteLevel is specified to zero, this value is ignored.
        /// </summary>
        public int Index { get; init; }

        /// <summary>
        /// Note level to be reported. if the value is zero, all types are reported.
        /// </summary>
        public NoteLevelEnum NoteLevel { get; init; }

        /// <summary>
        /// Specifies the type of information required. if the value is zero, all types to be reported
        /// </summary>
        public ItemInfoTypeEnum ItemInfoType { get; init; }
    }

    public sealed class GetItemInfoResult : DeviceResult
    {
        /// <summary>
        /// GetitemInfoResult
        /// Return item infomation requested by the client.
        /// </summary>
        public GetItemInfoResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 string ErrorDescription = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ItemInfos = null;
        }

        public GetItemInfoResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                 Dictionary<string, ItemInfoClass> ItemInfos)
            : base(CompletionCode, null)
        {
            this.ItemInfos = ItemInfos;
        }

        /// <summary>
        /// Reported requested item information.
        /// Key is note type Id.
        /// </summary>
        public Dictionary<string, ItemInfoClass> ItemInfos { get; init; }
    }
}

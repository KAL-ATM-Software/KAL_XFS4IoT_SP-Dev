/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.

\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.CashManagement;
using XFS4IoT;

namespace XFS4IoTFramework.CashAcceptor
{
    /// <summary>
    /// Class representing signature information
    /// </summary>
    public sealed class SignatureInfo(
        string NoteType,
        OrientationEnum Orientation,
        List<byte> Signature)
    {

        /// <summary>
        /// Note type identified.
        /// </summary>
        public string NoteType { get; init; } = NoteType;

        /// <summary>
        /// Orientation of notes scanned
        /// </summary>
        public OrientationEnum Orientation { get; init; } = Orientation;

        /// <summary>
        /// Base64 encoded vendor specific signature data. 
        /// </summary>
        public List<byte> Signature { get; init; } = Signature;
    }

    public sealed class DepleteOperationResult(
        int TotalNumberOfItemsReceived,
        int TotalNumberOfItemsRejected,
        Dictionary<string, DepleteOperationResult.DepleteSourceResult> SourceResults)
    {
        public sealed class DepleteSourceResult(
            string CashItem, 
            int NumberOfItemsRemoved)
        {

            /// <summary>
            /// A cash item as reported by CashManagement.GetBankNoteTypes. This is not 
            /// specified if the item was not identified as a cash item.
            /// </summary>
            public string CashItem { get; init; } = CashItem;

            /// <summary>
            /// Total number of items removed from this source storage unit of the item type. 
            /// Not reported if this source storage unit did not move any items of this item type, 
            /// for example due to a storage unit or transport jam.
            /// </summary>
            public int NumberOfItemsRemoved { get; init; } = NumberOfItemsRemoved;

        }

        /// <summary>
        /// Total number of items received in the target storage unit during execution of this operation.
        /// </summary>
        public int TotalNumberOfItemsReceived { get; init; } = TotalNumberOfItemsReceived;

        /// <summary>
        /// Total number of items rejected during execution of this operation.
        /// </summary>
        public int TotalNumberOfItemsRejected { get; init; } = TotalNumberOfItemsRejected;

        /// <summary>
        /// Specified result of the cash movement.
        /// Key - Name of the storage unit Id from which items have been removed.
        /// </summary>
        public Dictionary<string, DepleteSourceResult> SourceResults { get; init; } = SourceResults;
    }

    public sealed class ReplenishOperationResult(
        int NumberOfItemsRemoved,
        int TotalNumberOfItemsRejected,
        Dictionary<string, ReplenishOperationResult.ReplenishTargetResult> TargetResults)
    {
        public sealed class ReplenishTargetResult(
            string CashItem, 
            int NumberOfItemsReceived)
        {

            /// <summary>
            /// Name of the storage unit to which items have been moved.
            /// </summary>
            public string CashItem { get; init; } = CashItem;

            /// <summary>
            /// Total number of items received in this target storage unit of the item type.
            /// </summary>
            public int NumberOfItemsReceived { get; init; } = NumberOfItemsReceived;

        }

        /// <summary>
        /// Total number of items removed from the source storage unit including rejected items during execution of this operation.
        /// </summary>
        public int NumberOfItemsRemoved { get; init; } = NumberOfItemsRemoved;

        /// <summary>
        /// Total number of items rejected during execution of this operation.
        /// </summary>
        public int TotalNumberOfItemsRejected { get; init; } = TotalNumberOfItemsRejected;

        /// <summary>
        /// Specified result of the cash movement.
        /// Key - Name of the storage unit Id from which items have been moved.
        /// </summary>
        public Dictionary<string, ReplenishTargetResult> TargetResults { get; init; } = TargetResults;
    }

    /// <summary>
    /// CashInStartRequest
    /// Before initiating a cash-in operation
    /// </summary>
    public sealed class CashInStartRequest(
        long? TellerID,
        bool UseRecycleUnits,
        CashManagementCapabilitiesClass.PositionEnum OutputPosition,
        CashManagementCapabilitiesClass.PositionEnum InputPosition,
        long? TotalItemsLimit,
        Dictionary<string, double> AmountLimit)
    {

        /// <summary>
        /// Identification of teller. This field is not applicable to Self-Service devices
        /// </summary>
        public long TellerID { get; init; } = TellerID ?? 0;

        /// <summary>
        /// Specifies whether or not the recycle storage units should be used when items are cashed in on a 
        /// successfulCashAcceptor.CashInEnd command. This parameter will be ignored if 
        /// there are no recycle storage units or the hardware does not support this.
        /// </summary>
        public bool UseRecycleUnits { get; init; } = UseRecycleUnits;

        /// <summary>
        /// The output position where the items will be presented to the customer in the case of a rollback. 
        /// </summary>
        public CashManagementCapabilitiesClass.PositionEnum OutputPosition { get; init; } = OutputPosition;

        /// <summary>
        /// The position the cash should be inserted.
        /// </summary>
        public CashManagementCapabilitiesClass.PositionEnum InputPosition { get; init; } = InputPosition;

        /// <summary>
        /// If set to a non-zero value, specifies a limit on the total number of items to be accepted during the cash-in transaction. 
        /// If set to a zero value, this limitation will not be performed.
        /// </summary>
        public long TotalItemsLimit { get; init; } = TotalItemsLimit ?? 0;

        /// <summary>
        /// If specified, provides a list of the maximum amount of one or more currencies to be accepted during the cash-in transaction.
        /// </summary>
        public Dictionary<string, double> AmountLimit { get; init; } = AmountLimit;
    }

    /// <summary>
    /// CashInStartResult
    /// Return result of cash-in start operation.
    /// </summary>
    public sealed class CashInStartResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        CashInStartCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code for the cash-in start operation.
        /// </summary>
        public CashInStartCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// CashInRequest
    /// Request for the cash-in operation
    /// </summary>
    public sealed class CashInRequest(int Timeout)
    {

        /// <summary>
        /// Timeout for waiting customer to insert items
        /// </summary>
        public int Timeout { get; init; } = Timeout;
    }

    /// <summary>
    /// CashInResult
    /// Return result of cash-in operation.
    /// </summary>
    public sealed class CashInResult : DeviceResult
    {
        public CashInResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            CashInCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            Unrecognized = 0;
            ItemCounts = null;
            MovementResult = null;
        }

        public CashInResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, CashItemCountClass> ItemCounts,
            Dictionary<string, CashUnitCountClass> MovementResult,
            int Unrecognized = 0)
            : base(CompletionCode, null)
        {
            ErrorCode = null;
            this.Unrecognized = Unrecognized;
            this.ItemCounts = ItemCounts;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code for the cash-in operation.
        /// </summary>
        public CashInCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Number of items refused
        /// </summary>
        public int Unrecognized { get; init; }
        
        /// <summary>
        /// Number of items recognised
        /// </summary>
        public Dictionary<string, CashItemCountClass> ItemCounts { get; init; }

        /// <summary>
        /// Specifies the detailed note movement if cash is stored in the cash units.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }


    /// <summary>
    /// CashInRollbackResult
    /// Return result of cash-in operation.
    /// </summary>
    public sealed class CashInRollbackResult : DeviceResult
    {
        public CashInRollbackResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            CashInRollbackCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            MovementResult = null;
        }

        public CashInRollbackResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, CashUnitCountClass> MovementResult)
            : base(CompletionCode, null)
        {
            ErrorCode = null;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code for the cash-in rollback operation.
        /// </summary>
        public CashInRollbackCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the detailed note movement if cash is stored in the cash units.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// CashInEndResult
    /// Return result of cash-in end operation.
    /// </summary>
    public sealed class CashInEndResult : DeviceResult
    {
        public CashInEndResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            CashInEndCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            MovementResult = null;
        }

        public CashInEndResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, CashUnitCountClass> MovementResult)
            : base(CompletionCode, null)
        {
            ErrorCode = null;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code for the cash-in end operation.
        /// </summary>
        public CashInEndCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the detailed note movement if cash is stored in the cash units.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// ConfigureNoteTypesRequest
    /// Request for the specified banknotes enabled or disabled.
    /// </summary>
    public sealed class ConfigureNoteTypesRequest(Dictionary<string, bool> BanknoteItems)
    {
        /// <summary>
        /// Banknote items to be enabled/recognized
        /// </summary>
        public Dictionary<string, bool> BanknoteItems { get; init; } = BanknoteItems;
    }

    /// <summary>
    /// ConfigureNoteTypesResult
    /// Return result of configuring banknote types to the recognition unit
    /// </summary>
    public sealed class ConfigureNoteTypesResult : DeviceResult
    {
        public ConfigureNoteTypesResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ConfigureNoteTypesCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        /// <summary>
        /// Specifies the error code on configuring note items.
        /// </summary>
        public ConfigureNoteTypesCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    /// <summary>
    /// ConfigureNoteReaderRequest
    /// Request for loading template data
    /// </summary>
    public sealed class ConfigureNoteReaderRequest(bool LoadAlways)
    {

        /// <summary>
        /// If set to true, the Service loads the currency description data into the note reader, even if it is already
        /// loaded.
        /// </summary>
        public bool LoadAlways { get; init; } = LoadAlways;
    }

    /// <summary>
    /// ConfigureNoteTypesResult
    /// Return result of loading template data
    /// </summary>
    public sealed class ConfigureNoteReaderResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        ConfigureNoteReaderCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on loading note templates.
        /// </summary>
        public ConfigureNoteReaderCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// CreateSignatureRequest
    /// Request for the operation to scan notes of signature
    /// </summary>
    public sealed class CreateSignatureRequest(int Timeout)
    {

        /// <summary>
        /// Timeout for waiting customer to insert items
        /// </summary>
        public int Timeout { get; init; } = Timeout;
    }

    /// <summary>
    /// CreateSignatureResult
    /// Return result of scanning signature data
    /// </summary>
    public sealed class CreateSignatureResult : DeviceResult
    {
        public CreateSignatureResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            CreateSignatureCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            SignatureCaptured = null;
        }

        public CreateSignatureResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            SignatureInfo SignatureCaptured)
            : base(CompletionCode, null)
        {
            ErrorCode = null;
            this.SignatureCaptured = SignatureCaptured;
        }

        /// <summary>
        /// Specifies the error code on scanning note signature data
        /// </summary>
        public CreateSignatureCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Signature information captured
        /// </summary>
        public SignatureInfo SignatureCaptured { get; init; }
    }

    /// <summary>
    /// CashInStartRequest
    /// Request to prepare present operation
    /// </summary>
    public sealed class PreparePresentRequest(CashManagementCapabilitiesClass.PositionEnum OutputPosition)
    {

        /// <summary>
        /// Timeout for waiting customer to insert items
        /// </summary>
        public CashManagementCapabilitiesClass.PositionEnum OutputPosition { get; init; } = OutputPosition;
    }

    /// <summary>
    /// PreparePresentResult
    /// Return result of preparing present operation.
    /// </summary>
    public sealed class PreparePresentResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        PreparePresentCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code for the preparing present operation.
        /// </summary>
        public PreparePresentCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// CashInStartRequest
    /// Request to count cash in the specified cash units
    /// </summary>
    public sealed class CashUnitCountRequest(List<string> StorageIds)
    {

        /// <summary>
        /// Name of storage
        /// </summary>
        public List<string> StorageIds { get; init; } = StorageIds;
    }

    /// <summary>
    /// CashUnitCountResult
    /// Return result of counting cash in the specified cash units
    /// </summary>
    public sealed class CashUnitCountResult : DeviceResult
    {
        public CashUnitCountResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            CashUnitCountCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public CashUnitCountResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<string, CashUnitCountClass> MovementResult)
            : base(CompletionCode, null)
        {
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code for the counting cash in the cash unit.
        /// </summary>
        public CashUnitCountCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the detailed note movement if cash is counted for the specified storage.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// CompareSignatureRequest
    /// Request to find best match for the signature of notes
    /// </summary>
    public sealed class CompareSignatureRequest(
        List<SignatureInfo> ReferenceSignatures,
        Dictionary<int, SignatureInfo> SignaturesToCompare)
    {

        /// <summary>
        /// Each structure represents the signature corresponding to one orientation of a single reference banknote.
        /// </summary>
        public List<SignatureInfo> ReferenceSignatures { get; init; } = ReferenceSignatures;

        /// <summary>
        /// Each structure represents a signature from the cash-in transactions
        /// </summary>
        public Dictionary<int, SignatureInfo> SignaturesToCompare { get; init; } = SignaturesToCompare;
    }

    /// <summary>
    /// CompareSignaturetResult
    /// Return result of matching signature data
    /// </summary>
    public sealed class CompareSignatureResult : DeviceResult
    {
        public sealed class ConfidenceLevelClass(
            UInt16 ConfidenceLevel,
            string ComparisonData = null)
        {

            /// <summary>
            /// Specifies the level of confidence for the match found. This value is in a scale 1 - 100, where 100 is the
            /// maximum confidence level. This value is 0 if the Service does not support the confidence level factor.
            /// </summary>
            public UInt16 ConfidenceLevel { get; init; } = ConfidenceLevel;

            /// <summary>
            /// Vendor dependent comparison result data. This data may be used as justification for the signature match or
            /// confidence level. This field is omitted if no additional comparison data is returned.
            /// </summary>
            public string ComparisonData { get; init; } = ComparisonData;
        }

        public CompareSignatureResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            CompareSignatureCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.ConfidenceLevels = null;
        }

        public CompareSignatureResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            Dictionary<int, ConfidenceLevelClass> ConfidenceLevels)
            : base(CompletionCode, null)
        {
            this.ConfidenceLevels = ConfidenceLevels;
        }

        /// <summary>
        /// Specifies the error code on comparing supplied signature agaist stored signature detected during customer transaction.
        /// </summary>
        public CompareSignatureCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the confidence level associated with the key supplied by the input parameter for the SignatureToCompare property.
        /// </summary>
        public Dictionary<int, ConfidenceLevelClass> ConfidenceLevels { get; init; }
    }

    /// <summary>
    /// DepleteRequest
    /// Request to move cash from replenishment container to cash units or other direction.
    /// </summary>
    public sealed class DepleteRequest(
        Dictionary<string, int> DepleteSources,
        string DepleteDestination)
    {

        /// <summary>
        /// Key-pair of objects listing which storage units are to be depleted. There must be at least one element in this
        /// dictionary.
        /// </summary>
        public Dictionary<string, int> DepleteSources { get; init; } = DepleteSources;

        /// <summary>
        /// Name of the storage unit Id to which items are to be moved.
        /// </summary>
        public string DepleteDestination { get; init; } = DepleteDestination;
    }

    /// <summary>
    /// DepleteResult
    /// Return result of moving cash from source to the destination
    /// </summary>
    public sealed class DepleteResult : DeviceResult
    {
        public DepleteResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            DepleteCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.OperationResult = null;
            this.MovementResult = null;
        }

        public DepleteResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            DepleteOperationResult OperationResult,
            Dictionary<string, CashUnitCountClass> MovementResult)
            : base(CompletionCode, null)
        {
            this.OperationResult = OperationResult;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on moving cash from replenishment container to cash unit or other direction.
        /// </summary>
        public DepleteCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Total number of items received in the target storage unit during execution of this operation.
        /// </summary>
        public DepleteOperationResult OperationResult { get; init; }

        /// <summary>
        /// Specifies the detailed note movement if cash is moved from source to targes.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// DepleteRequest
    /// Request to lock devices.
    /// </summary>
    public sealed class DeviceLockRequest(
        DeviceLockRequest.DeviceActionEnum DeviceAction,
        DeviceLockRequest.CashUnitActionEnum CashUnitAction,
        Dictionary<string, DeviceLockRequest.UnitActionEnum> UnitLockControl)
    {
        public enum DeviceActionEnum
        {
            Lock,
            Unlock,
            NoLockAction,
        }

        public enum CashUnitActionEnum
        {
            LockAll,
            UnlockAll,
            LockIndividual,
            NoLockAction,
        }

        public enum UnitActionEnum
        {
            Lock,
            Unlock,
        }

        /// <summary>
        /// Specifies locking or unlocking the device in its normal operating position. The following values are 
        /// possible:
        /// 
        /// * ```Lock``` - Locks the device so that it cannot be removed from its normal operating position.
        /// * ```Unlock``` - Unlocks the device so that it can be removed from its normal operating position.
        /// * ```NoLockAction``` - No lock/unlock action will be performed on the device.
        /// </summary>
        public DeviceActionEnum DeviceAction { get; init; } = DeviceAction;

        /// <summary>
        /// Specifies the type of lock/unlock action on storage units. The following values are possible:
        /// 
        /// * ```LockAll``` - Locks all storage units supported.
        /// * ```UnlockAll``` - Unlocks all storage units supported.
        /// * ```LockIndividual``` - Locks/unlocks storage units individually as specified in the *unitLockControl* parameter.
        /// * ```NoLockAction``` - No lock/unlock action will be performed on storage units.
        /// </summary>
        public CashUnitActionEnum CashUnitAction { get; init; } = CashUnitAction;

        /// <summary>
        /// Array of structures, one for each storage unit to be locked or unlocked. Only valid in the case where 
        /// LockIndividual is specified in the CashUnitAction property otherwise this will be ignored.
        /// </summary>
        public Dictionary<string, UnitActionEnum> UnitLockControl { get; init; } = UnitLockControl;
    }

    /// <summary>
    /// DepleteResult
    /// Return result of locking device
    /// </summary>
    public sealed class DeviceLockResult(
        MessageHeader.CompletionCodeEnum CompletionCode,
        string ErrorDescription = null,
        DeviceLockControlCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null) : DeviceResult(CompletionCode, ErrorDescription)
    {

        /// <summary>
        /// Specifies the error code on locking the device
        /// </summary>
        public DeviceLockControlCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; } = ErrorCode;
    }

    /// <summary>
    /// ReplenishRequest
    /// Request to replenish items to the storage.
    /// </summary>
    public sealed class ReplenishRequest(
        string Source,
        Dictionary<string, int> Targets)
    {

        /// <summary>
        /// Name of the storage unit items are to be removed.
        /// </summary>
        public string Source { get; init; } = Source;

        /// <summary>
        /// How many items are to be moved and to which storage unit.
        /// </summary>
        public Dictionary<string, int> Targets { get; init; } = Targets;
    }

    /// <summary>
    /// ReplenishResult
    /// Return result of replenishment operation.
    /// </summary>
    public sealed class ReplenishResult : DeviceResult
    {
        public ReplenishResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            ReplenishCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            OperationResult = null;
            MovementResult = null;
        }

        public ReplenishResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            ReplenishOperationResult OperationResult,
            Dictionary<string, CashUnitCountClass> MovementResult)
            : base(CompletionCode, null)
        {
            this.ErrorCode = ErrorCode;
            this.OperationResult = OperationResult;
            this.MovementResult = MovementResult;
        }

        /// <summary>
        /// Specifies the error code on replenish operation.
        /// </summary>
        public ReplenishCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Total number of items moved in the target storage unit during execution of this operation.
        /// </summary>
        public ReplenishOperationResult OperationResult { get; init; }

        /// <summary>
        /// Specifies the detailed note movement if cash is moved from source to targes.
        /// </summary>
        public Dictionary<string, CashUnitCountClass> MovementResult { get; init; }
    }

    /// <summary>
    /// ReplenishRequest
    /// Request to presenting media
    /// </summary>
    public sealed class PresentMediaRequest(
        CashManagementCapabilitiesClass.PositionEnum Position,
        CashManagementPresentStatus CurrentPresentStatus)
    {

        /// <summary>
        /// Position to present media
        /// </summary>
        public CashManagementCapabilitiesClass.PositionEnum Position { get; init; } = Position;

        /// <summary>
        /// Current present status holding by the framework.
        /// </summary>
        public CashManagementPresentStatus CurrentPresentStatus { get; init; } = CurrentPresentStatus;
    }

    /// <summary>
    /// ReplenishResult
    /// Return result of presenting media operation.
    /// </summary>
    public sealed class PresentMediaResult : DeviceResult
    {
        public PresentMediaResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            PresentMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null,
            CashManagementPresentStatus LastPresentStatus = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.LastPresentStatus = LastPresentStatus;
        }

        public PresentMediaResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            CashManagementPresentStatus LastPresentStatus)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.LastPresentStatus = LastPresentStatus;
        }

        /// <summary>
        /// Specifies the error code on presenting media operation
        /// </summary>
        public PresentMediaCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Updated present status after the presenting media operation.
        /// </summary>
        public CashManagementPresentStatus LastPresentStatus { get; init; }
    }
}
/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;
using XFS4IoTServer;

// KAL specific implementation of cashacceptor. 
namespace XFS4IoTFramework.CashAcceptor
{
    public interface ICashAcceptorDevice : IDevice
    {
        /// <summary>
        /// Before initiating a cash-in operation, an application must issue the CashInStart command to begin a cash-in transaction.
        /// During a cash-in transaction any number of CashIn commands may be issued. 
        /// The transaction is ended when either a CashInRollback, CashInEnd, Storage.Retract or Reset command is sent. 
        /// Where the capabilities ShutterControl is false, this command precedes any explicit operation of the shutters.
        /// The Storage.Retract will terminate a transaction.
        /// In this case CashInEnd, CashInRollback and CashIn will report NoCashInActive command error.
        /// If an application wishes to determine where the notes went during a transaction it can execute a Storage.GetStorage before and after the transaction and then derive the difference.
        /// A hardware failure during the cash-in transaction does not reset the note number list information; 
        /// instead the note number list information will include items that could be accepted and identified up to the point of the hardware failure.
        /// </summary>
        Task<CashInStartResult> CashInStart(CashInStartRequest rquest, CancellationToken cancellation);

        /// <summary>
        /// This command moves items into the cash device from an input position.
        /// On devices with implicit shutter control, the CashAcceptor.InsertItemsEvent will be
        /// generated when the device is ready to start accepting media.
        /// The items may pass through the banknote reader for identification.Failure to identify items does not mean that the
        /// command has failed - even if some or all of the items are rejected by the banknote reader the command may return
        /// success. In this case one or more CashAcceptor.InputRefuseEvent events will be sent
        /// to report the rejection.See also the paragraph below about returning refused items.
        /// If the device does not have a banknote reader then the completion message will be empty.
        /// 
        /// If the device has a cash-in stacker then this command will cause inserted genuine items (see
        /// Note Classification) to be moved there after
        /// validation.Counterfeit, suspect or inked items may also be moved to the cash-in stacker, but some devices may immediately move
        /// them to a designated storage unit. Items on the stacker will remain there until the current cash-in transaction is either
        /// cancelled by the CashAcceptor.CashInRollback command or confirmed by the
        /// CashAcceptor.CashInEnd command. These commands will cause any non-genuine items on the
        /// cash-in stacker to be moved to the appropriate storage unit.If there is no cash-in stacker then this command will move
        /// items directly to the storage units and the CashAcceptor.CashInRollback command will not be supported. Storage unit
        /// information will be updated accordingly whenever notes are moved to a storage unit during this command.
        /// 
        /// Note that the acceptor status field may change value
        /// during a cash-in transaction.If media has been retained to storage units during a cash-in transaction, it may mean that
        /// Acceptor is set to Stop, which means subsequent cash-in operations may not be possible.In this case, the subsequent
        /// command fails with cashUnitError.
        /// 
        /// The shutterControl field will determine
        /// whether the shutter is controlled implicitly by this command or whether the application must explicitly open and close
        /// the shutter using the CashManagement.OpenShutter,CashManagement.CloseShutter or CashAcceptor.PresentMedia
        /// commands. If shutterControl is false then this command does not operate the shutter in any way, the application is
        /// responsible for all shutter control. If shutterControl is true this command opens the shutter at the start of the
        /// command and closes it once bills are inserted.
        /// 
        /// The presentControl field will determine
        /// whether or not it is necessary to call the CashAcceptor.PresentMedia command in order to move items to the output
        /// position. If presentControl is true then all items are moved immediately to the correct output position for removal (a
        /// CashManagement.OpenShutter command will be needed in the case of explicit shutter control). If presentControl is false then items
        /// are not returned immediately and must be presented to the correct output position for removal using the
        /// CashAcceptor.PresentMedia command.
        /// It is possible that a device may divide bill or coin accepting into a series of sub-operations under hardware control.
        /// In this case a CashAcceptor.SubCashInEvent may be sent after each sub-operation, if the
        /// hardware capabilities allow it.
        /// Returning items (single bunch):
        /// If shutterControl is true, and a single bunch of items is returned then this command will complete once the notes have
        /// been returned. A CashManagement.ItemsPresentedEvent will be generated.
        /// 
        /// If shutterControl is false, and a single bunch of items is returned then this command will complete without generating
        /// a CashManagement.ItemsPresentedEvent, instead the event will be generated by the
        /// subsequent CashManagement.OpenShutter or CashAcceptor.PresentMedia command.
        /// Returning items (multiple bunches):
        /// It is possible that a device will in certain situations return refused items in multiple bunches. In this case, this
        /// command will not complete until the final bunch has been presented and after the last
        /// CashManagement.ItemsPresentedEvent has been generated. For these devices
        /// shutterControl and presentControl fields of the positionCapabilities structure returned from the
        /// Common.Capabilities CashAcceptor.PositionCapabilities query must both be true otherwise it will not be possible to
        /// return multiple bunches. Additionally it may be possible to request the completion of this command with a
        /// Common.Cancel before the final bunch is presented so that after the completion of this command the
        /// CashManagement.Retract or CashManagement.Reset command can be used to
        /// move the remaining bunches, although the ability to do this will be hardware dependent.
        /// </summary>
        Task<CashInResult> CashIn(CashInCommandEvents events, 
                                  CashInRequest request, 
                                  CancellationToken cancellation);

        /// <summary>
        /// This command ends a cash-in transaction. If cash items are on the stacker as a result of a
        /// CashAcceptor.CashIn command these items are moved to the appropriate storage units.
        /// 
        /// The cash-in transaction is ended even if this command does not complete successfully.
        /// In the special case where all the items inserted by the customer are classified as counterfeit and/or suspect items and the
        /// Service is configured to automatically retain these item types then the command will complete with success even if the
        /// hardware may have already moved the counterfeit and/or suspect items to their respective storage units on the
        /// CashAcceptor.CashIn command and there are no items on the stacker at the start of the command. This allows the
        /// location of the notes retained to be reported in the output parameter.If no items are available for cash-in for any
        /// other reason, the NoItems error code is returned.
        /// </summary>
        Task<CashInEndResult> CashInEnd(CashInEndCommandEvents events, 
                                        CancellationToken cancellation);

        /// <summary>
        /// This command is used to roll back a cash-in transaction. It causes all the cash items cashed in since the last
        /// CashAcceptor.CashInStart command to be returned to the customer.
        /// This command ends the current cash-in transaction.The cash-in transaction is ended even if this command does not
        /// complete successfully.
        /// 
        /// The shutterControl property in the capabilities will determine
        /// whether the shutter is controlled implicitly by this command or whether the application must explicitly open and close
        /// the shutter using the CashManagement.OpenShutter, CashManagement.CloseShutter or CashAcceptor.PresentMedia
        /// commands. If shutterControl is false then this command does not operate the shutter in any way, the application is
        /// responsible for all shutter control. If shutterControl is true then this command opens the shutter and it is closed
        /// when all items are removed.
        /// 
        /// The presentControl property in the capabilities will determine
        /// whether or not it is necessary to call the CashAcceptor.PresentMedia command in order to move items to the output
        /// position. If presentControl is true then all items are moved immediately to the correct output position for removal (a
        /// CashManagement.OpenShutter command will be needed in the case of explicit shutter control). If presentControl is false then items are
        /// not returned immediately and must be presented to the correct output position for removal using the CashAcceptor.PresentMedia command.
        /// Items are returned in a single bunch or multiple bunches in the same way as described for the
        /// CashAcceptor.CashIn command.
        /// In the special case where all the items inserted by the customer are classified as counterfeit and/or suspect, and
        /// the Service is configured to automatically retain these item types, then the command
        /// will complete with Success even though no items are returned to the customer. This allows the location of the notes
        /// retained to be reported in the output parameter.The application can tell if items have been returned or not via the
        /// CashManagement.ItemsPresentedEvent](#cashmanagement.itemspresentedevent). This event will be generated before the command completes when items are returned.
        /// This event will not be generated if no items are returned. If no items are available to rollback for any other reason,
        /// the NoItems error code is returned.
        /// </summary>
        Task<CashInRollbackResult> CashInRollback(CashInRollbackCommandEvents events,
                                                  CancellationToken cancellation);

        /// <summary>
        /// This command is used to change the note types the banknote reader should accept during cash-in. 
        /// Only note typeswhich are to be changed need to be specified in the command payload. 
        /// If an unknown note type is given the UnsupportedData will be returned.
        /// The values set by this command are persistent.
        /// </summary>
        Task<ConfigureNoteTypesResult> ConfigureNoteTypes(ConfigureNoteTypesRequest request, 
                                                          CancellationToken cancellation);

        /// <summary>
        /// This command is used to create a reference signature which can be compared with the available signatures of the cash-in
        /// transactions to track back the customer.
        /// 
        /// When this command is executed, the device waits for a note to be inserted at the input position, transports the note to
        /// the recognition module, creates the signature and then returns the note to the output position.
        /// 
        /// The shutterControl property in the capabilities will determine
        /// whether the shutter is controlled implicitly by this command or whether the application must explicitly open and close
        /// the shutter using the CashManagement.OpenShutter, CashManagement.CloseShutter or CashAcceptor.PresentMedia
        /// commands. If shutterControl is false then this command does not operate the shutter in any way, and the application is
        /// responsible for all shutter control.If shutterControl is true then this command opens and closes the shutter at
        /// various times during the command execution and the shutter is finally closed when all items are removed.
        /// 
        /// The presentControl property in the capabilities will determine
        /// whether or not it is necessary to call the CashAcceptor.PresentMedia command in order to move items to the output
        /// position. If presentControl is true then all items are moved immediately to the correct output position for removal (a
        /// CashManagement.OpenShutter command will be needed in the case of explicit shutter control). If presentControl is false then items
        /// are not returned immediately and must be presented to the correct output position for removal using the
        /// CashAcceptor.PresentMedia command.
        /// 
        /// On devices with implicit shutter control, the CashAcceptor.InsertItemsEvent will be
        /// generated when the device is ready to start accepting media.
        /// 
        /// The application may have to execute this command repeatedly to make sure that all possible signatures are captured.
        /// If a single note is entered and returned to the customer but cannot be processed fully (e.g.no recognition software in
        /// the recognition module, the note is not recognized, etc.) then a
        /// CashAcceptor.InputRefuseEvent](#cashacceptor.inputrefuseevent) will be sent and the command will complete. In this
        /// case, no note specific output properties will be returned.
        /// </summary>
        Task<CreateSignatureResult> CreateSignature(CreateSignatureCommandEvents events,
                                                    CreateSignatureRequest request, 
                                                    CancellationToken cancellation);

        /// <summary>
        /// This command is used to configure the currencydescription configuration data into the banknote reader module. 
        /// Theformat and location of the configuration data is vendor and/or hardwaredependent.
        /// </summary>
        Task<ConfigureNoteReaderResult> ConfigureNoteReader(ConfigureNoteReaderRequest request,
                                                            CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/CompareSignatureDescription.md
        /// </summary>
        Task<CompareSignatureResult> CompareSignature(CompareSignatureRequest request, 
                                                      CancellationToken cancellation);

        /// <summary>
        /// This command replenishes items from a single storage unit to multiple storage units. Applications can use this command to
        /// ensure that there is the optimum number of items in the cassettes by moving items from a source storage unit to a target
        /// storage unit.This is especially applicable if a replenishment storage unit is used for the replenishment and can help to
        /// minimize manual replenishment operations.
        /// 
        /// The CashAcceptor.GetReplenishTarget command can be used to determine what storage unit.
        /// can be specified as target storage units for a given source storage unit.Any items which are removed from the source cash
        /// unit that are not of the correct currency and value for the target storage unit during execution of this command will be
        /// returned to the source storage unit.
        /// 
        /// The counts returned with the Storage.GetStorage command will be updated as part of the execution
        /// of this command.
        /// 
        /// If the command fails after some items have been moved, the command will complete with an appropriate error code, and a
        /// CashAcceptor.IncompleteReplenishEvent will be sent.
        /// </summary>
        Task<ReplenishResult> Replenish(ReplenishCommandEvents events,
                                        ReplenishRequest request, 
                                        CancellationToken cancellation);

        /// <summary>
        /// This command counts the items in the storage unit(s). If it is necessary to move items internally to count them,
        /// the items should be returned to the unit from which they originated before completion of the command.If items could not
        /// be moved back to the storage unit they originated from and did not get rejected, the command will complete with an
        /// appropriate error.
        /// 
        /// During the execution of this command one Storage.StorageChangedEvent will be
        /// generated for each storage unit that has been counted successfully, or if the counts have changed, even if the overall
        /// command fails.
        /// 
        /// If an application wishes to determine where the notes went during the command it can execute a
        /// Storage.GetStorage command before and after the transaction and then derive the difference.
        /// 
        /// This command is designed to be used on devices where the counts cannot be guaranteed to be accurate and therefore may
        /// need to be automatically counted periodically. Upon successful completion, for those storage units that have been
        /// counted, the counts are accurately reported with the Storage.GetStorage command.
        /// </summary>
        Task<CashUnitCountResult> CashUnitCount(CashUnitCountCommandEvents events,
                                                CashUnitCountRequest request, 
                                                CancellationToken cancellation);

        /// <summary>
        /// This command can be used to lock or unlock a CashAcceptor device or one or more storage units.
        /// CashAcceptor.GetDeviceLockStatus can be used to obtain the current lock state of any items which support locking.
        /// 
        /// During normal device operation the device and storage units will be locked and removal will not be possible.If supported,
        /// the device or storage units can be unlocked, ready for removal.In this situation the device will still remain online and
        /// cash-in or dispense operations will be possible, as long as the device or storage units are not physically removed from
        /// their normal operating position.
        /// 
        /// If the lock action is specified and the device or storage units are already locked, or if the unlock action is specified
        /// and the device or storage units are already unlocked then the action will complete successfully.
        /// 
        /// Once a storage unit has been removed and reinserted it may then have a manipulated status. This status can only
        /// be cleared by issuing a Storage.StartExchange / Storage.EndExchange command sequence.
        /// 
        /// The device and all storage units will also be locked implicitly as part of the execution of the Storage.EndExchange or
        /// the CashManagement.Reset command.
        /// 
        /// The normal command sequence is as follows:
        /// 1. CashAcceptor.DeviceLockControl command is executed to unlock the device and some or all of the storage units.
        /// 2. Optionally a cash-in transaction or a dispense transaction on a cash recycler device may be performed.
        /// 3. The operator was not required to remove any of the storage units, all storage units are still in their original position.
        /// 4. CashAcceptor.DeviceLockControl command is executed to lock the device and the storage units.
        /// 
        /// The relation of lock/unlock control with the Storage.StartExchange and the Storage.EndExchange commands is as follows:
        /// 1. CashAcceptor.DeviceLockControl command is executed to unlock the device and some or all of the storage units.
        /// 2. Optionally a CashAcceptor.CashInStart / CashAcceptor.CashIn / CashAcceptor.CashInEnd cash-in transaction or a
        /// CashDispenser.Dispense / CashDispenser.Present transaction on a 
        /// cash recycler device may be performed.
        /// 
        /// The operator removes and reinserts one or more of the previously unlocked storage units. The associated
        /// Storage.StorageChangedEvent will be posted and after the reinsertion the storage unit 
        /// will show the status manualInsertion.
        /// Storage.StartExchange command is executed.
        /// Storage.EndExchange command is executed.During this command execution the Service implicitly locks the device and
        /// all previously unlocked storage units. The status of the previously removed unit will be reset.
        /// </summary>
        Task<DeviceLockResult> DeviceLockControl(DeviceLockRequest request, 
                                                 CancellationToken cancellation);

        /// <summary>
        /// This command opens the shutter and presents items to be taken by the customer. The shutter is automatically closed after
        /// the media is taken.The command can be called after a[CashAcceptor.CashIn] (#cashacceptor.cashin),
        /// CashAcceptor.CashInRollback, CashManagement.Reset or
        /// CashAcceptor.CreateSignature command and can be used with explicit and implicit
        /// shutter control. The command is only valid on positions where Usage reported by the
        /// CashAcceptor.GetPositionCapabilities command is Rollback or Rrefuse and
        /// where PresentControl reported by the CashAcceptor.GetPositionCapabilities command is false.
        /// </summary>
        Task<PresentMediaResult> PresentMedia(PresentMediaRequest request, 
                                              CancellationToken cancellation);

        /// <summary>
        /// This command moves items from multiple storage units to a single storage unit. Applications can use this command to ensure
        /// that there are the optimum number of items in the cassettes by moving items from source storage units to a target storage unit.
        /// This is especially applicable if surplus items are removed from multiple recycle storage units to a replenishment storage unit
        /// and can help to minimize manual replenishment operations.
        /// 
        /// The CashAcceptor.GetDepleteSource command can be used to determine what storage units can
        /// be specified as source storage units for a given target storage unit.
        /// 
        /// The counts returned by the Storage.GetStorage command will be updated as part of the execution of this command.
        /// 
        /// If the command fails after some items have been moved, the command will complete with an appropriate error code, and a
        /// CashAcceptor.IncompleteDepleteEvent will be sent.
        /// </summary>
        Task<DepleteResult> Deplete(DepleteCommandEvents events,
                                    DepleteRequest request, 
                                    CancellationToken cancellation);

        /// <summary>
        /// In cases where multiple bunches are to be returned under explicit shutter control, 
        /// this command is used for the purpose of moving a remaining bunch to the output position explicitly before using the following commands:
        /// CashManagement.OpenShutter
        /// CashAcceptor.PresentMedia
        /// The application can tell whether the additional items were left by using the CashAcceptor.GetPresentStatus command.
        /// This command does not affect the status of the current cash-in transaction.
        /// </summary>
        Task<PreparePresentResult> PreparePresent(PreparePresentCommandEvents events,
                                                  PreparePresentRequest request, 
                                                  CancellationToken cancellation);

        /// <summary>
        /// The deplete target and destination information
        /// Key - The storage id can be used for target of the depletion operation.
        /// Value - List of storage id can be used for source of the depletion operation
        /// </summary>
        Dictionary<string, List<string>> GetDepleteCashUnitSources();

        /// <summary>
        /// Which storage units can be specified as targets for a given source storage unit with the CashAcceptor.Replenish command
        /// </summary>
        List<string> ReplenishTargets();

        /// <summary>
        /// CashAcceptor Status
        /// </summary>
        CashAcceptorStatusClass CashAcceptorStatus { get; set; }

        /// <summary>
        /// CashAcceptor Capabilities
        /// </summary>
        CashAcceptorCapabilitiesClass CashAcceptorCapabilities { get; set; }

        /// <summary>
        /// Status of current cash-in operation.
        /// if this property is set to null, the framework maitains cash-in status
        /// </summary>
        CashInStatusClass CashInStatus { get; set; }

        /// <summary>
        /// The physical lock/unlock status of the CashAcceptor device and storages.
        /// </summary>
        DeviceLockStatusClass DeviceLockStatus { get; set; }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ICashAcceptorDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTFramework.Common;
using XFS4IoTServer;

// KAL specific implementation of cashacceptor. 
namespace XFS4IoTFramework.CashAcceptor
{
    public interface ICashAcceptorDevice : IDevice
    {

        /// <summary>
        /// This command is used to get information about the statusof the currently active cash-in transaction, or in the case where nocash-in transaction is active the status of the most recently endedcash-in transaction. This value is persistent and is valid until thenext [CashAcceptor.CashInStart](#cashacceptor.cashinstart) command.
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.GetCashInStatusCompletion.PayloadData> GetCashInStatus(IGetCashInStatusEvents events, 
                                                                                                     XFS4IoT.CashAcceptor.Commands.GetCashInStatusCommand.PayloadData payload, 
                                                                                                     CancellationToken cancellation);

        /// <summary>
        /// This command allows the application to get additionalinformation about the use assigned to each position available in thedevice.
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.GetPositionCapabilitiesCompletion.PayloadData> GetPositionCapabilities(IGetPositionCapabilitiesEvents events, 
                                                                                                                     XFS4IoT.CashAcceptor.Commands.GetPositionCapabilitiesCommand.PayloadData payload, 
                                                                                                                     CancellationToken cancellation);

        /// <summary>
        /// This command is used to determine which storage units can be specified as targets for a given source storage unit with the[CashAcceptor.Replenish](#cashacceptor.replenish) command. For example, it can be used todetermine which targets can be used for replenishment from a replenishment container or from a recycle unit.
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.GetReplenishTargetCompletion.PayloadData> GetReplenishTarget(IGetReplenishTargetEvents events, 
                                                                                                           XFS4IoT.CashAcceptor.Commands.GetReplenishTargetCommand.PayloadData payload, 
                                                                                                           CancellationToken cancellation);

        /// <summary>
        /// This command is used to retrieve the lock/unlock statuses of the CashAcceptor device and each of its storage units.This is only supported if the physical locking and unlocking of the device or the storage units is supported.
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.GetDeviceLockStatusCompletion.PayloadData> GetDeviceLockStatus(IGetDeviceLockStatusEvents events, 
                                                                                                             XFS4IoT.CashAcceptor.Commands.GetDeviceLockStatusCommand.PayloadData payload, 
                                                                                                             CancellationToken cancellation);

        /// <summary>
        /// This command is used to determine which storage units canbe specified as source storage units for a given target storage unit with the[CashAcceptor.Deplete](#cashacceptor.deplete) command. For example, it can be used to determinewhich sources can be used for depletion to a replenishment container orto a cash-in storage unit.
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.GetDepleteSourceCompletion.PayloadData> GetDepleteSource(IGetDepleteSourceEvents events, 
                                                                                                       XFS4IoT.CashAcceptor.Commands.GetDepleteSourceCommand.PayloadData payload, 
                                                                                                       CancellationToken cancellation);

        /// <summary>
        /// This command is used to obtain the status of the most recent attempt to present or return items to the customer. This information includes the number of items previously moved to the output position and the number of items which have yet to be returned as a result of the following commands:[CashAcceptor.CashIn](#cashacceptor.cashin),[CashAcceptor.CashInRollback](#cashacceptor.cashinrollback),[CashAcceptor.PreparePresent](#cashacceptor.preparepresent),[CashAcceptor.PresentMedia](#cashacceptor.presentmedia),[CashManagement.OpenShutter](#cashmanagement.openshutter) (In the case of returning multiple bunches)
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.GetPresentStatusCompletion.PayloadData> GetPresentStatus(IGetPresentStatusEvents events, 
                                                                                                       XFS4IoT.CashAcceptor.Commands.GetPresentStatusCommand.PayloadData payload, 
                                                                                                       CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/CashInStartDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.CashInStartCompletion.PayloadData> CashInStart(ICashInStartEvents events, 
                                                                                             XFS4IoT.CashAcceptor.Commands.CashInStartCommand.PayloadData payload, 
                                                                                             CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/CashInDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.CashInCompletion.PayloadData> CashIn(ICashInEvents events, 
                                                                                   XFS4IoT.CashAcceptor.Commands.CashInCommand.PayloadData payload, 
                                                                                   CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/CashInEndDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.CashInEndCompletion.PayloadData> CashInEnd(ICashInEndEvents events, 
                                                                                         XFS4IoT.CashAcceptor.Commands.CashInEndCommand.PayloadData payload, 
                                                                                         CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/CashInRollbackDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.CashInRollbackCompletion.PayloadData> CashInRollback(ICashInRollbackEvents events, 
                                                                                                   XFS4IoT.CashAcceptor.Commands.CashInRollbackCommand.PayloadData payload, 
                                                                                                   CancellationToken cancellation);

        /// <summary>
        /// This command is used to change the note types the banknote reader should accept during cash-in. Only note typeswhich are to be changed need to be specified in the command payload. If an unknown note type is given the [completion code](#api.messagetypes.completionmessages.completioncodes)*unsupportedData* will be returned.The values set by this command are persistent.
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.ConfigureNoteTypesCompletion.PayloadData> ConfigureNoteTypes(IConfigureNoteTypesEvents events, 
                                                                                                           XFS4IoT.CashAcceptor.Commands.ConfigureNoteTypesCommand.PayloadData payload, 
                                                                                                           CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/CreateSignatureDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.CreateSignatureCompletion.PayloadData> CreateSignature(ICreateSignatureEvents events, 
                                                                                                     XFS4IoT.CashAcceptor.Commands.CreateSignatureCommand.PayloadData payload, 
                                                                                                     CancellationToken cancellation);

        /// <summary>
        /// This command is used to configure the currencydescription configuration data into the banknote reader module. Theformat and location of the configuration data is vendor and/or hardwaredependent.
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.ConfigureNoteReaderCompletion.PayloadData> ConfigureNoteReader(IConfigureNoteReaderEvents events, 
                                                                                                             XFS4IoT.CashAcceptor.Commands.ConfigureNoteReaderCommand.PayloadData payload, 
                                                                                                             CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/CompareSignatureDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.CompareSignatureCompletion.PayloadData> CompareSignature(ICompareSignatureEvents events, 
                                                                                                       XFS4IoT.CashAcceptor.Commands.CompareSignatureCommand.PayloadData payload, 
                                                                                                       CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/ReplenishDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.ReplenishCompletion.PayloadData> Replenish(IReplenishEvents events, 
                                                                                         XFS4IoT.CashAcceptor.Commands.ReplenishCommand.PayloadData payload, 
                                                                                         CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/CashUnitCountDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.CashUnitCountCompletion.PayloadData> CashUnitCount(ICashUnitCountEvents events, 
                                                                                                 XFS4IoT.CashAcceptor.Commands.CashUnitCountCommand.PayloadData payload, 
                                                                                                 CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/DeviceLockControlDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.DeviceLockControlCompletion.PayloadData> DeviceLockControl(IDeviceLockControlEvents events, 
                                                                                                         XFS4IoT.CashAcceptor.Commands.DeviceLockControlCommand.PayloadData payload, 
                                                                                                         CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/PresentMediaDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.PresentMediaCompletion.PayloadData> PresentMedia(IPresentMediaEvents events, 
                                                                                               XFS4IoT.CashAcceptor.Commands.PresentMediaCommand.PayloadData payload, 
                                                                                               CancellationToken cancellation);

        /// <summary>
        /// $ref: "../Docs/DepleteDescription.md
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.DepleteCompletion.PayloadData> Deplete(IDepleteEvents events, 
                                                                                     XFS4IoT.CashAcceptor.Commands.DepleteCommand.PayloadData payload, 
                                                                                     CancellationToken cancellation);

        /// <summary>
        /// In cases where multiple bunches are to be returned under explicit shutter control, this command is used for the purpose of moving a remaining bunch to the output position explicitly before using the following commands:[CashManagement.OpenShutter](#cashmanagement.openshutter)[CashAcceptor.PresentMedia](#cashacceptor.presentmedia)The application can tell whether the additional items were left by using the[CashAcceptor.GetPresentStatus](#cashacceptor.getpresentstatus) command. This command does not affect the status of the current cash-in transaction.
        /// </summary>
        Task<XFS4IoT.CashAcceptor.Completions.PreparePresentCompletion.PayloadData> PreparePresent(IPreparePresentEvents events, 
                                                                                                   XFS4IoT.CashAcceptor.Commands.PreparePresentCommand.PayloadData payload, 
                                                                                                   CancellationToken cancellation);

        /// <summary>
        /// CashAcceptor Status
        /// </summary>
        CashAcceptorStatusClass CashAcceptorStatus { get; set; }

        /// <summary>
        /// CashAcceptor Capabilities
        /// </summary>
        CashAcceptorCapabilitiesClass CashAcceptorCapabilities { get; set; }

    }
}

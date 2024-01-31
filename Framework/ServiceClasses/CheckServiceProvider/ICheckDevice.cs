/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ICheckDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of check. 
namespace XFS4IoTFramework.Check
{
    public interface ICheckDevice : IDevice
    {

        /// <summary>
        /// $ref: ../Docs/GetTransactionStatusDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.GetTransactionStatusCompletion.PayloadData> GetTransactionStatus(IGetTransactionStatusEvents events, 
                                                                                                        CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/MediaInDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.MediaInCompletion.PayloadData> MediaIn(IMediaInEvents events, 
                                                                              XFS4IoT.Check.Commands.MediaInCommand.PayloadData payload, 
                                                                              CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/MediaInEndDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.MediaInEndCompletion.PayloadData> MediaInEnd(IMediaInEndEvents events, 
                                                                                    CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/MediaInRollbackDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.MediaInRollbackCompletion.PayloadData> MediaInRollback(IMediaInRollbackEvents events, 
                                                                                              CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/ReadImageDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.ReadImageCompletion.PayloadData> ReadImage(IReadImageEvents events, 
                                                                                  XFS4IoT.Check.Commands.ReadImageCommand.PayloadData payload, 
                                                                                  CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/PresentMediaDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.PresentMediaCompletion.PayloadData> PresentMedia(IPresentMediaEvents events, 
                                                                                        XFS4IoT.Check.Commands.PresentMediaCommand.PayloadData payload, 
                                                                                        CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/RetractMediaDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.RetractMediaCompletion.PayloadData> RetractMedia(IRetractMediaEvents events, 
                                                                                        XFS4IoT.Check.Commands.RetractMediaCommand.PayloadData payload, 
                                                                                        CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/ResetDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.ResetCompletion.PayloadData> Reset(IResetEvents events, 
                                                                          XFS4IoT.Check.Commands.ResetCommand.PayloadData payload, 
                                                                          CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/GetNextItemDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.GetNextItemCompletion.PayloadData> GetNextItem(IGetNextItemEvents events, 
                                                                                      CancellationToken cancellation);

        /// <summary>
        /// This command is used to cause the predefined actions (move item to destination, stamping, endorsing, re-imaging) to beexecuted on the current media item. This command only applies to devices without stackers and on devices with stackers this command is notsupported.
        /// </summary>
        Task<XFS4IoT.Check.Completions.ActionItemCompletion.PayloadData> ActionItem(IActionItemEvents events, 
                                                                                    CancellationToken cancellation);

        /// <summary>
        /// The media that has been presented to the customer will be expelled out of the device.This command completes after the bunch has been expelled from the device.This command does not end the current media-in transaction. The application must deal with any media remaining within the device, e.g.,by using the [Check.MediaInRollBack](#check.mediainrollback), [Check.MediaInEnd](#check.mediainend), or [Check.RetractMedia](#check.retractmedia) command.
        /// </summary>
        Task<XFS4IoT.Check.Completions.ExpelMediaCompletion.PayloadData> ExpelMedia(IExpelMediaEvents events, 
                                                                                    CancellationToken cancellation);

        /// <summary>
        /// This command is used by applications to indicate if the current media item should be accepted or refused. Applications only usethis command when the [Check.MediaIn](#check.mediain) command is used in the mode where the application can decide if each physically acceptablemedia item should be accepted or refused, i.e. [applicationRefuse](#check.mediain.command.description.applicationrefuse) is true.
        /// </summary>
        Task<XFS4IoT.Check.Completions.AcceptItemCompletion.PayloadData> AcceptItem(IAcceptItemEvents events, 
                                                                                    XFS4IoT.Check.Commands.AcceptItemCommand.PayloadData payload, 
                                                                                    CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/SupplyReplenishDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.SupplyReplenishCompletion.PayloadData> SupplyReplenish(ISupplyReplenishEvents events, 
                                                                                              XFS4IoT.Check.Commands.SupplyReplenishCommand.PayloadData payload, 
                                                                                              CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/SetMediaParametersDescription.md
        /// </summary>
        Task<XFS4IoT.Check.Completions.SetMediaParametersCompletion.PayloadData> SetMediaParameters(ISetMediaParametersEvents events, 
                                                                                                    XFS4IoT.Check.Commands.SetMediaParametersCommand.PayloadData payload, 
                                                                                                    CancellationToken cancellation);

    }
}

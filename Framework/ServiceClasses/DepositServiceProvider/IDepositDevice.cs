/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * IDepositDevice.cs uses automatically generated parts.
\***********************************************************************************************/


using System.Threading;
using System.Threading.Tasks;
using XFS4IoTServer;

// KAL specific implementation of deposit. 
namespace XFS4IoTFramework.Deposit
{
    public interface IDepositDevice : IDevice
    {

        /// <summary>
        /// $ref: ../Docs/EntryDescription.md
        /// </summary>
        Task<XFS4IoT.Deposit.Completions.EntryCompletion.PayloadData> Entry(IEntryEvents events, 
                                                                            XFS4IoT.Deposit.Commands.EntryCommand.PayloadData payload, 
                                                                            CancellationToken cancellation);

        /// <summary>
        /// This command is used to dispense an envelope from the envelope supply. This command will either action thedispensing of an envelope from the envelope supply or will unlock the envelope supply for manual access.
        /// </summary>
        Task<XFS4IoT.Deposit.Completions.DispenseCompletion.PayloadData> Dispense(IDispenseEvents events, 
                                                                                  CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/ResetDescription.md
        /// </summary>
        Task<XFS4IoT.Deposit.Completions.ResetCompletion.PayloadData> Reset(IResetEvents events, 
                                                                            XFS4IoT.Deposit.Commands.ResetCommand.PayloadData payload, 
                                                                            CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/RetractDescription.md
        /// </summary>
        Task<XFS4IoT.Deposit.Completions.RetractCompletion.PayloadData> Retract(IRetractEvents events, 
                                                                                XFS4IoT.Deposit.Commands.RetractCommand.PayloadData payload, 
                                                                                CancellationToken cancellation);

        /// <summary>
        /// $ref: ../Docs/SupplyReplenishDescription.md
        /// </summary>
        Task SupplyReplenish(ISupplyReplenishEvents events, 
                             XFS4IoT.Deposit.Commands.SupplyReplenishCommand.PayloadData payload, 
                             CancellationToken cancellation);

    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashDispenser.Commands;
using XFS4IoT.CashDispenser.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Storage;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashDispenser
{
    public partial class RejectHandler
    {
        private async Task<CommandResult<RejectCompletion.PayloadData>> HandleReject(IRejectEvents events, RejectCommand reject, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CashDispenserDev.RejectAsync()");

            // Find reject unit
            bool foundDestination = false;
            foreach (var _ in from unit in Storage.CashUnits
                              where unit.Value.Unit.Configuration.Types.HasFlag(CashCapabilitiesClass.TypesEnum.Reject)
                              select new { })
            {
                foundDestination = true;
            }

            if (!foundDestination)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No reject units supported for this device.");
            }

            var result = await Device.RejectAsync(new RejectCommandEvents(Storage, events), cancel);

            Logger.Log(Constants.DeviceClass, $"CashDispenserDev.RejectAsync() -> {result.CompletionCode}, {result.ErrorCode}");

            await Storage.UpdateCashAccounting(result.MovementResult);

            return new(
                result.ErrorCode is not null ? new(result.ErrorCode) : null,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IStorageService Storage { get => Provider.IsA<IStorageService>(); }
    }
}

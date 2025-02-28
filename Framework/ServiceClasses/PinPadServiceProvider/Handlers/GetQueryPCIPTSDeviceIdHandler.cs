/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using XFS4IoTServer;

namespace XFS4IoTFramework.PinPad
{
    [CommandHandlerAsync]
    public partial class GetQueryPCIPTSDeviceIdHandler
    {
        private Task<CommandResult<GetQueryPCIPTSDeviceIdCompletion.PayloadData>> HandleGetQueryPCIPTSDeviceId(IGetQueryPCIPTSDeviceIdEvents events, GetQueryPCIPTSDeviceIdCommand getQueryPCIPTSDeviceId, CancellationToken cancel)
        {
            return Task.FromResult(
                new CommandResult<GetQueryPCIPTSDeviceIdCompletion.PayloadData>(
                    PinPad.PCIPTSDeviceId is null ? null : new(
                        PinPad.PCIPTSDeviceId.ManufacturerIdentifier,
                        PinPad.PCIPTSDeviceId.ModelIdentifier,
                        PinPad.PCIPTSDeviceId.HardwareIdentifier,
                        PinPad.PCIPTSDeviceId.FirmwareIdentifier,
                        PinPad.PCIPTSDeviceId.ApplicationIdentifier),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}

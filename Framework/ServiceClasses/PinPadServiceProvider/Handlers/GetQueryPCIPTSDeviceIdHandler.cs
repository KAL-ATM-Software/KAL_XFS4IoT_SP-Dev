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
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.PinPad
{
    public partial class GetQueryPCIPTSDeviceIdHandler
    {
        private Task<GetQueryPCIPTSDeviceIdCompletion.PayloadData> HandleGetQueryPCIPTSDeviceId(IGetQueryPCIPTSDeviceIdEvents events, GetQueryPCIPTSDeviceIdCommand getQueryPCIPTSDeviceId, CancellationToken cancel)
        {
            return Task.FromResult(new GetQueryPCIPTSDeviceIdCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                                    null,
                                                                                    PinPad.PCIPTSDeviceId?.ManufacturerIdentifier,
                                                                                    PinPad.PCIPTSDeviceId?.ModelIdentifier,
                                                                                    PinPad.PCIPTSDeviceId?.HardwareIdentifier,
                                                                                    PinPad.PCIPTSDeviceId?.FirmwareIdentifier,
                                                                                    PinPad.PCIPTSDeviceId?.ApplicationIdentifier));
        }
    }
}

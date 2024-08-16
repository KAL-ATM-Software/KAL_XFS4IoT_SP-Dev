/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Check.Commands;
using XFS4IoT.Check.Completions;
using System.Collections.Generic;
using XFS4IoT.Check;
using XFS4IoT.Storage;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.Check
{
    public partial class GetNextItemHandler
    {
        private async Task<CommandResult<GetNextItemCompletion.PayloadData>> HandleGetNextItem(IGetNextItemEvents events, GetNextItemCommand getNextItem, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CheckDev.GetNextItemAsync()");

            var result = await Device.GetNextItemAsync(
                events: new GetNextItemCommandEvents(Check, events),
                request: new(),
                cancellation: cancel);

            Logger.Log(Constants.DeviceClass, $"CheckDev.GetNextItemAsync() -> {result.CompletionCode}");

            return new(
                new(
                    ErrorCode: result.ErrorCode,
                    MediaFeeder: Device.CheckScannerStatus.MediaFeeder switch
                    {
                        XFS4IoTFramework.Common.CheckScannerStatusClass.MediaFeederEnum.Empty => MediaFeederEnum.Empty,
                        XFS4IoTFramework.Common.CheckScannerStatusClass.MediaFeederEnum.NotEmpty => MediaFeederEnum.NotEmpty,
                        XFS4IoTFramework.Common.CheckScannerStatusClass.MediaFeederEnum.Inoperative => MediaFeederEnum.Inoperative,
                        XFS4IoTFramework.Common.CheckScannerStatusClass.MediaFeederEnum.Unknown => MediaFeederEnum.Unknown,
                        _ => null,
                    }),
                CompletionCode: result.CompletionCode,
                ErrorDescription: result.ErrorDescription);
        }
    }
}

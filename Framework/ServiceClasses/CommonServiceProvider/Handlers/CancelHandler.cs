/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * CancelHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;

namespace XFS4IoTFramework.Common
{
    [CommandHandlerAsync]
    public partial class CancelHandler
    {

        private async Task<CancelCompletion.PayloadData> HandleCancel(ICancelEvents events, CancelCommand cancelCommand, CancellationToken cancel)
        {
            var cancelCmd = cancelCommand.IsA<CancelCommand>($"Invalid parameter in the Cancel Handle method. {nameof(CancelCommand)}");
            cancelCmd.Header.RequestId.HasValue.IsTrue();

            var dispatcher = Provider.IsA<ICommandDispatcher>();

            bool result = await dispatcher.CancelCommandsAsync(Connection, cancelCmd.Payload.RequestIds);

            return result ?
                new(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.Success, null)
                : new(XFS4IoT.Completions.MessagePayload.CompletionCodeEnum.CommandErrorCode, null, CancelCompletion.PayloadData.ErrorCodeEnum.NoMatchingRequestIDs);
        }

    }
}

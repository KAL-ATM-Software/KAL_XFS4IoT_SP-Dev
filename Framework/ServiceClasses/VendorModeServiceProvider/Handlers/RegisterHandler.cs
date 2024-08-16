/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.VendorMode.Commands;
using XFS4IoT.VendorMode.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.VendorMode
{
    public partial class RegisterHandler
    {
        private Task<CommandResult<MessagePayloadBase>> HandleRegister(IRegisterEvents events, RegisterCommand register, CancellationToken cancel)
        {
            if (!VendorMode.RegisteredClients.ContainsKey(Connection))
            {
                // Register client to send either EnterModeRequestEvent or ExitModeRequestEvent
                VendorMode.RegisteredClients.Add(Connection, register.Payload.AppName);
            }

            return Task.FromResult(
                new CommandResult<MessagePayloadBase>(MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}

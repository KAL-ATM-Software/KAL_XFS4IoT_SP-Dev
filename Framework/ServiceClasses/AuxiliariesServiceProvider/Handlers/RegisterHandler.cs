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
using XFS4IoT.Auxiliaries.Commands;
using XFS4IoT.Auxiliaries.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Auxiliaries
{
    public partial class RegisterHandler
    {
        private Task<RegisterCompletion.PayloadData> HandleRegister(IRegisterEvents events, RegisterCommand register, CancellationToken cancel)
        {
            return Task.FromResult(new RegisterCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success, $"The Reginster command is an obsolete command since 2023-2 specification."));
        }
    }
}

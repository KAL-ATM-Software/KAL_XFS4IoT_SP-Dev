/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Common.Commands;
using XFS4IoT.Common.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Common
{
    public partial class PowerSaveControlHandler
    {
        [Obsolete("This power save control command is not longer supported in the framework. Migrate it in the PowerManagement interface.")]
        private async Task<CommandResult<MessagePayloadBase>> HandlePowerSaveControl(IPowerSaveControlEvents events, PowerSaveControlCommand powerSaveControl, CancellationToken cancel)
        {
            throw new NotSupportedException($"Common.PowerSaveControl command is not supported. Use PowerManagement.PowerSaveControl command instead.");
        }
    }
}

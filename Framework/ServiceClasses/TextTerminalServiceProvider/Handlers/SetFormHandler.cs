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
using XFS4IoT.TextTerminal.Commands;
using XFS4IoT.TextTerminal.Completions;

namespace XFS4IoTFramework.TextTerminal
{
    public partial class SetFormHandler
    {
        private Task<CommandResult<SetFormCompletion.PayloadData>> HandleSetForm(ISetFormEvents events, SetFormCommand setForm, CancellationToken cancel)
        {
            throw new NotSupportedException("SetForm is not supported by the Framework.");
        }
    }
}

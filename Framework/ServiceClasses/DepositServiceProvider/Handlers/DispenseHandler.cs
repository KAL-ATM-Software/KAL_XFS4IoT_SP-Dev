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
using XFS4IoT.Deposit.Commands;
using XFS4IoT.Deposit.Completions;

namespace XFS4IoTFramework.Deposit
{
    public partial class DispenseHandler
    {
        private Task<CommandResult<DispenseCompletion.PayloadData>> HandleDispense(IDispenseEvents events, DispenseCommand dispense, CancellationToken cancel)
        {
            throw new NotImplementedException("HandleDispense for Deposit is not implemented in DispenseHandler.cs");
        }
    }
}

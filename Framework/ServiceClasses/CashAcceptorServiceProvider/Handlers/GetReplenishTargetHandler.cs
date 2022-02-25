/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetReplenishTargetHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class GetReplenishTargetHandler
    {

        private Task<GetReplenishTargetCompletion.PayloadData> HandleGetReplenishTarget(IGetReplenishTargetEvents events, GetReplenishTargetCommand getReplenishTarget, CancellationToken cancel)
        {
            //ToDo: Implement HandleGetReplenishTarget for CashAcceptor.
            
            #if DEBUG
                throw new NotImplementedException("HandleGetReplenishTarget for CashAcceptor is not implemented in GetReplenishTargetHandler.cs");
            #else
                #error HandleGetReplenishTarget for CashAcceptor is not implemented in GetReplenishTargetHandler.cs
            #endif
        }

    }
}

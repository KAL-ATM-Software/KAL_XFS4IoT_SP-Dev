/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetPresentStatusHandler.cs uses automatically generated parts.
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
    public partial class GetPresentStatusHandler
    {

        private Task<GetPresentStatusCompletion.PayloadData> HandleGetPresentStatus(IGetPresentStatusEvents events, GetPresentStatusCommand getPresentStatus, CancellationToken cancel)
        {
            //ToDo: Implement HandleGetPresentStatus for CashAcceptor.
            
            #if DEBUG
                throw new NotImplementedException("HandleGetPresentStatus for CashAcceptor is not implemented in GetPresentStatusHandler.cs");
            #else
                #error HandleGetPresentStatus for CashAcceptor is not implemented in GetPresentStatusHandler.cs
            #endif
        }

    }
}

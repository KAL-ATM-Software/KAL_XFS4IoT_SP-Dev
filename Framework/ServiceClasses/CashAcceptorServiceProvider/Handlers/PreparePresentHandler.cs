/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PreparePresentHandler.cs uses automatically generated parts.
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
    public partial class PreparePresentHandler
    {

        private Task<PreparePresentCompletion.PayloadData> HandlePreparePresent(IPreparePresentEvents events, PreparePresentCommand preparePresent, CancellationToken cancel)
        {
            //ToDo: Implement HandlePreparePresent for CashAcceptor.
            
            #if DEBUG
                throw new NotImplementedException("HandlePreparePresent for CashAcceptor is not implemented in PreparePresentHandler.cs");
            #else
                #error HandlePreparePresent for CashAcceptor is not implemented in PreparePresentHandler.cs
            #endif
        }

    }
}

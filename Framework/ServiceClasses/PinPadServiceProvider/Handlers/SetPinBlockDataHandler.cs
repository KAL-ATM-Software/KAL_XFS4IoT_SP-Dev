/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * SetPinBlockDataHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;

namespace XFS4IoTFramework.PinPad
{
    public partial class SetPinBlockDataHandler
    {

        private Task<SetPinBlockDataCompletion.PayloadData> HandleSetPinBlockData(ISetPinBlockDataEvents events, SetPinBlockDataCommand setPinBlockData, CancellationToken cancel)
        {
            //ToDo: Implement HandleSetPinBlockData for PinPad.
            
            #if DEBUG
                throw new NotImplementedException("HandleSetPinBlockData for PinPad is not implemented in SetPinBlockDataHandler.cs");
            #else
                #error HandleSetPinBlockData for PinPad is not implemented in SetPinBlockDataHandler.cs
            #endif
        }

    }
}

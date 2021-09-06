/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * MaintainPinHandler.cs uses automatically generated parts.
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
    public partial class MaintainPinHandler
    {

        private Task<MaintainPinCompletion.PayloadData> HandleMaintainPin(IMaintainPinEvents events, MaintainPinCommand maintainPin, CancellationToken cancel)
        {
            //ToDo: Implement HandleMaintainPin for PinPad.
            
            #if DEBUG
                throw new NotImplementedException("HandleMaintainPin for PinPad is not implemented in MaintainPinHandler.cs");
            #else
                #error HandleMaintainPin for PinPad is not implemented in MaintainPinHandler.cs
            #endif
        }

    }
}

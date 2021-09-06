/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * LocalVisaHandler.cs uses automatically generated parts.
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
    public partial class LocalVisaHandler
    {

        private Task<LocalVisaCompletion.PayloadData> HandleLocalVisa(ILocalVisaEvents events, LocalVisaCommand localVisa, CancellationToken cancel)
        {
            //ToDo: Implement HandleLocalVisa for PinPad.
            
            #if DEBUG
                throw new NotImplementedException("HandleLocalVisa for PinPad is not implemented in LocalVisaHandler.cs");
            #else
                #error HandleLocalVisa for PinPad is not implemented in LocalVisaHandler.cs
            #endif
        }

    }
}

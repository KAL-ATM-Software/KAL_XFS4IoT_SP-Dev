/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetMediaListHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;

namespace XFS4IoTFramework.Printer
{
    public partial class GetMediaListHandler
    {

        private Task<GetMediaListCompletion.PayloadData> HandleGetMediaList(IGetMediaListEvents events, GetMediaListCommand getMediaList, CancellationToken cancel)
        {
            //ToDo: Implement HandleGetMediaList for Printer.
            
            #if DEBUG
                throw new NotImplementedException("HandleGetMediaList for Printer is not implemented in GetMediaListHandler.cs");
            #else
                #error HandleGetMediaList for Printer is not implemented in GetMediaListHandler.cs
            #endif
        }

    }
}

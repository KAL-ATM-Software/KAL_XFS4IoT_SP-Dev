/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT MixedMedia interface.
 * SetModeHandler.cs uses automatically generated parts.
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.MixedMedia.Commands;
using XFS4IoT.MixedMedia.Completions;

namespace XFS4IoTFramework.MixedMedia
{
    public partial class SetModeHandler
    {

        private Task<SetModeCompletion.PayloadData> HandleSetMode(ISetModeEvents events, SetModeCommand setMode, CancellationToken cancel)
        {
            //ToDo: Implement HandleSetMode for MixedMedia.
            throw new NotImplementedException("HandleSetMode for MixedMedia is not implemented in SetModeHandler.cs");
        }

    }
}

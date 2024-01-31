/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * ClearAutoStartUpTime_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Auxiliaries.Commands
{
    //Original name = ClearAutoStartUpTime
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Auxiliaries.ClearAutoStartUpTime")]
    public sealed class ClearAutoStartUpTimeCommand : Command<MessagePayload>
    {
        public ClearAutoStartUpTimeCommand(int RequestId, int Timeout)
            : base(RequestId, null, Timeout)
        { }

    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * ActionItem_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Check.Commands
{
    //Original name = ActionItem
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Check.ActionItem")]
    public sealed class ActionItemCommand : Command<MessagePayload>
    {
        public ActionItemCommand()
            : base()
        { }

        public ActionItemCommand(int RequestId, int Timeout)
            : base(RequestId, null, Timeout)
        { }

    }
}

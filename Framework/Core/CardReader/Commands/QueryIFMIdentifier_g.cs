/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * QueryIFMIdentifier_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = QueryIFMIdentifier
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CardReader.QueryIFMIdentifier")]
    public sealed class QueryIFMIdentifierCommand : Command<MessagePayload>
    {
        public QueryIFMIdentifierCommand(int RequestId, int Timeout)
            : base(RequestId, null, Timeout)
        { }

    }
}

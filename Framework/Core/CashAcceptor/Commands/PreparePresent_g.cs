/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * PreparePresent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = PreparePresent
    [DataContract]
    [Command(Name = "CashAcceptor.PreparePresent")]
    public sealed class PreparePresentCommand : Command<PreparePresentCommand.PayloadData>
    {
        public PreparePresentCommand(int RequestId, PreparePresentCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, CashManagement.OutputPositionEnum? Position = null)
                : base(Timeout)
            {
                this.Position = Position;
            }

            [DataMember(Name = "position")]
            public CashManagement.OutputPositionEnum? Position { get; init; }

        }
    }
}

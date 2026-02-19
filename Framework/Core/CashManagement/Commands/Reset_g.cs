/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = Reset
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashManagement.Reset")]
    public sealed class ResetCommand : Command<ResetCommand.PayloadData>
    {
        public ResetCommand()
            : base()
        { }

        public ResetCommand(int RequestId, ResetCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ItemTargetDataClass Position = null)
                : base()
            {
                this.Position = Position;
            }

            /// <summary>
            /// Defines where items are to be moved to as one of the following:
            /// 
            /// * A single storage unit, further specified by *unit*.
            /// * Internal areas of the device.
            /// * An output position.
            /// 
            /// This may be null if the Service is to determine where items are to be moved.
            /// </summary>
            [DataMember(Name = "position")]
            public ItemTargetDataClass Position { get; init; }

        }
    }
}

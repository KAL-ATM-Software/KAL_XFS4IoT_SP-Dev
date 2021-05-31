/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = Reset
    [DataContract]
    [Command(Name = "CardReader.Reset")]
    public sealed class ResetCommand : Command<ResetCommand.PayloadData>
    {
        public ResetCommand(int RequestId, ResetCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, ResetInEnum? ResetIn = null)
                : base(Timeout)
            {
                this.ResetIn = ResetIn;
            }

            public enum ResetInEnum
            {
                Eject,
                Retain,
                NoAction
            }

            /// <summary>
            /// Specifies the action to be performed on any user card found within the ID card unit as one of the
            /// following:
            /// 
            /// * ```eject``` - Eject any card found.
            /// * ```retain``` - Retain any card found.
            /// * ```noAction``` - No action should be performed on any card found.
            /// </summary>
            [DataMember(Name = "resetIn")]
            public ResetInEnum? ResetIn { get; private set; }

        }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * Reset_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Deposit.Commands
{
    //Original name = Reset
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "Deposit.Reset")]
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

            public PayloadData(DepMediaControlEnum? DepMediaControl = null)
                : base()
            {
                this.DepMediaControl = DepMediaControl;
            }

            public enum DepMediaControlEnum
            {
                Eject,
                Retract
            }

            /// <summary>
            /// Specifies the action that should be done if deposited media is detected during the reset operation.
            /// 
            /// If null, the Service will go through default actions to clear the deposit transport. The envelope
            /// dispenser will go through the most effective means to clear any jammed media.
            /// 
            /// If not null, it must be one of the following:
            /// 
            /// * ```eject``` - Any media detected in the device should be ejected (depending on the hardware).
            /// * ```retract``` - Any media detected in the device should be deposited into the deposit container during
            ///   the reset operation.
            /// <example>retract</example>
            /// </summary>
            [DataMember(Name = "depMediaControl")]
            public DepMediaControlEnum? DepMediaControl { get; init; }

        }
    }
}

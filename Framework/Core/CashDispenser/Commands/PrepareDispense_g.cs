/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * PrepareDispense_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = PrepareDispense
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "CashDispenser.PrepareDispense")]
    public sealed class PrepareDispenseCommand : Command<PrepareDispenseCommand.PayloadData>
    {
        public PrepareDispenseCommand(int RequestId, PrepareDispenseCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ActionEnum? Action = null)
                : base()
            {
                this.Action = Action;
            }

            public enum ActionEnum
            {
                Start,
                Stop
            }

            /// <summary>
            /// A value specifying the type of actions. Following values are possible:
            /// 
            /// * ```start``` - Initiates the action to prepare for the next dispense command. This command does not wait
            ///   until the device is ready to dispense before returning a completion, it completes as soon as the
            ///   preparation has been initiated.
            /// * ```stop``` - Stops the previously activated dispense preparation. For example, the motor of the transport
            ///   will be stopped. This should be used if for some reason the subsequent dispense operation is no longer
            ///   required.
            /// </summary>
            [DataMember(Name = "action")]
            public ActionEnum? Action { get; init; }

        }
    }
}

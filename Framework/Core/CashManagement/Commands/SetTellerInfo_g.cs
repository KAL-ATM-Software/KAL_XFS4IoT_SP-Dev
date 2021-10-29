/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * SetTellerInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = SetTellerInfo
    [DataContract]
    [Command(Name = "CashManagement.SetTellerInfo")]
    public sealed class SetTellerInfoCommand : Command<SetTellerInfoCommand.PayloadData>
    {
        public SetTellerInfoCommand(int RequestId, SetTellerInfoCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, ActionEnum? Action = null, TellerDetailsClass TellerDetails = null)
                : base(Timeout)
            {
                this.Action = Action;
                this.TellerDetails = TellerDetails;
            }

            public enum ActionEnum
            {
                Create,
                Modify,
                Delete
            }

            /// <summary>
            /// The action to be performed. Following values are possible:
            /// 
            /// * ```create``` - A teller is to be added.
            /// * ```modify``` - Information about an existing teller is to be modified.
            /// * ```delete``` - A teller is to be removed.
            /// </summary>
            [DataMember(Name = "action")]
            public ActionEnum? Action { get; init; }

            /// <summary>
            /// Teller details object.
            /// </summary>
            [DataMember(Name = "tellerDetails")]
            public TellerDetailsClass TellerDetails { get; init; }

        }
    }
}

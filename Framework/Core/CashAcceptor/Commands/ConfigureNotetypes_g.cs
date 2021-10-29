/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * ConfigureNoteTypes_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = ConfigureNoteTypes
    [DataContract]
    [Command(Name = "CashAcceptor.ConfigureNoteTypes")]
    public sealed class ConfigureNoteTypesCommand : Command<ConfigureNoteTypesCommand.PayloadData>
    {
        public ConfigureNoteTypesCommand(int RequestId, ConfigureNoteTypesCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, Dictionary<string, CashManagement.BankNoteClass> Items = null)
                : base(Timeout)
            {
                this.Items = Items;
            }

            /// <summary>
            /// An object listing which cash items the device is capable of handling and whether the cash items
            /// are enabled for acceptance.
            /// </summary>
            [DataMember(Name = "items")]
            public Dictionary<string, CashManagement.BankNoteClass> Items { get; init; }

        }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
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

            public PayloadData(int Timeout, List<ItemsClass> Items = null)
                : base(Timeout)
            {
                this.Items = Items;
            }

            [DataContract]
            public sealed class ItemsClass
            {
                public ItemsClass(string Item = null, bool? Enabled = null)
                {
                    this.Item = Item;
                    this.Enabled = Enabled;
                }

                /// <summary>
                /// A cash item as reported by [CashManagement.GetBankNoteTypes](#cashmanagement.getbanknotetypes). This is not 
                /// specified if the item was not identified as a cash item.
                /// <example>type20USD1</example>
                /// </summary>
                [DataMember(Name = "item")]
                [DataTypes(Pattern = @"^type[0-9A-Z]+$")]
                public string Item { get; init; }

                /// <summary>
                /// If true the banknote reader will accept this note type during a cash-in operations.
                /// If false the banknote reader will refuse this note type, unless it must be retained by note classification 
                /// rules.
                /// </summary>
                [DataMember(Name = "enabled")]
                public bool? Enabled { get; init; }

            }

            /// <summary>
            /// An array which specifies which note types are to be disabled or re-enabled.
            /// </summary>
            [DataMember(Name = "items")]
            public List<ItemsClass> Items { get; init; }

        }
    }
}

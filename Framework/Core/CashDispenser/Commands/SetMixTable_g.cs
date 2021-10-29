/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * SetMixTable_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = SetMixTable
    [DataContract]
    [Command(Name = "CashDispenser.SetMixTable")]
    public sealed class SetMixTableCommand : Command<SetMixTableCommand.PayloadData>
    {
        public SetMixTableCommand(int RequestId, SetMixTableCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? MixNumber = null, string Name = null, List<Dictionary<string, int>> MixRows = null)
                : base(Timeout)
            {
                this.MixNumber = MixNumber;
                this.Name = Name;
                this.MixRows = MixRows;
            }

            /// <summary>
            /// Number identifying the house mix table.
            /// <example>21</example>
            /// </summary>
            [DataMember(Name = "mixNumber")]
            [DataTypes(Minimum = 1)]
            public int? MixNumber { get; init; }

            /// <summary>
            /// Name of the house mix table.
            /// <example>House mix 21</example>
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; init; }

            /// <summary>
            /// Array of rows of the mix table.
            /// </summary>
            [DataMember(Name = "mixRows")]
            public List<Dictionary<string, int>> MixRows { get; init; }

        }
    }
}

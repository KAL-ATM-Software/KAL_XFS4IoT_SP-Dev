/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * Count_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashDispenser.Commands
{
    //Original name = Count
    [DataContract]
    [Command(Name = "CashDispenser.Count")]
    public sealed class CountCommand : Command<CountCommand.PayloadData>
    {
        public CountCommand(int RequestId, CountCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Unit = null, CashManagement.OutputPositionEnum? Position = null)
                : base(Timeout)
            {
                this.Unit = Unit;
                this.Position = Position;
            }

            /// <summary>
            /// Specifies the unit to empty or that all units are to be emptied. Following values are possible:
            /// 
            ///   * ```all``` - All units are to be emptied.
            ///   * ```&lt;storage unit identifier&gt;``` - The storage unit to be emptied as
            ///   [identifier](#storage.getstorage.completion.properties.storage.unit1).
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "unit")]
            [DataTypes(Pattern = @"^all$|^.{1,5}$")]
            public string Unit { get; init; }

            [DataMember(Name = "position")]
            public CashManagement.OutputPositionEnum? Position { get; init; }

        }
    }
}

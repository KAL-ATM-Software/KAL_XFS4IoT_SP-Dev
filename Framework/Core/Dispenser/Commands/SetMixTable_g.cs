/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * SetMixTable_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Dispenser.Commands
{
    //Original name = SetMixTable
    [DataContract]
    [Command(Name = "Dispenser.SetMixTable")]
    public sealed class SetMixTableCommand : Command<SetMixTableCommand.PayloadData>
    {
        public SetMixTableCommand(int RequestId, SetMixTableCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, int? MixNumber = null, string Name = null, List<double> MixHeader = null, List<MixRowsClass> MixRows = null)
                : base(Timeout)
            {
                this.MixNumber = MixNumber;
                this.Name = Name;
                this.MixHeader = MixHeader;
                this.MixRows = MixRows;
            }

            /// <summary>
            /// Number identifying the house mix table.
            /// </summary>
            [DataMember(Name = "mixNumber")]
            public int? MixNumber { get; private set; }

            /// <summary>
            /// Name of the house mix table.
            /// </summary>
            [DataMember(Name = "name")]
            public string Name { get; private set; }

            /// <summary>
            /// Array of floating point numbers; each element defines the value of the item corresponding to its respective column.
            /// </summary>
            [DataMember(Name = "mixHeader")]
            public List<double> MixHeader { get; private set; }

            [DataContract]
            public sealed class MixRowsClass
            {
                public MixRowsClass(double? Amount = null, List<int> Mixture = null)
                {
                    this.Amount = Amount;
                    this.Mixture = Mixture;
                }

                /// <summary>
                /// Amount denominated by this mix row.
                /// </summary>
                [DataMember(Name = "amount")]
                public double? Amount { get; private set; }

                /// <summary>
                /// A mix row, an array of integers; each element defines the quantity of each item denomination in the mix used in the denomination of *amount*. 
                /// The value of each array element is defined by the *mixHeader*.
                /// </summary>
                [DataMember(Name = "mixture")]
                public List<int> Mixture { get; private set; }

            }

            /// <summary>
            /// Array of rows of the mix table.
            /// </summary>
            [DataMember(Name = "mixRows")]
            public List<MixRowsClass> MixRows { get; private set; }

        }
    }
}

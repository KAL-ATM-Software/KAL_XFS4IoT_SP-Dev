/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * WriteRawData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CardReader.Commands
{
    //Original name = WriteRawData
    [DataContract]
    [Command(Name = "CardReader.WriteRawData")]
    public sealed class WriteRawDataCommand : Command<WriteRawDataCommand.PayloadData>
    {
        public WriteRawDataCommand(int RequestId, WriteRawDataCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, List<DataClass> Data = null)
                : base(Timeout)
            {
                this.Data = Data;
            }

            [DataContract]
            public sealed class DataClass
            {
                public DataClass(DestinationEnum? Destination = null, string Data = null, WriteMethodEnum? WriteMethod = null)
                {
                    this.Destination = Destination;
                    this.Data = Data;
                    this.WriteMethod = WriteMethod;
                }

                public enum DestinationEnum
                {
                    Track1,
                    Track2,
                    Track3,
                    Track1Front,
                    Track1JIS,
                    Track3JIS
                }

                /// <summary>
                /// Specifies where the card data is to be written to as one of the following:
                /// 
                /// * ```track1``` - data is to be written to track 1.
                /// * ```track2``` - data is to be written to track 2.
                /// * ```track3``` - data is to be written to track 3.
                /// * ```track1Front``` - data is to be written to the front track 1. In some countries this track
                ///   is known as JIS II track.
                /// * ```track1JIS``` - data is to be written to JIS I track 1 (8bits/char).
                /// * ```track3JIS``` - data is to be written to JIS I track 3 (8bits/char).
                /// </summary>
                [DataMember(Name = "destination")]
                public DestinationEnum? Destination { get; private set; }

                /// <summary>
                /// Base4 encoded representation of the data
                /// </summary>
                [DataMember(Name = "data")]
                public string Data { get; private set; }

                public enum WriteMethodEnum
                {
                    Loco,
                    Hico,
                    Auto
                }

                /// <summary>
                /// Indicates whether a low coercivity or high coercivity magnetic stripe is to be written as one of
                /// the following:
                /// 
                /// * ```loco``` - Write using low coercivity.
                /// * ```hico``` - Write using high coercivity.
                /// * ```auto``` - Service Provider will determine whether low or high coercivity is to be used.
                /// </summary>
                [DataMember(Name = "writeMethod")]
                public WriteMethodEnum? WriteMethod { get; private set; }

            }

            /// <summary>
            /// An array of card data structures
            /// </summary>
            [DataMember(Name = "data")]
            public List<DataClass> Data { get; private set; }

        }
    }
}

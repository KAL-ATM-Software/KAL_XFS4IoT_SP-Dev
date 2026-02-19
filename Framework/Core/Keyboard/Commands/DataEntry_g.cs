/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * DataEntry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Keyboard.Commands
{
    //Original name = DataEntry
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Keyboard.DataEntry")]
    public sealed class DataEntryCommand : Command<DataEntryCommand.PayloadData>
    {
        public DataEntryCommand()
            : base()
        { }

        public DataEntryCommand(int RequestId, DataEntryCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int? MaxLen = null, bool? AutoEnd = null, Dictionary<string, ActiveKeysClass> ActiveKeys = null)
                : base()
            {
                this.MaxLen = MaxLen;
                this.AutoEnd = AutoEnd;
                this.ActiveKeys = ActiveKeys;
            }

            /// <summary>
            /// Specifies the maximum number of digits which can be returned to the application in the output parameter.
            /// </summary>
            [DataMember(Name = "maxLen")]
            [DataTypes(Minimum = 0)]
            public int? MaxLen { get; init; }

            /// <summary>
            /// If *autoEnd* is set to true, the Service Provider terminates the command when the maximum number of
            /// digits are entered.
            /// Otherwise, the input is terminated by the user using one of the termination keys.
            /// *autoEnd* is ignored when *maxLen* is set to zero.
            /// </summary>
            [DataMember(Name = "autoEnd")]
            public bool? AutoEnd { get; init; }

            [DataContract]
            public sealed class ActiveKeysClass
            {
                public ActiveKeysClass(bool? Terminate = null)
                {
                    this.Terminate = Terminate;
                }

                /// <summary>
                /// The key is a terminate key.
                /// </summary>
                [DataMember(Name = "terminate")]
                public bool? Terminate { get; init; }

            }

            /// <summary>
            /// Specifies all Function Keys which are active during the execution of the command.
            /// This should be the complete set or a subset of the keys returned in the payload of the
            /// [Keyboard.GetLayout](#keyboard.getlayout) command.
            /// </summary>
            [DataMember(Name = "activeKeys")]
            public Dictionary<string, ActiveKeysClass> ActiveKeys { get; init; }

        }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * SetStorage_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Storage.Commands
{
    //Original name = SetStorage
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Storage.SetStorage")]
    public sealed class SetStorageCommand : Command<SetStorageCommand.PayloadData>
    {
        public SetStorageCommand(int RequestId, SetStorageCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(Dictionary<string, SetStorageUnitClass> Storage = null)
                : base()
            {
                this.Storage = Storage;
            }

            /// <summary>
            /// Object containing storage unit information.
            /// </summary>
            [DataMember(Name = "storage")]
            public Dictionary<string, SetStorageUnitClass> Storage { get; init; }

        }
    }
}

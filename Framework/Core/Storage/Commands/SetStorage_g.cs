/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
    [Command(Name = "Storage.SetStorage")]
    public sealed class SetStorageCommand : Command<SetStorageCommand.PayloadData>
    {
        public SetStorageCommand(int RequestId, SetStorageCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, Dictionary<string, SetStorageUnitClass> Storage = null)
                : base(Timeout)
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

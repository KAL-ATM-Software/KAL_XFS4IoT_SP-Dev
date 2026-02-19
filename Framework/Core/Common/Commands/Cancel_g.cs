/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * Cancel_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Common.Commands
{
    //Original name = Cancel
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Common.Cancel")]
    public sealed class CancelCommand : Command<CancelCommand.PayloadData>
    {
        public CancelCommand()
            : base()
        { }

        public CancelCommand(int RequestId, CancelCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<int> RequestIds = null)
                : base()
            {
                this.RequestIds = RequestIds;
            }

            /// <summary>
            /// The request(s) to be canceled.
            /// 
            /// If included, the Service will only attempt to cancel the specified command requests which are queued or
            /// executing and which are associated with the client connection on which this command is received. All
            /// other requestIds will be ignored.
            /// 
            /// If null, the Service will attempt to cancel all queued or executing command requests associated with
            /// the client connection on which this command is received.
            /// <example>[1, 2]</example>
            /// </summary>
            [DataMember(Name = "requestIds")]
            [DataTypes(Minimum = 1)]
            public List<int> RequestIds { get; init; }

        }
    }
}

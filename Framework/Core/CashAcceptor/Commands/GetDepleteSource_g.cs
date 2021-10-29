/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetDepleteSource_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = GetDepleteSource
    [DataContract]
    [Command(Name = "CashAcceptor.GetDepleteSource")]
    public sealed class GetDepleteSourceCommand : Command<GetDepleteSourceCommand.PayloadData>
    {
        public GetDepleteSourceCommand(int RequestId, GetDepleteSourceCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string CashUnitTarget = null)
                : base(Timeout)
            {
                this.CashUnitTarget = CashUnitTarget;
            }

            /// <summary>
            /// Object name of the cash unit (as stated by the [Storage.GetStorage](#storage.getstorage) 
            /// command) which would be used as the target of the depletion operation.
            /// <example>unit2</example>
            /// </summary>
            [DataMember(Name = "cashUnitTarget")]
            public string CashUnitTarget { get; init; }

        }
    }
}

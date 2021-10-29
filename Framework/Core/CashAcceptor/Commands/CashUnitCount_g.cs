/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CashUnitCount_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = CashUnitCount
    [DataContract]
    [Command(Name = "CashAcceptor.CashUnitCount")]
    public sealed class CashUnitCountCommand : Command<CashUnitCountCommand.PayloadData>
    {
        public CashUnitCountCommand(int RequestId, CashUnitCountCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, List<string> Units = null)
                : base(Timeout)
            {
                this.Units = Units;
            }

            /// <summary>
            /// Array containing the [identifiers](#storage.getstorage.completion.properties.storage) of the individual 
            /// cash storage units to be counted. 
            /// If an invalid storage unit is contained in this list, the command will fail with a ```cashUnitError```
            /// [errorCode](#cashacceptor.cashunitcount.completion.properties.errorcode).
            /// 
            /// <example>unit1</example>
            /// </summary>
            [DataMember(Name = "units")]
            public List<string> Units { get; init; }

        }
    }
}

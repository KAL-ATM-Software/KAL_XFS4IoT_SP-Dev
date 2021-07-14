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

            public PayloadData(int Timeout, List<int> CUNumList = null)
                : base(Timeout)
            {
                this.CUNumList = CUNumList;
            }

            /// <summary>
            /// Array containing the numbers of the individual cash units to be counted. 
            /// If an invalid number is contained in this list, the command will fail with a CashUnitError error.
            /// 
            /// </summary>
            [DataMember(Name = "cUNumList")]
            public List<int> CUNumList { get; init; }

        }
    }
}

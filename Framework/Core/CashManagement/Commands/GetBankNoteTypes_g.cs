/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetBankNoteTypes_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = GetBankNoteTypes
    [DataContract]
    [Command(Name = "CashManagement.GetBankNoteTypes")]
    public sealed class GetBankNoteTypesCommand : Command<GetBankNoteTypesCommand.PayloadData>
    {
        public GetBankNoteTypesCommand(int RequestId, GetBankNoteTypesCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout)
                : base(Timeout)
            {
            }

        }
    }
}

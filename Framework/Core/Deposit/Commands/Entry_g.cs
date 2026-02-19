/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * Entry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Deposit.Commands
{
    //Original name = Entry
    [DataContract]
    [XFS4Version(Version = "1.0")]
    [Command(Name = "Deposit.Entry")]
    public sealed class EntryCommand : Command<EntryCommand.PayloadData>
    {
        public EntryCommand()
            : base()
        { }

        public EntryCommand(int RequestId, EntryCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string PrintData = null)
                : base()
            {
                this.PrintData = PrintData;
            }

            /// <summary>
            /// Specifies the data that will be printed on the envelope that is entered by the customer. If the data is
            /// longer than [maxNumChars](#common.capabilities.completion.description.deposit.printer.maxnumchars), then
            /// *invalidData* will be returned.
            /// <example>Data to be printed on the envelope</example>
            /// </summary>
            [DataMember(Name = "printData")]
            public string PrintData { get; init; }

        }
    }
}

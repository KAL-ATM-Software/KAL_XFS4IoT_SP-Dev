/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetBankNoteTypes_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashManagement.GetBankNoteTypes")]
    public sealed class GetBankNoteTypesCompletion : Completion<GetBankNoteTypesCompletion.PayloadData>
    {
        public GetBankNoteTypesCompletion()
            : base()
        { }

        public GetBankNoteTypesCompletion(int RequestId, GetBankNoteTypesCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(Dictionary<string, BankNoteClass> Items = null)
                : base()
            {
                this.Items = Items;
            }

            /// <summary>
            /// An object listing which cash items the device is capable of handling and whether the cash items
            /// are enabled for acceptance. May be null if empty.
            /// </summary>
            [DataMember(Name = "items")]
            public Dictionary<string, BankNoteClass> Items { get; init; }

        }
    }
}

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
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.GetBankNoteTypes")]
    public sealed class GetBankNoteTypesCompletion : Completion<GetBankNoteTypesCompletion.PayloadData>
    {
        public GetBankNoteTypesCompletion(int RequestId, GetBankNoteTypesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, Dictionary<string, BankNoteClass> Items = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Items = Items;
            }

            /// <summary>
            /// An object listing which cash items the device is capable of handling and whether the cash items
            /// are enabled for acceptance.
            /// </summary>
            [DataMember(Name = "items")]
            public Dictionary<string, BankNoteClass> Items { get; init; }

        }
    }
}

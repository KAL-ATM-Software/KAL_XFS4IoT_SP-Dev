/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetBanknoteTypes_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetBanknoteTypes")]
    public sealed class GetBanknoteTypesCompletion : Completion<GetBanknoteTypesCompletion.PayloadData>
    {
        public GetBanknoteTypesCompletion(int RequestId, GetBanknoteTypesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<NoteTypesClass> NoteTypes = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.NoteTypes = NoteTypes;
            }

            [DataContract]
            public sealed class NoteTypesClass
            {
                public NoteTypesClass(int? NoteID = null, string CurrencyID = null, double? Values = null, int? Release = null, bool? Configured = null)
                {
                    this.NoteID = NoteID;
                    this.CurrencyID = CurrencyID;
                    this.Values = Values;
                    this.Release = Release;
                    this.Configured = Configured;
                }

                /// <summary>
                /// Identification of note type.
                /// </summary>
                [DataMember(Name = "noteID")]
                public int? NoteID { get; init; }

                /// <summary>
                /// Currency ID in ISO 4217 format [Ref. 2].
                /// </summary>
                [DataMember(Name = "currencyID")]
                public string CurrencyID { get; init; }

                /// <summary>
                /// The value of a single item expressed as floating point value.
                /// </summary>
                [DataMember(Name = "values")]
                public double? Values { get; init; }

                /// <summary>
                /// The release of the banknote type. The higher this number is, the newer the release. Zero means that there is only one release of that banknote type. This value has not been standardized and therefore a release number of the same banknote will not necessarily have the same value in different systems.
                /// </summary>
                [DataMember(Name = "release")]
                public int? Release { get; init; }

                /// <summary>
                /// If TRUE the banknote reader will accept this note type during a cash-in operation, if FALSE the banknote reader will refuse this note type unless it must be retained by note classification rules.
                /// </summary>
                [DataMember(Name = "configured")]
                public bool? Configured { get; init; }

            }

            /// <summary>
            /// List of banknote types the banknote reader supports.
            /// </summary>
            [DataMember(Name = "noteTypes")]
            public List<NoteTypesClass> NoteTypes { get; init; }

        }
    }
}

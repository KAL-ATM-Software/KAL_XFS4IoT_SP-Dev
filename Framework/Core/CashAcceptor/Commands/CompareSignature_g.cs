/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * CompareSignature_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashAcceptor.Commands
{
    //Original name = CompareSignature
    [DataContract]
    [Command(Name = "CashAcceptor.CompareSignature")]
    public sealed class CompareSignatureCommand : Command<CompareSignatureCommand.PayloadData>
    {
        public CompareSignatureCommand(int RequestId, CompareSignatureCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, List<CashManagement.SignatureClass> ReferenceSignatures = null, List<CashManagement.SignatureClass> Signatures = null)
                : base(Timeout)
            {
                this.ReferenceSignatures = ReferenceSignatures;
                this.Signatures = Signatures;
            }

            /// <summary>
            /// Array of Signature structures.
            /// Each structure represents the signature corresponding to one orientation of a single reference banknote.
            /// At least one orientation must be provided. If no orientations are provided (this array is missing or empty) 
            /// the command returns an invalidData error.
            /// </summary>
            [DataMember(Name = "referenceSignatures")]
            public List<CashManagement.SignatureClass> ReferenceSignatures { get; init; }

            /// <summary>
            /// Array of Signature structures. Each structure represents a signature from the cash-in
            /// transactions, to be compared with the reference signatures in _referenceSignatures_.
            /// At least one signature must be provided. If there are no signatures provided (this array is missing or 
            /// empty) the command returns an invalidData error.
            /// </summary>
            [DataMember(Name = "signatures")]
            public List<CashManagement.SignatureClass> Signatures { get; init; }

        }
    }
}

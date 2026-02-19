/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * GetActiveInterface_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.VendorApplication.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "VendorApplication.GetActiveInterface")]
    public sealed class GetActiveInterfaceCompletion : Completion<GetActiveInterfaceCompletion.PayloadData>
    {
        public GetActiveInterfaceCompletion()
            : base()
        { }

        public GetActiveInterfaceCompletion(int RequestId, GetActiveInterfaceCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ActiveInterfaceEnum? ActiveInterface = null)
                : base()
            {
                this.ActiveInterface = ActiveInterface;
            }

            public enum ActiveInterfaceEnum
            {
                Consumer,
                Operator
            }

            /// <summary>
            /// Specifies the active interface as one of the following values:
            /// 
            /// * ```consumer``` - The consumer interface.
            /// * ```operator``` - The operator interface.
            /// </summary>
            [DataMember(Name = "activeInterface")]
            public ActiveInterfaceEnum? ActiveInterface { get; init; }

        }
    }
}

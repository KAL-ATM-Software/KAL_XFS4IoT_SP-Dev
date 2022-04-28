/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT ServicePublisher interface.
 * GetServices_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.ServicePublisher.Completions
{
    [DataContract]
    [Completion(Name = "ServicePublisher.GetServices")]
    public sealed class GetServicesCompletion : Completion<GetServicesCompletion.PayloadData>
    {
        public GetServicesCompletion(int RequestId, GetServicesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string VendorName = null, List<ServiceClass> Services = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.VendorName = VendorName;
                this.Services = Services;
            }

            /// <summary>
            /// Freeform string naming the hardware vendor.
            /// <example>ACME ATM Hardware GmbH</example>
            /// </summary>
            [DataMember(Name = "vendorName")]
            public string VendorName { get; init; }

            /// <summary>
            /// Array of one or more services exposed by the publisher.
            /// </summary>
            [DataMember(Name = "services")]
            public List<ServiceClass> Services { get; init; }

        }
    }
}

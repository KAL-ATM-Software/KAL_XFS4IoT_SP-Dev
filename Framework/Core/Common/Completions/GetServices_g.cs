/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Common interface.
 * GetServices_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Common.Completions
{
    [DataContract]
    [Completion(Name = "Common.GetServices")]
    public sealed class GetServicesCompletion : Completion<GetServicesCompletion.PayloadData>
    {
        public GetServicesCompletion(int RequestId, GetServicesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string Vendorname = null, List<ServicesClass> Services = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Vendorname = Vendorname;
                this.Services = Services;
            }

            /// <summary>
            /// Freeform string naming the hardware vendor
            /// </summary>
            [DataMember(Name = "vendorname")]
            public string Vendorname { get; private set; }

            [DataContract]
            public class ServicesClass
            {
                public ServicesClass(string ServiceURI = null)
                {
                    this.ServiceURI = ServiceURI;
                }

                /// <summary>
                /// The URI which can be used to contact this individual service
                /// </summary>
                [DataMember(Name = "serviceURI")]
                public string ServiceURI { get; private set; }

            }


            [DataMember(Name = "services")]
            public List<ServicesClass> Services { get; private set; }

        }
    }
}

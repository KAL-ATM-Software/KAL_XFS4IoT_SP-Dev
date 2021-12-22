/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT ServicePublisher interface.
 * ServicePublisherSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.ServicePublisher
{

    [DataContract]
    public sealed class ServiceClass
    {
        public ServiceClass(string ServiceURI = null)
        {
            this.ServiceURI = ServiceURI;
        }

        /// <summary>
        /// The URI which can be used to contact this individual service.
        /// <example>wss://ATM1:123/xfs4iot/v1.0/CardReader</example>
        /// </summary>
        [DataMember(Name = "serviceURI")]
        public string ServiceURI { get; init; }

    }


}

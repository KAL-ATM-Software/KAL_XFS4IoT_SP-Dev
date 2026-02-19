/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessQueryApplications_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [XFS4Version(Version = "3.0")]
    [Completion(Name = "CardReader.EMVClessQueryApplications")]
    public sealed class EMVClessQueryApplicationsCompletion : Completion<EMVClessQueryApplicationsCompletion.PayloadData>
    {
        public EMVClessQueryApplicationsCompletion()
            : base()
        { }

        public EMVClessQueryApplicationsCompletion(int RequestId, EMVClessQueryApplicationsCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<AppDataClass> AppData = null)
                : base()
            {
                this.AppData = AppData;
            }

            [DataContract]
            public sealed class AppDataClass
            {
                public AppDataClass(List<byte> Aid = null, List<byte> KernelIdentifier = null)
                {
                    this.Aid = Aid;
                    this.KernelIdentifier = KernelIdentifier;
                }

                /// <summary>
                /// Contains the Base64 encoded payment system application identifier (AID) supported by the
                /// intelligent contactless card unit.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "aid")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> Aid { get; init; }

                /// <summary>
                /// Contains the Base64 encoded Kernel Identifier associated with the *aid*. This data may be null
                /// if the reader does not support Kernel Identifiers for example in the case of legacy approved
                /// contactless readers.
                /// <example>O2gAUACFyEARAJAC</example>
                /// </summary>
                [DataMember(Name = "kernelIdentifier")]
                [DataTypes(Pattern = @"^([a-zA-Z0-9+/]{4})*([a-zA-Z0-9+/]{4}|[a-zA-Z0-9+/]{2}([a-zA-Z0-9+/]|=)=)$")]
                public List<byte> KernelIdentifier { get; init; }

            }

            /// <summary>
            /// An array of application data objects which specifies a supported application identifier (AID) and associated
            /// Kernel Identifier.
            /// </summary>
            [DataMember(Name = "appData")]
            public List<AppDataClass> AppData { get; init; }

        }
    }
}

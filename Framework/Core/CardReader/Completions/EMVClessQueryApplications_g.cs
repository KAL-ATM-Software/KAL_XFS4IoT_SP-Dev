/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * EMVClessQueryApplications_g.cs uses automatically generated parts. 
 * created at 3/18/2021 2:05:35 PM
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CardReader.Completions
{
    [DataContract]
    [Completion(Name = "CardReader.EMVClessQueryApplications")]
    public sealed class EMVClessQueryApplicationsCompletion : Completion<EMVClessQueryApplicationsCompletion.PayloadData>
    {
        public EMVClessQueryApplicationsCompletion(string RequestId, EMVClessQueryApplicationsCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {
            [DataContract]
            public sealed class AppDataClass
            {
                public AppDataClass(string Aid = null, string KernelIdentifier = null)
                    : base()
                {
                    this.Aid = Aid;
                    this.KernelIdentifier = KernelIdentifier;
                }

                /// <summary>
                ///Contains the Base64 encoded payment system application identifier (AID) supported by the intelligent contactless card unit.
                /// </summary>
                [DataMember(Name = "aid")] 
                public string Aid { get; private set; }

                /// <summary>
                ///Contains the Base64 encoded Kernel Identifier associated with the *aid*. This data may be empty if the reader does not support Kernel Identifiers for example in the case of legacy approved contactless readers.
                /// </summary>
                [DataMember(Name = "kernelIdentifier")] 
                public string KernelIdentifier { get; private set; }

            }


            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<AppDataClass> AppData = null)
                : base(CompletionCode, ErrorDescription)
            {
                ErrorDescription.IsNotNullOrWhitespace($"Null or an empty value for {nameof(ErrorDescription)} in received {nameof(EMVClessQueryApplicationsCompletion.PayloadData)}");

                this.AppData = AppData;
            }

            /// <summary>
            ///An array of application data objects which specifies a supported identifier (AID) and associated Kernel Identifier.
            /// </summary>
            [DataMember(Name = "appData")] 
            public List<AppDataClass> AppData{ get; private set; }

        }
    }
}

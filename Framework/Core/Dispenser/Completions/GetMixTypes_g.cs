/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * GetMixTypes_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Dispenser.Completions
{
    [DataContract]
    [Completion(Name = "Dispenser.GetMixTypes")]
    public sealed class GetMixTypesCompletion : Completion<GetMixTypesCompletion.PayloadData>
    {
        public GetMixTypesCompletion(int RequestId, GetMixTypesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<MixTypesClass> MixTypes = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.MixTypes = MixTypes;
            }

            [DataContract]
            public sealed class MixTypesClass
            {
                public MixTypesClass(int? MixNumber = null, MixTypeEnum? MixType = null, int? SubType = null, string Name = null)
                {
                    this.MixNumber = MixNumber;
                    this.MixType = MixType;
                    this.SubType = SubType;
                    this.Name = Name;
                }

                /// <summary>
                /// Number identifying the mix algorithm or the house mix table. 
                /// This number can be passed to the Dispenser.MixTable, Dispenser.Dispense and Dispenser.Denominate commands.
                /// </summary>
                [DataMember(Name = "mixNumber")]
                public int? MixNumber { get; private set; }

                public enum MixTypeEnum
                {
                    MixAlgorithm,
                    MixTable
                }

                /// <summary>
                /// Specifies whether the mix type is an algorithm or a house mix table. Possible values are ```mixAlgorithm``` and
                /// ```mixTable```.
                /// </summary>
                [DataMember(Name = "mixType")]
                public MixTypeEnum? MixType { get; private set; }

                /// <summary>
                /// Contains a vendor-defined number that identifies the type of algorithm. 
                /// Individual vendor-defined mix algorithms are defined above hexadecimal 7FFF. 
                /// Mix algorithms which are provided by the Service are in the range hexadecimal 8000 - 8FFF. 
                /// Application defined mix algorithms start at hexadecimal 9000. All numbers below 8000 hexadecimal are reserved. 
                /// If *mixType* is \"mixTable\", this value will be zero.
                /// </summary>
                [DataMember(Name = "subType")]
                public int? SubType { get; private set; }

                /// <summary>
                /// Name of the table/algorithm used.
                /// </summary>
                [DataMember(Name = "name")]
                public string Name { get; private set; }

            }

            /// <summary>
            /// Array of mix type objects.
            /// </summary>
            [DataMember(Name = "mixTypes")]
            public List<MixTypesClass> MixTypes { get; private set; }

        }
    }
}

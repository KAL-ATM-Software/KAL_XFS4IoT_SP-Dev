/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * GetCodelineMapping_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Printer.Completions
{
    [DataContract]
    [Completion(Name = "Printer.GetCodelineMapping")]
    public sealed class GetCodelineMappingCompletion : Completion<GetCodelineMappingCompletion.PayloadData>
    {
        public GetCodelineMappingCompletion(int RequestId, GetCodelineMappingCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, CodelineFormatEnum? CodelineFormat = null, string CharMapping = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.CodelineFormat = CodelineFormat;
                this.CharMapping = CharMapping;
            }

            public enum CodelineFormatEnum
            {
                Cmc7,
                E13b
            }

            /// <summary>
            /// Specifies the code-line format that is being reported as one of the following:
            /// 
            /// * ```cmc7``` - CMC7 mapping.
            /// * ```e13b``` - E13B mapping.
            /// </summary>
            [DataMember(Name = "codelineFormat")]
            public CodelineFormatEnum? CodelineFormat { get; private set; }

            /// <summary>
            /// Defines the mapping of the font specific symbols to byte values. These byte values are used to
            /// represent the font specific characters when the code line is read through the Printer.ReadImage
            /// command. The font specific meaning of each index is defined in the following tables.
            /// 
            /// *E13B* 
            /// 
            /// | Index | Symbol | Meaning |
            /// | :-- | :-- |:-- |
            /// | 0 | ![Transit](../../Assets/e13b-transit.png "Transit") | Transit |
            /// | 1 | ![Amount](../../Assets/e13b-amount.png "Amount") | Amount |
            /// | 2 | ![On Us](../../Assets/e13b-onus.png "On Us") | On Us |
            /// | 3 | ![Dash](../../Assets/e13b-dash.png "Dash") | Dash |
            /// | 4 | N/A | Reject / Unreadable |
            /// 
            /// *CMC7* 
            /// 
            /// | Index | Symbol | Meaning |
            /// | :-- | :-- |:-- |
            /// | 0 | ![S1](../../Assets/cmc7-s1.png "S1") | S1 - Start of Bank Account |
            /// | 1 | ![S2](../../Assets/cmc7-s2.png "S2") | S2 - Start of the Amount field |
            /// | 2 | ![S3](../../Assets/cmc7-s3.png "S3") | S3 - Terminate Routing |
            /// | 3 | ![S4](../../Assets/cmc7-s4.png "S4") | S4 - Unused |
            /// | 4 | ![S5](../../Assets/cmc7-s5.png "S5") | S5 - Transit / Routing |
            /// | 5 | N/A | Reject / Unreadable |
            /// 
            /// </summary>
            [DataMember(Name = "charMapping")]
            public string CharMapping { get; private set; }

        }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * GetKeyDetail_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.TextTerminal.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "TextTerminal.GetKeyDetail")]
    public sealed class GetKeyDetailCompletion : Completion<GetKeyDetailCompletion.PayloadData>
    {
        public GetKeyDetailCompletion(int RequestId, GetKeyDetailCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(List<string> Keys = null, Dictionary<string, KeyClass> CommandKeys = null)
                : base()
            {
                this.Keys = Keys;
                this.CommandKeys = CommandKeys;
            }

            /// <summary>
            /// String array which contains the printable characters, numeric and alphanumeric keys
            /// on the Text Terminal Unit,
            /// e.g. ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "A", "B", "C", "a", "b", "c" ]
            /// if those text terminal input keys are present. This property will be null if no keys are supported.
            /// 
            /// The following prefixed key names are defined:
            /// 
            ///   * ```zero``` - Numeric digit 0
            ///   * ```one``` - Numeric digit 1
            ///   * ```two``` - Numeric digit 2
            ///   * ```three``` - Numeric digit 3
            ///   * ```four``` - Numeric digit 4
            ///   * ```five``` - Numeric digit 5
            ///   * ```six``` - Numeric digit 6
            ///   * ```seven``` - Numeric digit 7
            ///   * ```eight``` - Numeric digit 8
            ///   * ```nine``` - Numeric digit 9
            ///   * ```\\D``` - Any character other than a decimal digit
            /// <example>["one", "nine"]</example>
            /// </summary>
            [DataMember(Name = "keys")]
            [DataTypes(Pattern = @"^(zero|one|two|three|four|five|six|seven|eight|nine|\\D)$")]
            public List<string> Keys { get; init; }

            /// <summary>
            /// Supporting command keys on the Text Terminal Unit.
            /// This property can be null if no command keys are supported.
            /// </summary>
            [DataMember(Name = "commandKeys")]
            public Dictionary<string, KeyClass> CommandKeys { get; init; }

        }
    }
}

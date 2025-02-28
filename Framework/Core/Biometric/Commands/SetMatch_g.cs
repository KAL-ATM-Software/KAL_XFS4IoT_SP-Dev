/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * SetMatch_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Biometric.Commands
{
    //Original name = SetMatch
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Biometric.SetMatch")]
    public sealed class SetMatchCommand : Command<SetMatchCommand.PayloadData>
    {
        public SetMatchCommand(int RequestId, SetMatchCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompareModeEnum? CompareMode = null, string Identifier = null, int? Maximum = null, int? Threshold = null)
                : base()
            {
                this.CompareMode = CompareMode;
                this.Identifier = Identifier;
                this.Maximum = Maximum;
                this.Threshold = Threshold;
            }

            public enum CompareModeEnum
            {
                Verify,
                Identity
            }

            /// <summary>
            /// Specifies the type of match operation that is being done. The following values are possible:
            /// 
            /// * ```verify``` - The biometric data will be compared as a one-to-one verification operation.
            /// * ```identity``` - The biometric data will be compared as a one-to-many identification operation.
            /// </summary>
            [DataMember(Name = "compareMode")]
            public CompareModeEnum? CompareMode { get; init; }

            /// <summary>
            /// In the case where *compareMode* is verify this parameter corresponds to a template that has been imported by a previous call to
            /// the [Biometric.Import](#biometric.import). If *compareMode* is identify a comparison is performed against all of the
            /// imported templates, in which case this property can be null.
            /// This property corresponds to the list of template identifiers returned by the
            /// [Biometric.GetStorageInfo](#biometric.getstorageinfo) command.
            /// <example>id1</example>
            /// </summary>
            [DataMember(Name = "identifier")]
            [DataTypes(Pattern = @"^id[0-9A-Za-z]+$")]
            public string Identifier { get; init; }

            /// <summary>
            /// Specifies the maximum number of matches to return. In the case where *compareMode* is verify this property can be null.
            /// </summary>
            [DataMember(Name = "maximum")]
            [DataTypes(Minimum = 0)]
            public int? Maximum { get; init; }

            /// <summary>
            /// Specifies the minimum matching confidence level necessary for the candidate to be included in the results. This value should be
            /// in the range of 0 to 100, where 100 represents an exact match and 0 represents no match.
            /// <example>80</example>
            /// </summary>
            [DataMember(Name = "threshold")]
            [DataTypes(Minimum = 0, Maximum = 100)]
            public int? Threshold { get; init; }

        }
    }
}

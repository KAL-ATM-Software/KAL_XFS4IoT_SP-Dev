/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * Match_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Biometric.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Biometric.Match")]
    public sealed class MatchCompletion : Completion<MatchCompletion.PayloadData>
    {
        public MatchCompletion(int RequestId, MatchCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, Dictionary<string, CandidatesClass> Candidates = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.Candidates = Candidates;
            }

            public enum ErrorCodeEnum
            {
                NoImportedData,
                ModeNotSupported,
                NoCaptureData,
                InvalidCompareMode,
                InvalidThreshold
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```noImportedData``` - The command failed because no data was imported previously using the [Biometric.Import](#biometric.import).
            /// * ```invalidIdentifier``` - The command failed because data was imported but *identifier* was not found.
            /// * ```modeNotSupported``` -\tThe type of match specified in *compareMode* is not supported.
            /// * ```noCaptureData``` -\tNo captured data is present. Typically means that the [Biometric.Read](#biometric.read)
            ///                         command has not been called, or the captured data has been cleared using the [Biometric.Clear](#biometric.clear).
            /// * ```invalidCompareMode``` - The compare mode specified by the *compareMode* input parameter is not supported.
            /// * ```invalidThreshold``` - The *Threshold* input parameter is greater than the maximum allowed of 100.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            [DataContract]
            public sealed class CandidatesClass
            {
                public CandidatesClass(int? ConfidenceLevel = null, List<byte> TemplateData = null)
                {
                    this.ConfidenceLevel = ConfidenceLevel;
                    this.TemplateData = TemplateData;
                }

                /// <summary>
                /// Specifies the level of confidence for the match found.
                /// This value is in a scale of 0 - 100, where 0 is no match and 100 is an exact match.
                /// The minimum value will be that which was set by the *threshold* property.
                /// </summary>
                [DataMember(Name = "confidenceLevel")]
                [DataTypes(Minimum = 0, Maximum = 100)]
                public int? ConfidenceLevel { get; init; }

                /// <summary>
                /// Contains the biometric template data that was matched.
                /// This data may be used as justification for the biometric data match or confidence level.
                /// This property is null if no additional comparison data is returned.
                /// <example>dGVtcGxhdGUgZGF0YQ==</example>
                /// </summary>
                [DataMember(Name = "templateData")]
                [DataTypes(Pattern = @"^[A-Za-z0-9+/]+={0,2}?$")]
                public List<byte> TemplateData { get; init; }

            }

            /// <summary>
            /// The object name has a unique number that positively identifies the biometric template data.
            /// This corresponds to the list of template identifiers returned by
            /// the [Biometric.GetStorageInfo](#biometric.getstorageinfo) command.
            /// This property can be null if the [Biometric.Match](#biometric.match) operation completes with no match found.
            /// If there are matches found, this property contains all of the matching templates in order of confidence level, with the highest score first.
            /// Note that where the number of templates that match the input criteria of the threshold are greater than *maximum*, only the *maximum* templates
            /// with the highest scores will be returned.
            /// </summary>
            [DataMember(Name = "candidates")]
            public Dictionary<string, CandidatesClass> Candidates { get; init; }

        }
    }
}

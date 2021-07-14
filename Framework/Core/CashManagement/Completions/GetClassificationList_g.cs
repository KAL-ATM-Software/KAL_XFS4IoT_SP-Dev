/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetClassificationList_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.GetClassificationList")]
    public sealed class GetClassificationListCompletion : Completion<GetClassificationListCompletion.PayloadData>
    {
        public GetClassificationListCompletion(int RequestId, GetClassificationListCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, string Version = null, List<ClassificationElementsClass> ClassificationElements = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.Version = Version;
                this.ClassificationElements = ClassificationElements;
            }

            /// <summary>
            /// This is an application defined string that sets the version identifier of 
            /// the classification list. This property can be omitted if it has no version identifier.
            /// </summary>
            [DataMember(Name = "version")]
            public string Version { get; init; }

            [DataContract]
            public sealed class ClassificationElementsClass
            {
                public ClassificationElementsClass(string SerialNumber = null, string CurrencyID = null, double? Value = null, LevelEnum? Level = null)
                {
                    this.SerialNumber = SerialNumber;
                    this.CurrencyID = CurrencyID;
                    this.Value = Value;
                    this.Level = Level;
                }

                /// <summary>
                /// This string defines the serial number or a mask of serial numbers of one element with the 
                /// defined currency and value. For a definition of the mask see Section Note Classification.
                /// </summary>
                [DataMember(Name = "serialNumber")]
                public string SerialNumber { get; init; }

                /// <summary>
                /// The three character ISO 4217 format currency identifier [Ref. 2] of the element.
                /// </summary>
                [DataMember(Name = "currencyID")]
                public string CurrencyID { get; init; }

                /// <summary>
                /// The value of the element. This field can be zero to represent all values.
                /// </summary>
                [DataMember(Name = "value")]
                public double? Value { get; init; }

                public enum LevelEnum
                {
                    Level1,
                    Level2,
                    Level3,
                    Level4Fit,
                    Level4Unfit
                }

                /// <summary>
                /// Specifies the note level. Following values are possible:
                /// 
                /// * ```level1``` - The element specifies notes to be treated as level 1 notes.
                /// * ```level2``` - The element specifies notes to be treated as level 2 notes.
                /// * ```level3``` - The element specifies notes to be treated as level 3 notes.
                /// * ```level4Fit``` - The element specifies notes to be treated as fit level 4 notes.
                /// * ```level4Unfit``` - The element specifies notes to be treated as unfit level 4 notes.
                /// </summary>
                [DataMember(Name = "level")]
                public LevelEnum? Level { get; init; }

            }

            /// <summary>
            /// Array of classification objects.
            /// </summary>
            [DataMember(Name = "classificationElements")]
            public List<ClassificationElementsClass> ClassificationElements { get; init; }

        }
    }
}

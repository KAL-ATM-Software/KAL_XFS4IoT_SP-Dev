/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetCashUnitCountStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetCashUnitCountStatus")]
    public sealed class GetCashUnitCountStatusCompletion : Completion<GetCashUnitCountStatusCompletion.PayloadData>
    {
        public GetCashUnitCountStatusCompletion(int RequestId, GetCashUnitCountStatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, Dictionary<string, CashUnitCountStatusClass> CashUnitCountStatus = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.CashUnitCountStatus = CashUnitCountStatus;
            }

            [DataContract]
            public sealed class CashUnitCountStatusClass
            {
                public CashUnitCountStatusClass(string PhysicalPositionName = null, AccuracyEnum? Accuracy = null)
                {
                    this.PhysicalPositionName = PhysicalPositionName;
                    this.Accuracy = Accuracy;
                }

                /// <summary>
                /// A name identifying the physical location of the cash unit within the CashAcceptor. This field can be 
                /// used to identify shared cash units/media bins.
                /// </summary>
                [DataMember(Name = "physicalPositionName")]
                public string PhysicalPositionName { get; init; }

                public enum AccuracyEnum
                {
                    NotSupported,
                    Accurate,
                    AccurateSet,
                    Inaccurate,
                    Unknown
                }

                /// <summary>
                /// Describes the accuracy of *count*. Following values are possible:
                /// 
                /// "notSupported": The hardware is not capable to determine the accuracy of *count*.
                /// 
                /// "accurate": The *count* is expected to be accurate. The notes were previously counted or replenished 
                /// and there have since been no events that might have introduced inaccuracy. 
                /// This value will be reported as a result of the following commands: Replenish and CashUnitCount.
                /// 
                /// "accurateSet": The *count* is expected to be accurate.  The notes were previously set and there have 
                /// since been no events that might have introduced inaccuracy.
                /// 
                /// "inaccurate": The *count* is likely to be inaccurate.  A jam, picking fault, or some other event may 
                /// have resulted in a counting inaccuracy.
                /// 
                /// "unknown": The accuracy of *count* cannot be determined. This may be due to cash unit insertion or 
                /// some other hardware event.
                /// </summary>
                [DataMember(Name = "accuracy")]
                public AccuracyEnum? Accuracy { get; init; }

            }

            /// <summary>
            /// Object containing cashUnitCountStatus objects. cashUnitCountStatus objects use the same names 
            /// as used in [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo).
            /// </summary>
            [DataMember(Name = "cashUnitCountStatus")]
            public Dictionary<string, CashUnitCountStatusClass> CashUnitCountStatus { get; init; }

        }
    }
}

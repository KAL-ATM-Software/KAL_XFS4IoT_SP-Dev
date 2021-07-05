/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * GetTellerInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.GetTellerInfo")]
    public sealed class GetTellerInfoCompletion : Completion<GetTellerInfoCompletion.PayloadData>
    {
        public GetTellerInfoCompletion(int RequestId, GetTellerInfoCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, List<TellerDetailsClass> TellerDetails = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.TellerDetails = TellerDetails;
            }

            public enum ErrorCodeEnum
            {
                InvalidCurrency,
                InvalidTellerId
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```invalidCurrency``` - Specified currency not currently available.
            /// * ```invalidTellerId``` - Invalid teller ID.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

            [DataContract]
            public sealed class TellerDetailsClass
            {
                public TellerDetailsClass(int? TellerID = null, InputPositionEnum? InputPosition = null, OutputPositionEnum? OutputPosition = null, Dictionary<string, TellerTotalsClass> TellerTotals = null)
                {
                    this.TellerID = TellerID;
                    this.InputPosition = InputPosition;
                    this.OutputPosition = OutputPosition;
                    this.TellerTotals = TellerTotals;
                }

                /// <summary>
                /// Identification of the teller.
                /// </summary>
                [DataMember(Name = "tellerID")]
                public int? TellerID { get; private set; }

                public enum InputPositionEnum
                {
                    None,
                    Left,
                    Right,
                    Center,
                    Top,
                    Bottom,
                    Front,
                    Rear
                }

                /// <summary>
                /// The input position assigned to the teller for cash entry. Following values are possible:
                /// 
                /// * ```none``` - No position is assigned to the teller.
                /// * ```left``` - Left position is assigned to the teller.
                /// * ```right``` - Right position is assigned to the teller.
                /// * ```center``` - Center position is assigned to the teller.
                /// * ```top``` - Top position is assigned to the teller.
                /// * ```bottom``` - Bottom position is assigned to the teller.
                /// * ```front``` - Front position is assigned to the teller.
                /// * ```rear``` - Rear position is assigned to the teller.
                /// </summary>
                [DataMember(Name = "inputPosition")]
                public InputPositionEnum? InputPosition { get; private set; }

                public enum OutputPositionEnum
                {
                    None,
                    Left,
                    Right,
                    Center,
                    Top,
                    Bottom,
                    Front,
                    Rear
                }

                /// <summary>
                /// The output position from which cash is presented to the teller. Following values are possible:
                /// 
                /// * ```none``` - No position is assigned to the teller.
                /// * ```left``` - Left position is assigned to the teller.
                /// * ```right``` - Right position is assigned to the teller.
                /// * ```center``` - Center position is assigned to the teller.
                /// * ```top``` - Top position is assigned to the teller.
                /// * ```bottom``` - Bottom position is assigned to the teller.
                /// * ```front``` - Front position is assigned to the teller.
                /// * ```rear``` - Rear position is assigned to the teller.
                /// </summary>
                [DataMember(Name = "outputPosition")]
                public OutputPositionEnum? OutputPosition { get; private set; }

                [DataContract]
                public sealed class TellerTotalsClass
                {
                    public TellerTotalsClass(double? ItemsReceived = null, double? ItemsDispensed = null, double? CoinsReceived = null, double? CoinsDispensed = null, double? CashBoxReceived = null, double? CashBoxDispensed = null)
                    {
                        this.ItemsReceived = ItemsReceived;
                        this.ItemsDispensed = ItemsDispensed;
                        this.CoinsReceived = CoinsReceived;
                        this.CoinsDispensed = CoinsDispensed;
                        this.CashBoxReceived = CashBoxReceived;
                        this.CashBoxDispensed = CashBoxDispensed;
                    }

                    /// <summary>
                    /// The total amount of items (other than coins) of the specified currency accepted.
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "itemsReceived")]
                    public double? ItemsReceived { get; private set; }

                    /// <summary>
                    /// The total amount of items (other than coins) of the specified currency dispensed. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "itemsDispensed")]
                    public double? ItemsDispensed { get; private set; }

                    /// <summary>
                    /// The total amount of coin currency accepted. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "coinsReceived")]
                    public double? CoinsReceived { get; private set; }

                    /// <summary>
                    /// The total amount of coin currency dispensed. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "coinsDispensed")]
                    public double? CoinsDispensed { get; private set; }

                    /// <summary>
                    /// The total amount of cash box currency accepted. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "cashBoxReceived")]
                    public double? CashBoxReceived { get; private set; }

                    /// <summary>
                    /// The total amount of cash box currency dispensed. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "cashBoxDispensed")]
                    public double? CashBoxDispensed { get; private set; }

                }

                /// <summary>
                /// List of teller total objects. There is one object per currency.
                /// </summary>
                [DataMember(Name = "tellerTotals")]
                public Dictionary<string, TellerTotalsClass> TellerTotals { get; private set; }

            }

            /// <summary>
            /// Array of teller detail objects.
            /// </summary>
            [DataMember(Name = "tellerDetails")]
            public List<TellerDetailsClass> TellerDetails { get; private set; }

        }
    }
}

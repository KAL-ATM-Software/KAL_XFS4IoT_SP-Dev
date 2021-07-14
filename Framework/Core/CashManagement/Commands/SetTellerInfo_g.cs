/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * SetTellerInfo_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.CashManagement.Commands
{
    //Original name = SetTellerInfo
    [DataContract]
    [Command(Name = "CashManagement.SetTellerInfo")]
    public sealed class SetTellerInfoCommand : Command<SetTellerInfoCommand.PayloadData>
    {
        public SetTellerInfoCommand(int RequestId, SetTellerInfoCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, ActionEnum? Action = null, TellerDetailsClass TellerDetails = null)
                : base(Timeout)
            {
                this.Action = Action;
                this.TellerDetails = TellerDetails;
            }

            public enum ActionEnum
            {
                CreateTeller,
                ModifyTeller,
                DeleteTeller
            }

            /// <summary>
            /// The action to be performed. Following values are possible:
            /// 
            /// * ```createTeller``` - A teller is to be added.
            /// * ```modifyTeller``` - Information about an existing teller is to be modified.
            /// * ```deleteTeller``` - A teller is to be removed.
            /// </summary>
            [DataMember(Name = "action")]
            public ActionEnum? Action { get; init; }

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
                public int? TellerID { get; init; }

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
                public InputPositionEnum? InputPosition { get; init; }

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
                public OutputPositionEnum? OutputPosition { get; init; }

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
                    public double? ItemsReceived { get; init; }

                    /// <summary>
                    /// The total amount of items (other than coins) of the specified currency dispensed. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "itemsDispensed")]
                    public double? ItemsDispensed { get; init; }

                    /// <summary>
                    /// The total amount of coin currency accepted. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "coinsReceived")]
                    public double? CoinsReceived { get; init; }

                    /// <summary>
                    /// The total amount of coin currency dispensed. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "coinsDispensed")]
                    public double? CoinsDispensed { get; init; }

                    /// <summary>
                    /// The total amount of cash box currency accepted. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "cashBoxReceived")]
                    public double? CashBoxReceived { get; init; }

                    /// <summary>
                    /// The total amount of cash box currency dispensed. 
                    /// The amount is expressed as floating point value.
                    /// </summary>
                    [DataMember(Name = "cashBoxDispensed")]
                    public double? CashBoxDispensed { get; init; }

                }

                /// <summary>
                /// List of teller total objects. There is one object per currency.
                /// </summary>
                [DataMember(Name = "tellerTotals")]
                public Dictionary<string, TellerTotalsClass> TellerTotals { get; init; }

            }

            /// <summary>
            /// Teller details object.
            /// </summary>
            [DataMember(Name = "tellerDetails")]
            public TellerDetailsClass TellerDetails { get; init; }

        }
    }
}

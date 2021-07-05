/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CalibrateCashUnit_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashManagement.Completions
{
    [DataContract]
    [Completion(Name = "CashManagement.CalibrateCashUnit")]
    public sealed class CalibrateCashUnitCompletion : Completion<CalibrateCashUnitCompletion.PayloadData>
    {
        public CalibrateCashUnitCompletion(int RequestId, CalibrateCashUnitCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, string Cashunit = null, int? NumOfBills = null, PositionClass Position = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.Cashunit = Cashunit;
                this.NumOfBills = NumOfBills;
                this.Position = Position;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                UnsupportedPosition,
                ExchangeActive,
                InvalidCashUnit
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - A cash unit caused an error. A CashManagement.CashUnitErrorEvent will be sent with the details.
            /// * ```unsupportedPosition``` - The position specified is not valid.
            /// * ```exchangeActive``` - The device is in an exchange state.
            /// * ```invalidCashUnit``` - The cash unit number specified is not valid.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

            /// <summary>
            /// The object name of the cash unit which has been calibrated as stated by the 
            /// [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) command.
            /// </summary>
            [DataMember(Name = "cashunit")]
            public string Cashunit { get; private set; }

            /// <summary>
            /// Number of items that were actually dispensed during the calibration process. This value may be different from that 
            /// passed in using the input structure if the cash dispenser always dispenses a default number of bills. 
            /// When bills are presented to an output position this is the count of notes presented to the output position, 
            /// any other notes rejected during the calibration process are not included in this count as they will be accounted for within the cash unit counts.
            /// </summary>
            [DataMember(Name = "numOfBills")]
            public int? NumOfBills { get; private set; }

            [DataContract]
            public sealed class PositionClass
            {
                public PositionClass(string Cashunit = null, RetractAreaClass RetractArea = null, OutputPositionEnum? OutputPosition = null)
                {
                    this.Cashunit = Cashunit;
                    this.RetractArea = RetractArea;
                    this.OutputPosition = OutputPosition;
                }

                /// <summary>
                /// If defined, this value specifies the object name (as stated by the 
                /// [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) command) of the single cash unit to 
                /// be used for the storage of any items found.
                /// </summary>
                [DataMember(Name = "cashunit")]
                public string Cashunit { get; private set; }

                [DataContract]
                public sealed class RetractAreaClass
                {
                    public RetractAreaClass(OutputPositionEnum? OutputPosition = null, RetractAreaEnum? RetractArea = null, int? Index = null)
                    {
                        this.OutputPosition = OutputPosition;
                        this.RetractArea = RetractArea;
                        this.Index = Index;
                    }

                    public enum OutputPositionEnum
                    {
                        Default,
                        Left,
                        Right,
                        Center,
                        Top,
                        Bottom,
                        Front,
                        Rear
                    }

                    /// <summary>
                    /// Output position from which to retract the items. Following values are possible:
                    /// 
                    /// * ```default``` - The default configuration information should be used.
                    /// * ```left``` - Retract items from the left output position.
                    /// * ```right``` - Retract items from the right output position.
                    /// * ```center``` - Retract items from the center output position.
                    /// * ```top``` - Retract items from the top output position.
                    /// * ```bottom``` - Retract items from the bottom output position.
                    /// * ```front``` - Retract items from the front output position.
                    /// * ```rear``` - Retract items from the rear output position.
                    /// </summary>
                    [DataMember(Name = "outputPosition")]
                    public OutputPositionEnum? OutputPosition { get; private set; }

                    public enum RetractAreaEnum
                    {
                        Retract,
                        Transport,
                        Stacker,
                        Reject,
                        ItemCassette
                    }

                    /// <summary>
                    /// This value specifies the area to which the items are to be retracted. Following values are possible:
                    /// 
                    /// * ```retract``` - Retract the items to a retract cash unit.
                    /// * ```transport``` - Retract the items to the transport.
                    /// * ```stacker``` - Retract the items to the intermediate stacker area.
                    /// * ```reject``` - Retract the items to a reject cash unit.
                    /// * ```itemCassette``` - Retract the items to the item cassettes, i.e. cassettes that can be dispensed from.
                    /// </summary>
                    [DataMember(Name = "retractArea")]
                    public RetractAreaEnum? RetractArea { get; private set; }

                    /// <summary>
                    /// If *retractArea* is set to \"retract\" this field defines the position inside the retract cash units into 
                    /// which the cash is to be retracted. *index* starts with a value of one (1) for the first retract position 
                    /// and increments by one for each subsequent position. If there are several retract cash units 
                    /// (of type \"retractCassette\" in command CashManagement.CashUnitInfo), *index* would be incremented from the 
                    /// first position of the first retract cash unit to the last position of the last retract cash unit. 
                    /// The maximum value of *index* is the sum of *maximum* of each retract cash unit. If *retractArea* is not 
                    /// set to \"retract\" the value of this field is ignored.
                    /// </summary>
                    [DataMember(Name = "index")]
                    public int? Index { get; private set; }

                }

                /// <summary>
                /// This field is used if items are to be moved to internal areas of the device, including cash units, the intermediate stacker, or the transport.
                /// </summary>
                [DataMember(Name = "retractArea")]
                public RetractAreaClass RetractArea { get; private set; }

                public enum OutputPositionEnum
                {
                    Default,
                    Left,
                    Right,
                    Center,
                    Top,
                    Bottom,
                    Front,
                    Rear
                }

                /// <summary>
                /// The output position to which items are to be moved. This field is only used if *number* is zero and retractArea is omitted.
                /// Following values are possible:
                /// 
                /// * ```default``` - The default configuration.
                /// * ```left``` - The left output position.
                /// * ```right``` - The right output position.
                /// * ```center``` - The center output position.
                /// * ```top``` - The top output position.
                /// * ```bottom``` - The bottom output position.
                /// * ```front``` - The front output position.
                /// * ```rear``` - The rear output position.
                /// </summary>
                [DataMember(Name = "outputPosition")]
                public OutputPositionEnum? OutputPosition { get; private set; }

            }

            /// <summary>
            /// Specifies where the items were moved to during the calibration process.
            /// </summary>
            [DataMember(Name = "position")]
            public PositionClass Position { get; private set; }

        }
    }
}

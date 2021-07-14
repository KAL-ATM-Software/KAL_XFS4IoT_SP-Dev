/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetPositionCapabilities_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetPositionCapabilities")]
    public sealed class GetPositionCapabilitiesCompletion : Completion<GetPositionCapabilitiesCompletion.PayloadData>
    {
        public GetPositionCapabilitiesCompletion(int RequestId, GetPositionCapabilitiesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, List<PosCapabilitiesClass> PosCapabilities = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.PosCapabilities = PosCapabilities;
            }

            [DataContract]
            public sealed class PosCapabilitiesClass
            {
                public PosCapabilitiesClass(PositionEnum? Position = null, UsageClass Usage = null, bool? ShutterControl = null, bool? ItemsTakenSensor = null, bool? ItemsInsertedSensor = null, RetractAreasClass RetractAreas = null, bool? PresentControl = null, bool? PreparePresent = null)
                {
                    this.Position = Position;
                    this.Usage = Usage;
                    this.ShutterControl = ShutterControl;
                    this.ItemsTakenSensor = ItemsTakenSensor;
                    this.ItemsInsertedSensor = ItemsInsertedSensor;
                    this.RetractAreas = RetractAreas;
                    this.PresentControl = PresentControl;
                    this.PreparePresent = PreparePresent;
                }

                public enum PositionEnum
                {
                    InLeft,
                    InRight,
                    InCenter,
                    InTop,
                    InBottom,
                    InFront,
                    InRear,
                    OutLeft,
                    OutRight,
                    OutCenter,
                    OutTop,
                    OutBottom,
                    OutFront,
                    OutRear
                }

                /// <summary>
                /// Specifies one of the input or output positions as one of the following values:
                /// 
                /// "inLeft": Left input position.
                /// 
                /// "inRight": Right input position.
                /// 
                /// "inCenter": Center input position.
                /// 
                /// "inTop": Top input position.
                /// 
                /// "inBottom": Bottom input position.
                /// 
                /// "inFront": Front input position.
                /// 
                /// "inRear": Rear input position.
                /// 
                /// "outLeft": Left output position.
                /// 
                /// "outRight": Right output position.
                /// 
                /// "outCenter": Center output position.
                /// 
                /// "outTop": Top output position.
                /// 
                /// "outBottom": Bottom output position.
                /// 
                /// "outFront": Front output position.
                /// 
                /// "outRear": Rear output position.
                /// </summary>
                [DataMember(Name = "position")]
                public PositionEnum? Position { get; init; }

                [DataContract]
                public sealed class UsageClass
                {
                    public UsageClass(bool? In = null, bool? Refuse = null, bool? Rollback = null)
                    {
                        this.In = In;
                        this.Refuse = Refuse;
                        this.Rollback = Rollback;
                    }

                    /// <summary>
                    /// It is an input position.
                    /// </summary>
                    [DataMember(Name = "in")]
                    public bool? In { get; init; }

                    /// <summary>
                    /// It is an output position used to refuse items.
                    /// </summary>
                    [DataMember(Name = "refuse")]
                    public bool? Refuse { get; init; }

                    /// <summary>
                    /// It is an output position used to rollback items.
                    /// </summary>
                    [DataMember(Name = "rollback")]
                    public bool? Rollback { get; init; }

                }

                /// <summary>
                /// Indicates if an output position is used to reject or rollback.
                /// </summary>
                [DataMember(Name = "usage")]
                public UsageClass Usage { get; init; }

                /// <summary>
                /// If set to TRUE the shutter is controlled implicitly by the Service. If set to FALSE the shutter must be controlled explicitly by the application using the OpenShutter and the CloseShutter commands. In either case the CashAcceptor.PresentMedia command may be used if the presentControl field is reported as FALSE. The shutterControl field is always set to TRUE if the described position has no shutter.
                /// </summary>
                [DataMember(Name = "shutterControl")]
                public bool? ShutterControl { get; init; }

                /// <summary>
                /// Specifies whether or not the described position can detect when items at the exit position are taken by the user. If set to TRUE the Service generates an accompanying CashAcceptor.ItemsTaken event. If set to FALSE this event is not generated. This field relates to output and refused positions.
                /// </summary>
                [DataMember(Name = "itemsTakenSensor")]
                public bool? ItemsTakenSensor { get; init; }

                /// <summary>
                /// Specifies whether the described position has the ability to detect when items have been inserted by the user. If set to TRUE the Service Provider generates an accompanying CashAcceptor.ItemsInserted event. If set to FALSE this event is not generated. This field relates to all input positions.
                /// </summary>
                [DataMember(Name = "itemsInsertedSensor")]
                public bool? ItemsInsertedSensor { get; init; }

                [DataContract]
                public sealed class RetractAreasClass
                {
                    public RetractAreasClass(bool? Retract = null, bool? Reject = null, bool? Transport = null, bool? Stacker = null, bool? BillCassettes = null, bool? CashIn = null)
                    {
                        this.Retract = Retract;
                        this.Reject = Reject;
                        this.Transport = Transport;
                        this.Stacker = Stacker;
                        this.BillCassettes = BillCassettes;
                        this.CashIn = CashIn;
                    }

                    /// <summary>
                    /// Items may be retracted to a retract cash unit.
                    /// </summary>
                    [DataMember(Name = "retract")]
                    public bool? Retract { get; init; }

                    /// <summary>
                    /// Items may be retracted to a reject cash unit.
                    /// </summary>
                    [DataMember(Name = "reject")]
                    public bool? Reject { get; init; }

                    /// <summary>
                    /// Items may be retracted to the transport.
                    /// </summary>
                    [DataMember(Name = "transport")]
                    public bool? Transport { get; init; }

                    /// <summary>
                    /// Items may be retracted to the intermediate stacker.
                    /// </summary>
                    [DataMember(Name = "stacker")]
                    public bool? Stacker { get; init; }

                    /// <summary>
                    /// Items may be retracted to item cassettes, i.e. cash-in and recycle cash units.
                    /// </summary>
                    [DataMember(Name = "billCassettes")]
                    public bool? BillCassettes { get; init; }

                    /// <summary>
                    /// Items may be retracted to a cash-in cash unit.
                    /// </summary>
                    [DataMember(Name = "cashIn")]
                    public bool? CashIn { get; init; }

                }

                /// <summary>
                /// Specifies the areas to which items may be retracted from this position. If the device does not have a retract capability all values will be FALSE.
                /// </summary>
                [DataMember(Name = "retractAreas")]
                public RetractAreasClass RetractAreas { get; init; }

                /// <summary>
                /// Specifies how the presenting of media items is controlled. If presentControl is TRUE then the CashAcceptor.PresentMedia command is not supported and items are moved to the output position for removal as part of the relevant command, e.g. CashAcceptor.CashIn or CashAcceptor.CashInRollback where there is implicit shutter control. If presentControl is FALSE then items returned or rejected can be moved to the output position using the CashAcceptor.PresentMedia command, this includes items returned or rejected as part of a CashAcceptor.CashIn or CashAcceptor.CashInRollback operation. The CashAcceptor.PresentMedia command will open and close the shutter implicitly.
                /// </summary>
                [DataMember(Name = "presentControl")]
                public bool? PresentControl { get; init; }

                /// <summary>
                /// Specifies how the presenting of items is controlled. If preparePresent is FALSE then items to be removed are moved to the output position as part of the relevant command e.g. CashAcceptor.OpenShutter or CashAcceptor.PresentMedia or CashAcceptor.CashInRollback. If preparePresent is TRUE then items are moved to the output position using the CashAcceptor.PreparePresent command.
                /// </summary>
                [DataMember(Name = "preparePresent")]
                public bool? PreparePresent { get; init; }

            }

            /// <summary>
            /// Array of position capabilities for all positions configured in this service.
            /// </summary>
            [DataMember(Name = "posCapabilities")]
            public List<PosCapabilitiesClass> PosCapabilities { get; init; }

        }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * GetCashUnitCapabilities_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.GetCashUnitCapabilities")]
    public sealed class GetCashUnitCapabilitiesCompletion : Completion<GetCashUnitCapabilitiesCompletion.PayloadData>
    {
        public GetCashUnitCapabilitiesCompletion(int RequestId, GetCashUnitCapabilitiesCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, Dictionary<string, CashUnitCapsClass> CashUnitCaps = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.CashUnitCaps = CashUnitCaps;
            }

            [DataContract]
            public sealed class CashUnitCapsClass
            {
                public CashUnitCapsClass(string PhysicalPositionName = null, int? MaximumCapacity = null, bool? HardwareSensors = null, bool? RetractNoteCountThresholds = null, PossibleItemTypesClass PossibleItemTypes = null)
                {
                    this.PhysicalPositionName = PhysicalPositionName;
                    this.MaximumCapacity = MaximumCapacity;
                    this.HardwareSensors = HardwareSensors;
                    this.RetractNoteCountThresholds = RetractNoteCountThresholds;
                    this.PossibleItemTypes = PossibleItemTypes;
                }

                /// <summary>
                /// A name identifying the physical location of the cash unit.
                /// </summary>
                [DataMember(Name = "physicalPositionName")]
                public string PhysicalPositionName { get; private set; }

                /// <summary>
                /// The maximum number of items the cash unit can hold.
                /// No threshold event CashManagement.CashUnitThresholdEvent will be generated when this value is reached. 
                /// This value is persistent.
                /// </summary>
                [DataMember(Name = "maximumCapacity")]
                public int? MaximumCapacity { get; private set; }

                /// <summary>
                /// Specifies whether or not threshold events can be generated based on hardware sensors in the device. 
                /// If this value is TRUE then threshold 
                /// events may be generated based on hardware sensors as opposed to counts.
                /// </summary>
                [DataMember(Name = "hardwareSensors")]
                public bool? HardwareSensors { get; private set; }

                /// <summary>
                /// This field is only valid for cash units of type \"retractCassette\". It specifies whether the CashAcceptor 
                /// retract cassette capacity is based on the number of notes, and therefore whether threshold events are 
                /// generated based on note counts or the number of retract operations. If this value is set to TRUE, threshold 
                /// events for retract cassettes are generated based on the number of notes, when *cashInCount* reaches the 
                /// *maximum* value. If this value is set to FALSE, threshold events for retract cassettes are generated based 
                /// on the number of retract operations, when *count* reaches the *maximum* value.
                /// </summary>
                [DataMember(Name = "retractNoteCountThresholds")]
                public bool? RetractNoteCountThresholds { get; private set; }

                [DataContract]
                public sealed class PossibleItemTypesClass
                {
                    public PossibleItemTypesClass(bool? All = null, bool? Unfit = null, bool? Individual = null, bool? Level1 = null, bool? Level2 = null, bool? Level3 = null, bool? ItemProcessor = null, bool? UnfitIndividual = null)
                    {
                        this.All = All;
                        this.Unfit = Unfit;
                        this.Individual = Individual;
                        this.Level1 = Level1;
                        this.Level2 = Level2;
                        this.Level3 = Level3;
                        this.ItemProcessor = ItemProcessor;
                        this.UnfitIndividual = UnfitIndividual;
                    }

                    /// <summary>
                    /// The cash unit takes all fit banknote types. These are level 4 notes which are fit for recycling.
                    /// </summary>
                    [DataMember(Name = "all")]
                    public bool? All { get; private set; }

                    /// <summary>
                    /// The cash unit takes all unfit banknotes. These are level 4 notes which are unfit for recycling.
                    /// </summary>
                    [DataMember(Name = "unfit")]
                    public bool? Unfit { get; private set; }

                    /// <summary>
                    /// The cash unit takes all types of fit banknotes specified in an individual list. These are level 4 notes which are fit for recycling.
                    /// </summary>
                    [DataMember(Name = "individual")]
                    public bool? Individual { get; private set; }

                    /// <summary>
                    /// Level 1 note types are stored in this cash unit.
                    /// </summary>
                    [DataMember(Name = "level1")]
                    public bool? Level1 { get; private set; }

                    /// <summary>
                    /// If notes can be classified as level 2, then level 2 note types are stored in this cash unit.
                    /// </summary>
                    [DataMember(Name = "level2")]
                    public bool? Level2 { get; private set; }

                    /// <summary>
                    /// If notes can be classified as level 3, then level 3 note types are stored in this cash unit.
                    /// </summary>
                    [DataMember(Name = "level3")]
                    public bool? Level3 { get; private set; }

                    /// <summary>
                    /// The cash unit can accept items on the ItemProcessor interface.
                    /// </summary>
                    [DataMember(Name = "itemProcessor")]
                    public bool? ItemProcessor { get; private set; }

                    /// <summary>
                    /// The cash unit takes all types of unfit banknotes specified in an individual list. These are level 4 notes which are unfit for recycling.
                    /// </summary>
                    [DataMember(Name = "unfitIndividual")]
                    public bool? UnfitIndividual { get; private set; }

                }

                /// <summary>
                /// Specifies the type of items the cash unit can be configured to accept as a combination of flags. The flags 
                /// are defined as the same values listed in the *itemType* field of the CashManagement.CashUnitInfo output 
                /// structure. The CashManagement.CashUnitInfo command describes the item types currently configured for a cash 
                /// unit. This field provides the possible item types values that can be configured for a cash unit using the 
                /// CashManagement.SetCashUnitInfo command.
                /// </summary>
                [DataMember(Name = "possibleItemTypes")]
                public PossibleItemTypesClass PossibleItemTypes { get; private set; }

            }

            /// <summary>
            /// Object containing additional cash unit capapabilities. Cash Unit capabiity objects use the same names 
            /// as used in [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo).
            /// </summary>
            [DataMember(Name = "cashUnitCaps")]
            public Dictionary<string, CashUnitCapsClass> CashUnitCaps { get; private set; }

        }
    }
}

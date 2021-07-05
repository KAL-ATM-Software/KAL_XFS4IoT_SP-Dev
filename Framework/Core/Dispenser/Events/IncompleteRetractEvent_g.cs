/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * IncompleteRetractEvent_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Events;

namespace XFS4IoT.Dispenser.Events
{

    [DataContract]
    [Event(Name = "Dispenser.IncompleteRetractEvent")]
    public sealed class IncompleteRetractEvent : Event<IncompleteRetractEvent.PayloadData>
    {

        public IncompleteRetractEvent(int RequestId, PayloadData Payload)
            : base(RequestId, Payload)
        { }


        [DataContract]
        public sealed class PayloadData : MessagePayloadBase
        {

            public PayloadData(ItemNumberListClass ItemNumberList = null, ReasonEnum? Reason = null)
                : base()
            {
                this.ItemNumberList = ItemNumberList;
                this.Reason = Reason;
            }

            [DataContract]
            public sealed class ItemNumberListClass
            {
                public ItemNumberListClass(List<ItemNumberClass> ItemNumber = null)
                {
                    this.ItemNumber = ItemNumber;
                }

                [DataContract]
                public sealed class ItemNumberClass
                {
                    public ItemNumberClass(string CurrencyID = null, double? Values = null, int? Release = null, int? Count = null, string Cashunit = null)
                    {
                        this.CurrencyID = CurrencyID;
                        this.Values = Values;
                        this.Release = Release;
                        this.Count = Count;
                        this.Cashunit = Cashunit;
                    }

                    /// <summary>
                    /// A three character array storing the ISO format [Ref. 2] Currency ID; if the currency of the item is not known this is omitted.
                    /// </summary>
                    [DataMember(Name = "currencyID")]
                    public string CurrencyID { get; private set; }

                    /// <summary>
                    /// The value of a single item expressed as floating point value; or a zero value if the value of the item is not known.
                    /// </summary>
                    [DataMember(Name = "values")]
                    public double? Values { get; private set; }

                    /// <summary>
                    /// The release of the item. The higher this number is, the newer the release. Zero means that there is 
                    /// only one release or the release is not known. This value has not been standardized 
                    /// and therefore a release number of the same item will not necessarily have the same value in different systems.
                    /// </summary>
                    [DataMember(Name = "release")]
                    public int? Release { get; private set; }

                    /// <summary>
                    /// The count of items of the same type moved to the same destination during the execution of this command.
                    /// </summary>
                    [DataMember(Name = "count")]
                    public int? Count { get; private set; }

                    /// <summary>
                    /// The object name of the cash unit which received items during the execution of this command as stated by the 
                    /// [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) command.
                    /// This value will be omitted if items were moved to the 
                    /// [retractArea](#dispenser.retract.command.properties.retractarea) ```transport``` or ```stacker```.
                    /// </summary>
                    [DataMember(Name = "cashunit")]
                    public string Cashunit { get; private set; }

                }

                /// <summary>
                /// Array of item number objects.
                /// </summary>
                [DataMember(Name = "itemNumber")]
                public List<ItemNumberClass> ItemNumber { get; private set; }

            }

            /// <summary>
            /// The values in this structure report the amount and number of each denomination that were successfully moved during the command prior to the failure.
            /// </summary>
            [DataMember(Name = "itemNumberList")]
            public ItemNumberListClass ItemNumberList { get; private set; }

            public enum ReasonEnum
            {
                RetractFailure,
                RetractAreaFull,
                ForeignItemsDetected,
                InvalidBunch
            }

            /// <summary>
            /// The reason for not having retracted items. Following values are possible:
            /// 
            /// * ```retractFailure``` - The retract has partially failed for a reason not covered by the other reasons 
            /// listed in this event, for example failing to pick an item to be retracted.
            /// * ```retractAreaFull``` - The specified retract area (see input parameter *retractArea*) has become full 
            /// during the retract operation.
            /// * ```foreignItemsDetected``` - Foreign items have been detected.
            /// * ```invalidBunch``` - An invalid bunch of items has been detected, e.g. it is too large or could not be processed.
            /// </summary>
            [DataMember(Name = "reason")]
            public ReasonEnum? Reason { get; private set; }

        }

    }
}

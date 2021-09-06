/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * Retract_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashDispenser.Completions
{
    [DataContract]
    [Completion(Name = "CashDispenser.Retract")]
    public sealed class RetractCompletion : Completion<RetractCompletion.PayloadData>
    {
        public RetractCompletion(int RequestId, RetractCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, List<ItemNumberClass> ItemNumber = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.ItemNumber = ItemNumber;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                NoItems,
                ExchangeActive,
                ShutterNotClosed,
                ItemsTaken,
                InvalidRetractPosition,
                NotRetractArea,
                UnsupportedPosition,
                PositionNotEmpty,
                IncompleteRetract
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// * ```cashUnitError``` - There is a problem with a cash unit. A CashManagement.CashUnitErrorEvent will be posted with the details.
            /// * ```noItems``` - There were no items to retract.
            /// * ```exchangeActive``` - The device is in an exchange state (see CashManagement.StartExchange).
            /// * ```shutterNotClosed``` - The shutter failed to close.
            /// * ```itemsTaken``` - Items were present at the output position at the start of the operation, but were removed before the operation was complete - some or all of the items were not retracted.
            /// * ```invalidRetractPosition``` - The *index* is not supported.
            /// * ```notRetractArea``` - The retract area specified in *retractArea* is not supported.
            /// * ```unsupportedPosition``` - The retract area specified in *retractArea* is not empty so the retract operation is not possible.
            /// * ```positionNotEmpty``` - The request requires too many items to be dispensed.
            /// * ```incompleteRetract``` - Some or all of the items were not retracted for a reason not covered by other error codes. The detail will be reported with the CashDispenser.IncompleteRetractEvent.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

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
                public string CurrencyID { get; init; }

                /// <summary>
                /// The value of a single item expressed as floating point value; or a zero value if the value of the item is not known.
                /// </summary>
                [DataMember(Name = "values")]
                public double? Values { get; init; }

                /// <summary>
                /// The release of the item. The higher this number is, the newer the release. Zero means that there is 
                /// only one release or the release is not known. This value has not been standardized 
                /// and therefore a release number of the same item will not necessarily have the same value in different systems.
                /// </summary>
                [DataMember(Name = "release")]
                public int? Release { get; init; }

                /// <summary>
                /// The count of items of the same type moved to the same destination during the execution of this command.
                /// </summary>
                [DataMember(Name = "count")]
                public int? Count { get; init; }

                /// <summary>
                /// The object name of the cash unit which received items during the execution of this command as stated by the 
                /// [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) command.
                /// This value will be omitted if items were moved to the 
                /// [retractArea](#CashDispenser.retract.command.properties.retractarea) ```transport``` or ```stacker```.
                /// </summary>
                [DataMember(Name = "cashunit")]
                public string Cashunit { get; init; }

            }

            /// <summary>
            /// Array of item number objects.
            /// </summary>
            [DataMember(Name = "itemNumber")]
            public List<ItemNumberClass> ItemNumber { get; init; }

        }
    }
}

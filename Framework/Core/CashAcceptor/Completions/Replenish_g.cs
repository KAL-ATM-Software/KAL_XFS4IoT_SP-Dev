/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * Replenish_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "CashAcceptor.Replenish")]
    public sealed class ReplenishCompletion : Completion<ReplenishCompletion.PayloadData>
    {
        public ReplenishCompletion(int RequestId, ReplenishCompletion.PayloadData Payload, MessageHeader.CompletionCodeEnum CompletionCode, string ErrorDescription)
            : base(RequestId, Payload, CompletionCode, ErrorDescription)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(ErrorCodeEnum? ErrorCode = null, int? NumberOfItemsRemoved = null, int? NumberOfItemsRejected = null, List<ReplenishTargetResultsClass> ReplenishTargetResults = null)
                : base()
            {
                this.ErrorCode = ErrorCode;
                this.NumberOfItemsRemoved = NumberOfItemsRemoved;
                this.NumberOfItemsRejected = NumberOfItemsRejected;
                this.ReplenishTargetResults = ReplenishTargetResults;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                InvalidCashUnit,
                ExchangeActive,
                CashInActive
            }

            /// <summary>
            /// Specifies the error code if applicable, otherwise null. The following values are possible:
            /// 
            /// * ```cashUnitError``` - A problem occurred with a storage unit. A
            /// [Storage.StorageErrorEvent](#storage.storageerrorevent) will be sent with the details. If appropriate
            /// a [CashAcceptor.IncompleteReplenishEvent](#cashacceptor.incompletereplenishevent) will also be sent.
            /// * ```invalidCashUnit``` - The source or target storage unit specified is invalid for this operation.
            /// The [CashAcceptor.GetReplenishTarget](#cashacceptor.getreplenishtarget) command can be used to determine which source or target is valid.
            /// * ```exchangeActive``` - The device is in the exchange state.
            /// * ```cashInActive``` - A cash-in transaction is active.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// Total number of items removed from the source storage unit including rejected items during execution of this
            /// command. This property is null if no items were removed.
            /// <example>20</example>
            /// </summary>
            [DataMember(Name = "numberOfItemsRemoved")]
            [DataTypes(Minimum = 1)]
            public int? NumberOfItemsRemoved { get; init; }

            /// <summary>
            /// Total number of items rejected during execution of this command. This property is null if no items were rejected.
            /// <example>2</example>
            /// </summary>
            [DataMember(Name = "numberOfItemsRejected")]
            [DataTypes(Minimum = 1)]
            public int? NumberOfItemsRejected { get; init; }

            [DataContract]
            public sealed class ReplenishTargetResultsClass
            {
                public ReplenishTargetResultsClass(string Target = null, string CashItem = null, int? NumberOfItemsReceived = null)
                {
                    this.Target = Target;
                    this.CashItem = CashItem;
                    this.NumberOfItemsReceived = NumberOfItemsReceived;
                }

                /// <summary>
                /// Name of the storage unit (as stated by the [Storage.GetStorage](#storage.getstorage)
                /// command) to which items have been moved.
                /// <example>unit1</example>
                /// </summary>
                [DataMember(Name = "target")]
                [DataTypes(Pattern = @"^unit[0-9A-Za-z]+$")]
                public string Target { get; init; }

                /// <summary>
                /// A cash item as reported by [CashManagement.GetBankNoteTypes](#cashmanagement.getbanknotetypes). This is null
                /// if the item was not identified as a cash item.
                /// <example>type20USD1</example>
                /// </summary>
                [DataMember(Name = "cashItem")]
                [DataTypes(Pattern = @"^type[0-9A-Z]+$")]
                public string CashItem { get; init; }

                /// <summary>
                /// Total number of items received in this target storage unit of the *cashItem* note type.
                /// <example>20</example>
                /// </summary>
                [DataMember(Name = "numberOfItemsReceived")]
                [DataTypes(Minimum = 1)]
                public int? NumberOfItemsReceived { get; init; }

            }

            /// <summary>
            /// Breakdown of which notes were moved and where they moved to. In the case where one note type has several releases and these
            /// are moved, or where items are moved from a multi denomination storage unit to a multi denomination storage unit,
            /// each target can receive several note types.
            /// 
            /// For example:
            /// * If one single target was specified with the *replenishTargets* input structure, and this target received
            /// two different note types, then this property will have two elements.
            /// * If two targets were specified and the first target received two different note types and the second target
            /// received three different note types, then this property will have five elements.
            /// </summary>
            [DataMember(Name = "replenishTargetResults")]
            public List<ReplenishTargetResultsClass> ReplenishTargetResults { get; init; }

        }
    }
}

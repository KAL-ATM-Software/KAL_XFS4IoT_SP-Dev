/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashAcceptor interface.
 * Deplete_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.CashAcceptor.Completions
{
    [DataContract]
    [Completion(Name = "CashAcceptor.Deplete")]
    public sealed class DepleteCompletion : Completion<DepleteCompletion.PayloadData>
    {
        public DepleteCompletion(int RequestId, DepleteCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, ErrorCodeEnum? ErrorCode = null, int? NumberOfItemsReceived = null, int? NumberOfItemsRejected = null, List<DepleteSourceResultsClass> DepleteSourceResults = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.ErrorCode = ErrorCode;
                this.NumberOfItemsReceived = NumberOfItemsReceived;
                this.NumberOfItemsRejected = NumberOfItemsRejected;
                this.DepleteSourceResults = DepleteSourceResults;
            }

            public enum ErrorCodeEnum
            {
                CashUnitError,
                InvalidCashUnit,
                CashInActive,
                ExchangeActive
            }

            /// <summary>
            /// Specifies the error code if applicable. Following values are possible:
            /// 
            /// \"cashUnitError\": A problem occurred with a cash unit. A CashManagement.CashUnitErrorEvent will be sent with the details. 
            /// If appropriate a CashAcceptor.IncompleteDepleteEvent will also be sent.
            /// 
            /// \"invalidCashUnit\": The source or target cash unit specified is invalid for this operation. 
            /// The CashAcceptor.DepleteSource command can be used to determine which source or target is valid.
            /// 
            /// \"cashInActive\": A cash-in transaction is active.
            /// 
            /// \"exchangeActive\": The device is in the exchange state.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; private set; }

            /// <summary>
            /// Total number of items received in the target cash unit during execution of this command.
            /// </summary>
            [DataMember(Name = "numberOfItemsReceived")]
            public int? NumberOfItemsReceived { get; private set; }

            /// <summary>
            /// Total number of items rejected during execution of this command.
            /// </summary>
            [DataMember(Name = "numberOfItemsRejected")]
            public int? NumberOfItemsRejected { get; private set; }

            [DataContract]
            public sealed class DepleteSourceResultsClass
            {
                public DepleteSourceResultsClass(string CashunitSource = null, int? NoteID = null, int? NumberOfItemsRemoved = null)
                {
                    this.CashunitSource = CashunitSource;
                    this.NoteID = NoteID;
                    this.NumberOfItemsRemoved = NumberOfItemsRemoved;
                }

                /// <summary>
                /// Object name of the cash unit (as stated by the [CashManagement.GetCashUnitInfo](#cashmanagement.getcashunitinfo) 
                /// command) from which items have been removed.
                /// </summary>
                [DataMember(Name = "cashunitSource")]
                public string CashunitSource { get; private set; }

                /// <summary>
                /// Identification of item type. The note ID represents the item identifiers reported by the CashAcceptor.BanknoteTypes command.
                /// </summary>
                [DataMember(Name = "noteID")]
                public int? NoteID { get; private set; }

                /// <summary>
                /// Total number of items removed from this source cash unit of the *noteID* item type. 
                /// A zero value will be returned if this source cash unit did not move any items of this item type, for example due to a cash unit or transport jam.
                /// </summary>
                [DataMember(Name = "numberOfItemsRemoved")]
                public int? NumberOfItemsRemoved { get; private set; }

            }

            /// <summary>
            /// Array of DepleteSpourceResult structures. In the case where one item type has several releases and these are moved, 
            /// or where items are moved from a multi denomination cash unit to a multi denomination cash unit, each source can move several *noteID* item types. 
            /// 
            /// For example: If one single source was specified with the *depleteSources* input structure, and this source moved two different *noteID* item types, 
            /// then the *depleteSourceResults* array will have two elements. Or if two sources were specified and the 
            /// first source moved two different *noteID* item types and the second source moved three different *noteID* item types, then the *depleteSourceResults* array will have five elements.
            /// </summary>
            [DataMember(Name = "depleteSourceResults")]
            public List<DepleteSourceResultsClass> DepleteSourceResults { get; private set; }

        }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * GetTransactionStatus_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Completions;

namespace XFS4IoT.Check.Completions
{
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Completion(Name = "Check.GetTransactionStatus")]
    public sealed class GetTransactionStatusCompletion : Completion<GetTransactionStatusCompletion.PayloadData>
    {
        public GetTransactionStatusCompletion(int RequestId, GetTransactionStatusCompletion.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(CompletionCodeEnum CompletionCode, string ErrorDescription, MediaInTransactionEnum? MediaInTransaction = null, string MediaOnStacker = null, string LastMediaInTotal = null, string LastMediaAddedToStacker = null, string TotalItems = null, string TotalItemsRefused = null, string TotalBunchesRefused = null, List<MediaStatusClass> MediaInfo = null)
                : base(CompletionCode, ErrorDescription)
            {
                this.MediaInTransaction = MediaInTransaction;
                this.MediaOnStacker = MediaOnStacker;
                this.LastMediaInTotal = LastMediaInTotal;
                this.LastMediaAddedToStacker = LastMediaAddedToStacker;
                this.TotalItems = TotalItems;
                this.TotalItemsRefused = TotalItemsRefused;
                this.TotalBunchesRefused = TotalBunchesRefused;
                this.MediaInfo = MediaInfo;
            }

            public enum MediaInTransactionEnum
            {
                Ok,
                Active,
                Rollback,
                RollbackAfterDeposit,
                Retract,
                Failure,
                Unknown,
                Reset
            }

            /// <summary>
            /// Status of the media-in transaction. The following values are possible:
            /// 
            /// * ```ok``` - The media-in transaction completed successfully.
            /// * ```active``` - There is a media-in transaction active.
            /// * ```rollback``` - The media-in transaction was successfully rolled back.
            /// * ```rollbackAfterDeposit``` - The media-in transaction was successfully rolled back after some items had
            /// been deposited to a storage unit. This value only applies to devices without a stacker.
            /// * ```retract``` - The media-in transaction ended with the items being successfully retracted.
            /// * ```failure``` - The media-in transaction failed as the result of a device failure.
            /// * ```unknown``` - The state of the media-in transaction is unknown.
            /// * ```reset``` - The media-in transaction ended as the result of a [Reset](#check.reset) or
            /// [CashManagement.Reset](#cashmanagement.reset) command.
            /// <example>active</example>
            /// </summary>
            [DataMember(Name = "mediaInTransaction")]
            public MediaInTransactionEnum? MediaInTransaction { get; init; }

            /// <summary>
            /// Contains the total number of media items currently on the stacker or null if the device has no stacker. This
            /// value can change outside of a transaction as the media moves within the device. Following values are possible:
            /// 
            /// * ```&lt;number&gt;``` - The number of items.
            /// * ```unknown``` - The precise number of items is unknown.
            /// <example>5</example>
            /// </summary>
            [DataMember(Name = "mediaOnStacker")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]+$")]
            public string MediaOnStacker { get; init; }

            /// <summary>
            /// Contains the number of media items processed by the last [MediaIn](#check.mediain) command. This count is
            /// not modified for bunches of items which are refused as a single entity. This count only applies to devices
            /// with stackers is persistent and is therefore null if not applicable. Following values are possible:
            /// 
            /// * ```&lt;number&gt;``` - The number of items.
            /// * ```unknown``` - The precise number of items is unknown.
            /// <example>10</example>
            /// </summary>
            [DataMember(Name = "lastMediaInTotal")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]+$")]
            public string LastMediaInTotal { get; init; }

            /// <summary>
            /// Contains the number of media items on the stacker successfully accepted by the last
            /// [MediaIn](#check.mediain) command.  This count is persistent and is null if the device has no stacker.
            /// 
            /// The number of media items refused during the last command can be determined by
            /// *lastMediaInTotal* - *lastMediaAddedToStacker*. This is only possible if these values contain values,
            /// and would not include bunches of items refused as a single entity.
            /// 
            /// Following values are possible:
            /// 
            /// * ```&lt;number&gt;``` - The number of items.
            /// * ```unknown``` - The precise number of items is unknown.
            /// <example>3</example>
            /// </summary>
            [DataMember(Name = "lastMediaAddedToStacker")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]+$")]
            public string LastMediaAddedToStacker { get; init; }

            /// <summary>
            /// The total number of items that have been allocated a media ID during the whole of the current transaction
            /// (if a transaction is active) or last transaction (if no transaction is active). This count does not
            /// include refused items and Cash items. This count is persistent.
            /// 
            /// Following values are possible:
            /// 
            /// * ```&lt;number&gt;``` - The number of items.
            /// * ```unknown``` - The precise number of items is unknown.
            /// <example>8</example>
            /// </summary>
            [DataMember(Name = "totalItems")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]+$")]
            public string TotalItems { get; init; }

            /// <summary>
            /// Contains the total number of refused items during the execution of the whole transaction. This count does
            /// not include bunches of items which are refused as a single entity without being processed as single items.
            /// This count is persistent. Following values are possible:
            /// 
            /// * ```&lt;number&gt;``` - The number of items.
            /// * ```unknown``` - The precise number of items is unknown.
            /// <example>2</example>
            /// </summary>
            [DataMember(Name = "totalItemsRefused")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]+$")]
            public string TotalItemsRefused { get; init; }

            /// <summary>
            /// Contains the total number of refused bunches of items that were not processed as single items.
            /// This count is persistent. Following values are possible:
            /// 
            /// * ```&lt;number&gt;``` - The number of items.
            /// * ```unknown``` - The precise number of items is unknown.
            /// <example>1</example>
            /// </summary>
            [DataMember(Name = "totalBunchesRefused")]
            [DataTypes(Pattern = @"^unknown$|^[0-9]+$")]
            public string TotalBunchesRefused { get; init; }

            /// <summary>
            /// This array contains details of the media items processed during the current or last transaction
            /// (depending on the value of *mediaInTransaction*). The array contains one element for every item that has
            /// been allocated a media ID (i.e. items that have been reported to the application). If there are no media
            /// items then mediaInfo is null. The media info is available until a new transaction is started
            /// with the [MediaIn](#check.mediain) command. The media location information may be updated after a
            /// transaction is completed, e.g. if media that was presented to the customer is subsequently retracted.
            /// The media info is persistent.
            /// </summary>
            [DataMember(Name = "mediaInfo")]
            public List<MediaStatusClass> MediaInfo { get; init; }

        }
    }
}

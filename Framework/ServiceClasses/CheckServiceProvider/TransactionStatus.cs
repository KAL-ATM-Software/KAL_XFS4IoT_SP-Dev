/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using XFS4IoT.Check;
using XFS4IoT.Commands;
using XFS4IoT.Events;
using XFS4IoTServer;

namespace XFS4IoTFramework.Check
{
    public sealed class MediaStatus : MediaDataInfo
    {
        /// <summary>
        /// Unit - The media is in the storage unit.
        /// Device - The media is in the device.
        /// Customer - The media has been returned and taken by the customer.
        /// UnKnown - The media is in an unknown position.
        /// </summary>
        public enum MediaLocationEnum
        {
            Unit,
            Device,
            Customer,
            Unknown,
        }

        /// <summary>
        /// Unknown - It is not known if the media item has been in a position with customer access.
        /// Customer - The media item has been in a position with customer access.
        /// None - The media item has not been in a position with customer access.
        /// </summary>
        public enum CustomerAccessEnum
        {
            Unknown,
            Customer,
            None,
        }

        public MediaStatus() :
            base(MagneticReadIndicator: MagneticReadIndicatorEnum.Unknown)
        { }

        public MediaStatus(Dictionary<ImageSourceEnum, ImageDataInfo> Images) :
            base(MagneticReadIndicator: MagneticReadIndicatorEnum.Unknown,
                 Images: Images)
        { }

        public MediaStatus(string CodelineData,
                           MagneticReadIndicatorEnum MagneticReadIndicator,
                           Dictionary<ImageSourceEnum, ImageDataInfo> Images,
                           CodelineOrientationEnum CodelineOrientation,
                           MediaOrientationEnum MediaOrientation,
                           MediaSizeInfo MediaSize,
                           MediaValidityEnum MediaValidity) :
            base(MagneticReadIndicator,
                 CodelineData,
                 Images,
                 CodelineOrientation,
                 MediaOrientation,
                 MediaSize,
                 MediaValidity)
        { }

        public MediaStatus(MediaLocationEnum MediaLocation,
                           string UnitLocation,
                           string CodelineData,
                           MagneticReadIndicatorEnum MagneticReadIndicator,
                           Dictionary<ImageSourceEnum, ImageDataInfo> Images,
                           CodelineOrientationEnum CodelineOrientation,
                           MediaOrientationEnum MediaOrientation,
                           MediaSizeInfo MediaSize,
                           MediaValidityEnum MediaValidity,
                           CustomerAccessEnum CustomerAccess) :
            base(MagneticReadIndicator,
                 CodelineData,
                 Images,
                 CodelineOrientation,
                 MediaOrientation,
                 MediaSize,
                 MediaValidity)
        {
            this.MediaLocation = MediaLocation;
            this.UnitLocation = UnitLocation;
            this.CustomerAccess = CustomerAccess;
        }

        /// <summary>
        /// Location of the media.
        /// </summary>
        public MediaLocationEnum MediaLocation { get; set; } = MediaLocationEnum.Unknown;

        /// <summary>
        /// Set storage ID if the property MediaLocation is Unit. otherwise empty string.
        /// </summary>
        public string UnitLocation { get; set; } = null;

        /// <summary>
        /// Customer access status
        /// </summary>
        public CustomerAccessEnum CustomerAccess { get; set; } = CustomerAccessEnum.Unknown;
    }

    public sealed class TransactionStatus
    {
        /// <summary>
        /// Ok - The media-in transaction completed successfully.
        /// Active - There is a media-in transaction active.
        /// Rollback - The media-in transaction was successfully rolled back.
        /// RollbackAfterDeposit - The media-in transaction was successfully rolled back after some items had
        ///     been deposited to a storage unit.This value only applies to devices without a stacker.
        /// Retract - The media-in transaction ended with the items being successfully retracted.
        /// Failure - The media-in transaction failed as the result of a device failure.
        /// Unknown - The state of the media-in transaction is unknown.
        /// Reset - The media-in transaction ended as the result of a reset operation.
        /// </summary>
        public enum MediaInTransactionStateEnum
        {
            Ok,
            Active,
            Rollback,
            RollbackAfterDeposit,
            Retract,
            Failure,
            Unknown,
            Reset,
        }

        public TransactionStatus()
        {
            MediaInTransactionState = MediaInTransactionStateEnum.Ok;
            MediaOnStacker = 0;
            LastMediaInTotal = 0;
            LastMediaAddedToStacker = 0;
            TotalItemsRefused = 0;
            TotalBunchesRefused = 0;
            MediaInfo.Clear();
        }

        /// <summary>
        /// Status of the media-in transaction.
        /// </summary>
        public MediaInTransactionStateEnum MediaInTransactionState { get; set; }

        /// <summary>
        /// Contains the total number of media items currently on the stacker. -1 if it's unknown.
        /// </summary>
        public int MediaOnStacker {  get; set; }

        /// <summary>
        /// Contains the number of media items processed by the last MediaIn operation. This count is
        /// not modified for bunches of items which are refused as a single entity.
        /// This count only applies to devices with stackers is persistent.
        /// -1 if it's unknown.
        /// </summary>
        public int LastMediaInTotal { get; set; }

        /// <summary>
        /// Contains the number of media items on the stacker successfully accepted by the last
        /// MediaIn operation.  This count is persistent and is null if the device has no stacker.
        /// The number of media items refused during the last command can be determined by
        /// LastMediaInTotal property. This is only possible if these values contain values,
        /// and would not include bunches of items refused as a single entity.
        /// -1 if it's unknown.
        /// </summary>
        public int LastMediaAddedToStacker { get; set; }

        /// <summary>
        /// The total number of items that have been allocated a media ID during the whole of the current transaction
        /// (if a transaction is active) or last transaction(if no transaction is active). This count does not
        /// include refused items and Cash items.This count is persistent.
        /// </summary>
        public int TotalItems { get => MediaInfo.Count; }

        /// <summary>
        /// Contains the total number of refused items during the execution of the whole transaction. 
        /// This count does not include bunches of items which are refused as a single entity 
        /// without being processed as single items.
        /// </summary>
        public int TotalItemsRefused { get; set; }

        /// <summary>
        /// Contains the total number of refused bunches of items that were not processed as single items.
        /// </summary>
        public int TotalBunchesRefused { get; set; }

        /// <summary>
        /// This dictionary contains details of the media items processed during the current or last transaction
        /// (depending on the value of MediaInTransaction). 
        /// The dictionary contains one element for every item that has been allocated a media ID
        /// (i.e.items that have been reported to the application). If there are no media
        /// items then this object is an empty.
        /// The media info is available until a new transaction is started
        /// with MediaIn operation. The media location information may be updated after a
        /// transaction is completed, e.g. if media that was presented to the customer is subsequently retracted.
        /// </summary>
        public Dictionary<int, MediaStatus> MediaInfo { get; init; } = [];

        /// <summary>
        /// Reset all transaction status
        /// </summary>
        public void NewTransaction()
        {
            MediaInTransactionState = MediaInTransactionStateEnum.Ok;
            MediaOnStacker = 0;
            LastMediaInTotal = 0;
            LastMediaAddedToStacker = 0;
            TotalItemsRefused = 0;
            TotalBunchesRefused = 0;
            MediaInfo.Clear();
        }
    }
}

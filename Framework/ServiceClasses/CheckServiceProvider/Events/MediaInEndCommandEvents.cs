/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Check;
using XFS4IoT.Commands;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.Check
{
    public sealed class MediaInEndCommandEvents(IStorageService storage, ICheckService check, IMediaInEndEvents events) :
        MediaPresentedCommandEvent(events)
    {

        public Task StorageErrorEvent(FailureEnum Failure, List<string> CashUnitIds) => StorageErrorCommandEvent?.StorageErrorEvent(Failure, CashUnitIds);

        public Task MediaDataEvent(
            int MediaID,
            string CodelineData,
            MediaDataInfo.MagneticReadIndicatorEnum MagneticReadIndicator,
            Dictionary<ImageSourceEnum, ImageDataInfo> Images,
            MediaDataInfo.CodelineOrientationEnum CodelineOrientation,
            MediaDataInfo.MediaOrientationEnum MediaOrientation,
            MediaSizeInfo MediaSize,
            MediaDataInfo.MediaValidityEnum MediaValidity) => MediaDataCommandEvent?.MediaDataEvent(
                MediaID,
                CodelineData,
                MagneticReadIndicator,
                Images,
                CodelineOrientation,
                MediaOrientation,
                MediaSize,
                MediaValidity);


        private StorageErrorCommandEvent StorageErrorCommandEvent { get; init; } = new(storage, events);

        private MediaDataCommandEvent MediaDataCommandEvent { get; init; } = new(check, events);
    }
}
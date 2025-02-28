/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.Events;

namespace XFS4IoTFramework.Check
{
    public class GetNextItemCommandEvents(ICheckService service, IGetNextItemEvents events) : MediaDataCommandEvent(service, events)
    {
        public Task MediaRefusedEvent(
            MediaRefusedCommandEvent.MediaRefusedReasonEnum reason,
            MediaRefusedCommandEvent.MediaRefusedLocationEnum location,
            bool presentRequired,
            MediaSizeInfo mediaSize,
            MediaRefusedCommandEvent.MediaTypeEnum mediaType) => MediaRefused.MediaRefusedEvent(reason, location, presentRequired, mediaSize, mediaType);


        private MediaRefusedCommandEvent MediaRefused { get; init; } = new(service ,events);
    }
}
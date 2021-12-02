/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using XFS4IoTServer;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.CardReader.Events;

namespace XFS4IoTFramework.CardReader
{
    public sealed class WriteCardCommandEvents : AcceptCardCommandEvents
    {
        public enum StatusEnum
        {
            Missing,
            Invalid,
            TooLong,
            TooShort
        }

        public WriteCardCommandEvents(IWriteRawDataEvents events) :
            base(events)
        { }

        public Task InvalidTrackDataEvent(StatusEnum Status, 
                                          string Track, 
                                          string Data)
        {
            WriteRawDataEvents.IsNotNull($"Unexpected interface specified. " + nameof(TrackDetectedEvent));
            return WriteRawDataEvents.InvalidTrackDataEvent(new(Status switch
                                                                {
                                                                    StatusEnum.Invalid => XFS4IoT.CardReader.Events.InvalidTrackDataEvent.PayloadData.StatusEnum.Invalid,
                                                                    StatusEnum.Missing => XFS4IoT.CardReader.Events.InvalidTrackDataEvent.PayloadData.StatusEnum.Missing,
                                                                    StatusEnum.TooLong => XFS4IoT.CardReader.Events.InvalidTrackDataEvent.PayloadData.StatusEnum.TooLong,
                                                                    _ => XFS4IoT.CardReader.Events.InvalidTrackDataEvent.PayloadData.StatusEnum.TooShort,
                                                                },
                                                                Track,
                                                                Data));
        }
    }
}
/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
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
    public sealed class ReadCardCommandEvents : CommonCardCommandEvents
    {
        public ReadCardCommandEvents(IReadRawDataEvents events) :
            base(events)
        { }

        public Task TrackDetectedEvent(bool? Track1 = null,
                                       bool? Track2 = null,
                                       bool? Track3 = null,
                                       bool? Watermark = null,
                                       bool? FrontTrack1 = null)
        {
            ReadRawDataEvents.IsNotNull($"Unexpected interface specified. " + nameof(TrackDetectedEvent));
            return ReadRawDataEvents.TrackDetectedEvent(new TrackDetectedEvent.PayloadData(Track1,
                                                                                           Track2,
                                                                                           Track3,
                                                                                           Watermark,
                                                                                           FrontTrack1));
        }
    }
}
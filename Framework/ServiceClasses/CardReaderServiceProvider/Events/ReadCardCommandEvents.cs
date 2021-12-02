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
    public sealed class ReadCardCommandEvents : AcceptCardCommandEvents
    {
        public ReadCardCommandEvents(IReadRawDataEvents events) :
            base(events)
        { }

        public Task TrackDetectedEvent(bool Track1, 
                                       bool Track2, 
                                       bool Track3, 
                                       bool Watermark, 
                                       bool FrontTrack1)
        {
            ReadRawDataEvents.IsNotNull($"Unexpected interface specified. " + nameof(TrackDetectedEvent));
            return ReadRawDataEvents.TrackDetectedEvent(new(Track1, Track2, Track3, Watermark, FrontTrack1));
        }
    }
}
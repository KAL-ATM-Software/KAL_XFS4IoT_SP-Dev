/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.PinPad
{
    public sealed class PinPadCommandEvents
    {
        public PinPadCommandEvents(IGetPinBlockEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(PinPadCommandEvents));
            events.IsA<IGetPinBlockEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(PinPadCommandEvents));
            GetPinBlockEvents = events;
        }

        public Task DUKPTKSNEvent(string Key, List<byte> KSN)
        {
            GetPinBlockEvents.IsNotNull($"Invalid interface specified. " + nameof(DUKPTKSNEvent));
            return GetPinBlockEvents.DUKPTKSNEvent(new(Key, KSN));
        }

        private IGetPinBlockEvents GetPinBlockEvents { get; init; } = null;
    }
}

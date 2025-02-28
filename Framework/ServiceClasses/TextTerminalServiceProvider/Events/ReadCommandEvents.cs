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
using XFS4IoT.TextTerminal.Events;

namespace XFS4IoTFramework.TextTerminal
{
    public sealed class ReadCommandEvents
    {
        public ReadCommandEvents(IReadEvents events)
        {
            ReadEvents = events;
        }

        public Task KeyEvent(string Key, string CommandKey)
        {
            ReadEvents.IsNotNull($"Invalid interface specified. " + nameof(KeyEvent));

            return ReadEvents.KeyEvent(new KeyEvent.PayloadData(Key, CommandKey));
        }

        private IReadEvents ReadEvents { get; init; }
    }
}

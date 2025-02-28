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
using XFS4IoTFramework.Check;

namespace XFS4IoTFramework.Check
{
    public sealed class PresentMediaCommandEvents(IPresentMediaEvents events) : 
        MediaPresentedCommandEvent(events)
    {
    }
}

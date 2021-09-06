﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.PinPad;
using XFS4IoT.PinPad.Events;

namespace XFS4IoTServer
{
    public interface IPinPadServiceClass : IPinPadUnsolicitedEvents
    {
    }
}

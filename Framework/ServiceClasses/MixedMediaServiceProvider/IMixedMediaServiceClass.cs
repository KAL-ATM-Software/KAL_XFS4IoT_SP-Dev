/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT.CashDispenser.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.MixedMedia
{
    public interface IMixedMediaService
    {

    }

    public interface IMixedMediaServiceClass : IMixedMediaService, IMixedMediaUnsolicitedEvents
    {

    }
}
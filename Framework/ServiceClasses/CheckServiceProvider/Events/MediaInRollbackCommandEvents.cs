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
using XFS4IoT.Check;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Security.Policy;
using XFS4IoT.Commands;
using System.ComponentModel;
using XFS4IoTServer;

namespace XFS4IoTFramework.Check
{
    public class MediaInRollbackCommandEvents(IMediaInRollbackEvents events) : MediaPresentedCommandEvent(events)
    {
    }
}
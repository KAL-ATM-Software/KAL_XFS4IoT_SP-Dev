﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * AuxiliariesServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Auxiliaries;

namespace XFS4IoTServer
{
    public partial class AuxiliariesServiceClass : IAuxiliariesServiceClass
    {

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IAuxiliariesDevice Device { get => ServiceProvider.Device.IsA<IAuxiliariesDevice>(); }
    }
}

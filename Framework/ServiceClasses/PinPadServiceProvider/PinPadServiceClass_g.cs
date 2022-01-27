/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * PinPadServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.PinPad;

namespace XFS4IoTServer
{
    public partial class PinPadServiceClass : IPinPadServiceClass
    {

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IPinPadDevice Device { get => ServiceProvider.Device.IsA<IPinPadDevice>(); }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * KeyboardServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Keyboard;

namespace XFS4IoTServer
{
    public partial class KeyboardServiceClass : IKeyboardServiceClass
    {

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IKeyboardDevice Device { get => ServiceProvider.Device.IsA<IKeyboardDevice>(); }
    }
}

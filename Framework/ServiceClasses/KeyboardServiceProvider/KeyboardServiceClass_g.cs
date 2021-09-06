/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * KeyboardServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Keyboard;

namespace XFS4IoTServer
{
    public partial class KeyboardServiceClass : IKeyboardServiceClass
    {
        public KeyboardServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(KeyboardServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<IKeyboardDevice>();
        }
        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private IKeyboardDevice Device { get => ServiceProvider.Device.IsA<IKeyboardDevice>(); }
    }
}

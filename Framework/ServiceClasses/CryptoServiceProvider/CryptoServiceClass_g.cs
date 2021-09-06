/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Crypto interface.
 * CryptoServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Crypto;

namespace XFS4IoTServer
{
    public partial class CryptoServiceClass : ICryptoServiceClass
    {
        public CryptoServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CryptoServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICryptoDevice>();
        }
        public async Task IllegalKeyAccessEvent(XFS4IoT.Crypto.Events.IllegalKeyAccessEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Crypto.Events.IllegalKeyAccessEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private ICryptoDevice Device { get => ServiceProvider.Device.IsA<ICryptoDevice>(); }
    }
}

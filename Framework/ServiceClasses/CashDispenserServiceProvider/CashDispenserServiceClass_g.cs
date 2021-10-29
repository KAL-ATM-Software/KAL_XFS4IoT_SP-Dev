/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * CashDispenserServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CashDispenser;

namespace XFS4IoTServer
{
    public partial class CashDispenserServiceClass : ICashDispenserServiceClass
    {
        public CashDispenserServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashDispenserServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICashDispenserDevice>();
        }
        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private ICashDispenserDevice Device { get => ServiceProvider.Device.IsA<ICashDispenserDevice>(); }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CashManagementServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTServer
{
    public partial class CashManagementServiceClass : ICashManagementServiceClass
    {
        public CashManagementServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
            this.Logger = logger;
            this.ServiceProvider.Device.IsNotNull($"Invalid parameter received in the {nameof(CashManagementServiceClass)} constructor. {nameof(ServiceProvider.Device)}").IsA<ICashManagementDevice>();
        }
        public async Task TellerInfoChangedEvent(XFS4IoT.CashManagement.Events.TellerInfoChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.TellerInfoChangedEvent(Payload));

        public async Task SafeDoorOpenEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.SafeDoorOpenEvent());

        public async Task SafeDoorClosedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.SafeDoorClosedEvent());

        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private ICashManagementDevice Device { get => ServiceProvider.Device.IsA<ICashManagementDevice>(); }
    }
}

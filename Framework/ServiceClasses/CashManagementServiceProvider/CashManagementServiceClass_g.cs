/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * CashManagementServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTServer
{
    public partial class CashManagementServiceClass : ICashManagementServiceClass
    {

        public async Task TellerInfoChangedEvent(XFS4IoT.CashManagement.Events.TellerInfoChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.TellerInfoChangedEvent(Payload));

        public async Task ItemsTakenEvent(XFS4IoT.CashManagement.Events.ItemsTakenEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.ItemsTakenEvent(Payload));

        public async Task ItemsInsertedEvent(XFS4IoT.CashManagement.Events.ItemsInsertedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.ItemsInsertedEvent(Payload));

        public async Task ItemsPresentedEvent(XFS4IoT.CashManagement.Events.ItemsPresentedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.ItemsPresentedEvent(Payload));

        public async Task MediaDetectedEvent(XFS4IoT.CashManagement.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.MediaDetectedEvent(Payload));

        public async Task ShutterStatusChangedEvent(XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent(Payload));

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private ICashManagementDevice Device { get => ServiceProvider.Device.IsA<ICashManagementDevice>(); }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * DepositServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Deposit;

namespace XFS4IoTServer
{
    public partial class DepositServiceClass : IDepositServiceClass
    {

        public async Task EnvTakenEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.EnvTakenEvent());

        public async Task EnvDepositedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.EnvDepositedEvent());

        public async Task DepositErrorEvent(XFS4IoT.Deposit.Events.DepositErrorEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.DepositErrorEvent(Payload));

        public async Task EnvInsertedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.EnvInsertedEvent());

        public async Task InsertDepositEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.InsertDepositEvent());

        public async Task MediaDetectedEvent(XFS4IoT.Deposit.Events.MediaDetectedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Deposit.Events.MediaDetectedEvent(Payload));

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IDepositDevice Device { get => ServiceProvider.Device.IsA<IDepositDevice>(); }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTServer;

namespace XFS4IoTFramework.Auxiliaries
{
    /// <summary>
    /// Events for Register command / AuxiliaryStatusEvent.
    /// </summary>
    [Flags]
    public enum EventTypesEnum : long
    {
        None = 0,
        OperatorSwitch = 1 << 0,
        TamperSensor = 1 << 1,
        InternalTamperSensor = 1 << 2,
        SeismicSensor = 1 << 3,
        HeatSensor = 1 << 4,
        ProximitySensor = 1 << 5,
        AmbientLightSensor = 1 << 6,
        EnhancedAudio = 1 << 7,
        BootSwitch = 1 << 8,
        ConsumerDisplay = 1 << 9,
        OperatorCallButton = 1 << 10,
        HandsetSensor = 1 << 11,
        HeadsetMicrophone = 1 << 12,
        SafeDoor = 1 << 13,
        VandalShield = 1 << 14,
        CabinetFront = 1 << 15,
        CabinetRear = 1 << 16,
        CabinetRight = 1 << 17,
        CabinetLeft = 1 << 18,
        OpenCloseIndicator = 1 << 19,
        FasciaLight = 1 << 20,
        AudioIndicator = 1 << 21,
        HeatingIndicator = 1 << 22,
        ConsumerDisplayBacklight = 1 << 23,
        SignageDisplay = 1 << 24,
        VolumeControl = 1 << 25,
        Ups = 1 << 26,
        RemoteStatusMonitor = 1 << 27,
        AudibleAlarm = 1 << 28,
        EnhancedAudioControl = 1 << 29,
        EnhancedMicrophoneControl = 1 << 30,
        MicrophoneVolume = 1 << 31,
        FasciaMicrophone = 1 << 32,
    }


    internal class AuxiliariesStatusChangedRegistry
    {

        public AuxiliariesStatusChangedRegistry(ILogger Logger, XFS4IoTServer.IServiceProvider ServiceProvider)
        {
            this.Logger = Logger.IsNotNull();
            this.ServiceProvider = ServiceProvider.IsNotNull();
        }

        private readonly XFS4IoTServer.IServiceProvider ServiceProvider;
        private readonly ILogger Logger;


        internal async Task BroadcastRegisteredStatusChangedEvents(object message, EventTypesEnum eventIdentifier)
        {
            Logger.Log(Constants.DeviceClass, $"Broadcasting Auxiliaries event {eventIdentifier}.");

            List<IConnection> sendConnections;
            using (var syncObj = await DisposableLock.Create(subscriptionsSemaphore, CancellationToken.None))
            {
                sendConnections = (from subscription in Subscriptions
                                   where subscription.Value.HasFlag(eventIdentifier)
                                   select subscription.Key).ToList();
            }
            await ServiceProvider.BroadcastEvent(sendConnections, message);
        }

        internal async Task RegisterStatusChangedEvents(IConnection connection, EventTypesEnum eventIdentifiers, CancellationToken token)
        {
            using (var syncObj = await DisposableLock.Create(subscriptionsSemaphore, token))
            {
                if (Subscriptions.ContainsKey(connection))
                    Subscriptions[connection] |= eventIdentifiers;
                else
                    Subscriptions.Add(connection, eventIdentifiers);
            }
            Logger.Log(nameof(ServiceProvider), $"Registered connection to Auxiliaries events: {eventIdentifiers}.");
        }

        internal async Task DeregisterStatusChangedEvents(IConnection connection, EventTypesEnum eventIdentifiers, CancellationToken token)
        {
            using (var syncObj = await DisposableLock.Create(subscriptionsSemaphore, token))
            {
                if (!Subscriptions.ContainsKey(connection))
                    return;

                Subscriptions[connection] &= ~eventIdentifiers;
            }
            Logger.Log(nameof(ServiceProvider), $"Deregistered connection from Auxiliaries events: {eventIdentifiers}.");
        }

        private readonly SemaphoreSlim subscriptionsSemaphore = new(1, 1);
        private readonly Dictionary<IConnection, EventTypesEnum> Subscriptions = new();
        private class DisposableLock : IDisposable
        {
            private readonly SemaphoreSlim Semaphore;

            public DisposableLock(SemaphoreSlim semaphore)
            {
                Semaphore = semaphore;
            }

            public async Task Aquire(CancellationToken token)
            {
                await Semaphore.WaitAsync(token);
            }

            public void Dispose()
            {
                Semaphore.Release();
            }

            public static async Task<DisposableLock> Create(SemaphoreSlim semaphore, CancellationToken token)
            {
                DisposableLock disposableLock = new(semaphore);
                await disposableLock.Aquire(token);
                return disposableLock;
            }
        }

    }
}

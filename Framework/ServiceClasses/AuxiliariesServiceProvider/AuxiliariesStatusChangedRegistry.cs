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
        OperatorSwitch = 1L << 0,
        TamperSensor = 1L << 1,
        InternalTamperSensor = 1L << 2,
        SeismicSensor = 1L << 3,
        HeatSensor = 1L << 4,
        ProximitySensor = 1L << 5,
        AmbientLightSensor = 1L << 6,
        EnhancedAudio = 1L << 7,
        BootSwitch = 1L << 8,
        ConsumerDisplay = 1L << 9,
        OperatorCallButton = 1L << 10,
        HandsetSensor = 1L << 11,
        HeadsetMicrophone = 1L << 12,
        SafeDoor = 1L << 13,
        VandalShield = 1L << 14,
        CabinetFront = 1L << 15,
        CabinetRear = 1L << 16,
        CabinetRight = 1L << 17,
        CabinetLeft = 1L << 18,
        OpenCloseIndicator = 1L << 19,
        FasciaLight = 1L << 20,
        AudioIndicator = 1L << 21,
        HeatingIndicator = 1L << 22,
        ConsumerDisplayBacklight = 1L << 23,
        SignageDisplay = 1L << 24,
        VolumeControl = 1L << 25,
        Ups = 1L << 26,
        RemoteStatusMonitor = 1L << 27,
        AudibleAlarm = 1L << 28,
        EnhancedAudioControl = 1L << 29,
        EnhancedMicrophoneControl = 1L << 30,
        MicrophoneVolume = 1L << 31,
        FasciaMicrophone = 1L << 32,
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

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
        None = 0x0,
        OperatorSwitch = 0x1,
        TamperSensor = 0x2,
        InternalTamperSensor = 0x4,
        SeismicSensor = 0x8,
        HeatSensor = 0x10,
        ProximitySensor = 0x20,
        AmbientLightSensor = 0x40,
        EnhancedAudio = 0x80,
        BootSwitch = 0x100,
        ConsumerDisplay = 0x200,
        OperatorCallButton = 0x400,
        HandsetSensor = 0x800,
        HeadsetMicrophone = 0x1000,
        SafeDoor = 0x2000,
        VandalShield = 0x4000,
        CabinetFront = 0x8000,
        CabinetRear = 0x10000,
        CabinetRight = 0x20000,
        CabinetLeft = 0x40000,
        OpenCloseIndicator = 0x80000,
        FasciaLight = 0x100000,
        AudioIndicator = 0x200000,
        HeatingIndicator = 0x400000,
        ConsumerDisplayBacklight = 0x800000,
        SignageDisplay = 0x1000000,
        VolumeControl = 0x2000000,
        Ups = 0x4000000,
        RemoteStatusMonitor = 0x8000000,
        AudibleAlarm = 0x10000000,
        EnhancedAudioControl = 0x20000000,
        EnhancedMicrophoneControl = 0x40000000,
        MicrophoneVolume = 0x80000000,
        FasciaMicrophone = 0x100000000
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

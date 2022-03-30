/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * BiometricServiceClass_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;
using XFS4IoTFramework.Biometric;

namespace XFS4IoTServer
{
    public partial class BiometricServiceClass : IBiometricServiceClass
    {

        public async Task PresentSubjectEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.PresentSubjectEvent());

        public async Task SubjectDetectedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.SubjectDetectedEvent());

        public async Task RemoveSubjectEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.RemoveSubjectEvent());

        public async Task SubjectRemovedEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.SubjectRemovedEvent());

        public async Task DataClearedEvent(XFS4IoT.Biometric.Events.DataClearedEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.DataClearedEvent(Payload));

        public async Task OrientationEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.Biometric.Events.OrientationEvent());

        private IServiceProvider ServiceProvider { get; init; }
        private ILogger Logger { get; init; }
        private IBiometricDevice Device { get => ServiceProvider.Device.IsA<IBiometricDevice>(); }
    }
}

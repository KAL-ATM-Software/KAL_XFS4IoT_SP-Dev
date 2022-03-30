/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.Biometric;
using XFS4IoT.Biometric.Events;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public interface IBiometricService
    {
        Task PresentSubjectEvent();
        Task SubjectDetectedEvent();
        Task RemoveSubjectEvent();
        Task SubjectRemovedEvent();
        Task DataClearedEvent(BiometricCapabilitiesClass.ClearModesEnum ClearMode);
        Task OrientationEvent();

    }

    public interface IBiometricServiceClass : IBiometricUnsolicitedEvents
    {
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Biometric interface.
 * ReadEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoT;
using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Biometric
{
    internal class ReadEvents : BiometricEvents, IReadEvents
    {

        public ReadEvents(IConnection connection, int requestId)
            : base(connection, requestId)
        { }

        public async Task PresentSubjectEvent() => await connection.SendMessageAsync(new XFS4IoT.Biometric.Events.PresentSubjectEvent(requestId));

        public async Task SubjectDetectedEvent() => await connection.SendMessageAsync(new XFS4IoT.Biometric.Events.SubjectDetectedEvent(requestId));

        public async Task RemoveSubjectEvent() => await connection.SendMessageAsync(new XFS4IoT.Biometric.Events.RemoveSubjectEvent(requestId));

    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.Camera;
using XFS4IoT.Camera.Events;

namespace XFS4IoTServer
{
    public interface ICameraService
    {
    }

    public interface ICameraServiceClass : ICameraUnsolicitedEvents
    {
    }
}

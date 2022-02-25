/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Camera interface.
 * ICameraEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Camera
{
    public interface ICameraUnsolicitedEvents
    {

        Task MediaThresholdEvent(XFS4IoT.Camera.Events.MediaThresholdEvent.PayloadData Payload);

    }
}

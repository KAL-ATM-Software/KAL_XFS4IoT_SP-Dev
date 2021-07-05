/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * IDispenserEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Dispenser
{
    public interface IDispenserUnsolicitedEvents
    {

        Task ItemsTakenEvent(XFS4IoT.Dispenser.Events.ItemsTakenEvent.PayloadData Payload);

        Task ShutterStatusChangedEvent(XFS4IoT.Dispenser.Events.ShutterStatusChangedEvent.PayloadData Payload);

        Task MediaDetectedEvent(XFS4IoT.Dispenser.Events.MediaDetectedEvent.PayloadData Payload);

        Task ItemsPresentedEvent();

    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * ICashDispenserEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashDispenser
{
    public interface ICashDispenserUnsolicitedEvents
    {

        Task ItemsTakenEvent(XFS4IoT.CashDispenser.Events.ItemsTakenEvent.PayloadData Payload);

        Task ShutterStatusChangedEvent(XFS4IoT.CashDispenser.Events.ShutterStatusChangedEvent.PayloadData Payload);

        Task MediaDetectedEvent(XFS4IoT.CashDispenser.Events.MediaDetectedEvent.PayloadData Payload);

        Task ItemsPresentedEvent();

    }
}

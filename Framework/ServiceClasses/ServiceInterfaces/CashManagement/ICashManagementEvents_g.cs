/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashManagement interface.
 * ICashManagementEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashManagement
{
    public interface ICashManagementUnsolicitedEvents
    {

        Task TellerInfoChangedEvent(XFS4IoT.CashManagement.Events.TellerInfoChangedEvent.PayloadData Payload);

        Task ItemsTakenEvent(XFS4IoT.CashManagement.Events.ItemsTakenEvent.PayloadData Payload);

        Task ItemsInsertedEvent(XFS4IoT.CashManagement.Events.ItemsInsertedEvent.PayloadData Payload);

        Task ItemsPresentedEvent(XFS4IoT.CashManagement.Events.ItemsPresentedEvent.PayloadData Payload);

        Task MediaDetectedEvent(XFS4IoT.CashManagement.Events.MediaDetectedEvent.PayloadData Payload);

        Task ShutterStatusChangedEvent(XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent.PayloadData Payload);

    }
}

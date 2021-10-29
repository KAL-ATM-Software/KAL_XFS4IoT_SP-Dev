/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * IDispenseEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashDispenser
{
    public interface IDispenseEvents
    {

        Task ShutterStatusChangedEvent(XFS4IoT.CashManagement.Events.ShutterStatusChangedEvent.PayloadData Payload);

        Task ItemsTakenEvent(XFS4IoT.CashManagement.Events.ItemsTakenEvent.PayloadData Payload);

        Task NoteErrorEvent(XFS4IoT.CashManagement.Events.NoteErrorEvent.PayloadData Payload);

        Task InfoAvailableEvent(XFS4IoT.CashManagement.Events.InfoAvailableEvent.PayloadData Payload);

        Task DelayedDispenseEvent(XFS4IoT.CashDispenser.Events.DelayedDispenseEvent.PayloadData Payload);

        Task StartDispenseEvent();

        Task PartialDispenseEvent(XFS4IoT.CashDispenser.Events.PartialDispenseEvent.PayloadData Payload);

        Task SubDispenseOkEvent(XFS4IoT.CashDispenser.Events.SubDispenseOkEvent.PayloadData Payload);

        Task IncompleteDispenseEvent(XFS4IoT.CashDispenser.Events.IncompleteDispenseEvent.PayloadData Payload);

    }
}

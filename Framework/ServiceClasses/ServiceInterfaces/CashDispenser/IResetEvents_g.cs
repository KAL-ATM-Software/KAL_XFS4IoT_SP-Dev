/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CashDispenser interface.
 * IResetEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CashDispenser
{
    public interface IResetEvents
    {

        Task CashUnitErrorEvent(XFS4IoT.CashManagement.Events.CashUnitErrorEvent.PayloadData Payload);

        Task InfoAvailableEvent(XFS4IoT.CashManagement.Events.InfoAvailableEvent.PayloadData Payload);

        Task IncompleteRetractEvent(XFS4IoT.CashDispenser.Events.IncompleteRetractEvent.PayloadData Payload);

    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Printer interface.
 * IReadFormEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Printer
{
    public interface IReadFormEvents
    {

        Task NoMediaEvent(XFS4IoT.Printer.Events.NoMediaEvent.PayloadData Payload);

        Task MediaInsertedEvent();

        Task FieldErrorEvent(XFS4IoT.Printer.Events.FieldErrorEvent.PayloadData Payload);

        Task FieldWarningEvent(XFS4IoT.Printer.Events.FieldWarningEvent.PayloadData Payload);

        Task MediaRejectedEvent(XFS4IoT.Printer.Events.MediaRejectedEvent.PayloadData Payload);

    }
}

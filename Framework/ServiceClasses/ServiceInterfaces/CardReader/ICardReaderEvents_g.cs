/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT CardReader interface.
 * ICardReaderEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.CardReader
{
    public interface ICardReaderUnsolicitedEvents
    {

        Task MediaRemovedEvent();

        Task CardActionEvent(XFS4IoT.CardReader.Events.CardActionEvent.PayloadData Payload);

        Task MediaDetectedEvent(XFS4IoT.CardReader.Events.MediaDetectedEvent.PayloadData Payload);

    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * IGetNextItemEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Check
{
    public interface IGetNextItemEvents
    {

        Task MediaRefusedEvent(XFS4IoT.Check.Events.MediaRefusedEvent.PayloadData Payload);

        Task MediaDataEvent(XFS4IoT.Check.Events.MediaDataEvent.PayloadData Payload);

    }
}

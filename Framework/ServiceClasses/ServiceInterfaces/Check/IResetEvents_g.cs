/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Check interface.
 * IResetEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Check
{
    public interface IResetEvents
    {

        Task StorageErrorEvent(XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData Payload);

        Task MediaPresentedEvent(XFS4IoT.Check.Events.MediaPresentedEvent.PayloadData Payload);

    }
}

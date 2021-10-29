/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Storage interface.
 * IStorageEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Storage
{
    public interface IStorageUnsolicitedEvents
    {

        Task StorageThresholdEvent(XFS4IoT.Storage.Events.StorageThresholdEvent.PayloadData Payload);

        Task StorageChangedEvent(XFS4IoT.Storage.Events.StorageChangedEvent.PayloadData Payload);

        Task StorageErrorEvent(XFS4IoT.Storage.Events.StorageErrorEvent.PayloadData Payload);

    }
}

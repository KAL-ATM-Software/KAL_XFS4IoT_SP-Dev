/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PowerManagement interface.
 * IIntelligentBanknoteNeutralizationServiceClass.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.BanknoteNeutralization;
using XFS4IoTFramework.Storage;

namespace XFS4IoTServer
{
    public interface IBanknoteNeutralizationService
    {
        void UpdateStorageStatus(string storageId, UnitStorageBase.StatusEnum storageStatus);
    }

    public interface IBanknoteNeutralizationServiceClass : IBanknoteNeutralizationService, IBanknoteNeutralizationUnsolicitedEvents
    {
    }
}

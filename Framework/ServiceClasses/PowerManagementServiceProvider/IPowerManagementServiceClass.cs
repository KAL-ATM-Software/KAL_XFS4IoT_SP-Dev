/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PowerManagement interface.
 * PowerManagementServiceClass.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.PowerManagement;

namespace XFS4IoTServer
{
    public interface IPowerManagementService
    {
    }

    public interface IPowerManagementServiceClass : IPowerManagementService, IPowerManagementUnsolicitedEvents
    {
    }
}

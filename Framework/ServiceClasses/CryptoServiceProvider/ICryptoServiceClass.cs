/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Threading.Tasks;
using XFS4IoTFramework.Crypto;
using XFS4IoTFramework.Common;

namespace XFS4IoTServer
{
    public interface ICryptoService : IKeyManagementService, ICommonService
    {
    }

    public interface ICryptoServiceClass : ICryptoService, ICryptoUnsolicitedEvents
    {
    }
}

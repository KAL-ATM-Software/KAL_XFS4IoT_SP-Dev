/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Deposit interface.
 * DepositServiceClass.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.Deposit;
using XFS4IoT.Deposit.Events;

namespace XFS4IoTServer
{
    public interface IDepositService
    {
    }

    public interface IDepositServiceClass : IDepositService, IDepositUnsolicitedEvents
    {
    }
}

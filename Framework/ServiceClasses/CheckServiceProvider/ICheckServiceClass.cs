/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2024
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * CheckServiceClass.cs.cs uses automatically generated parts.
\***********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT.CashDispenser.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.Check
{
    public interface ICheckService
    {
        /// <summary>
        /// Store transaction status
        /// </summary>
        TransactionStatus LastTransactionStatus { get; init; }

        /// <summary>
        /// Store transaction status persistently
        /// </summary>
        void StoreTransactionStatus();
    }

    public interface ICheckServiceClass : ICheckService, ICheckUnsolicitedEvents
    {

    }
}

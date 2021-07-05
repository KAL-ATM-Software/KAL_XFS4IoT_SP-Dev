/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using XFS4IoTFramework.CashManagement;
using XFS4IoT.CashManagement.Events;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.CashManagement
{
    public interface ICashManagementService : ICommonService
    {
        /// <summary>
        /// Construct cash unit information given by the device class
        /// </summary>
        void ConstructCashUnits();

        /// <summary>
        /// Update various counts from the device class
        /// </summary>
        void UpdateCashUnitAccounting(Dictionary<string, ItemMovement> MovementResult = null);

        /// <summary>
        /// Cash unit structure information of this device
        /// </summary>
        Dictionary<string, CashUnit> CashUnits { get; set;  }

        /// <summary>
        /// This property is set to true once the framework processed first GetCashUnitInfo command on the start of the day.
        /// </summary>
        bool FirstCashUnitInfoCommand { get; set; }
    }

    public interface ICashManagementServiceClass : ICashManagementService, ICashManagementUnsolicitedEvents
    { 
    }
}

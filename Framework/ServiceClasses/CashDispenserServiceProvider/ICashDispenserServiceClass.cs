/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Dispenser interface.
 * DispenserServiceClass.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT.CashDispenser.Events;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashDispenser
{
    public interface ICashDispenserService : ICashManagementService, ICommonService
    {
        /// <summary>
        /// Adding mix algorithm to an internal object held in the framework
        /// </summary>
        void AddMix(string mixId, Mix mix);

        /// <summary>
        /// Return mix algorithm object specified 
        /// </summary>
        Mix GetMix(string mixId);

        /// <summary>
        /// Return supported mix algorithm
        /// </summary>
        Dictionary<string, Mix> GetMixAlgorithms();

        /// <summary>
        /// Keep last present status
        /// </summary>
        Dictionary<CashDispenserCapabilitiesClass.OutputPositionEnum, PresentStatus> LastPresentStatus { get; set; }
    }

    public interface ICashDispenserServiceClass : ICashDispenserService, ICashDispenserUnsolicitedEvents
    {
    }
}

﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.BarcodeReader;

namespace XFS4IoTServer
{
    public interface IBarcodeReaderService
    {
    }

    public interface IBarcodeReaderServiceClass : IBarcodeReaderUnsolicitedEvents
    {
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Auxiliaries interface.
 * AuxiliariesServiceClass.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.Auxiliaries;
using XFS4IoTFramework.Common;
using XFS4IoT.Auxiliaries;
using System;
using System.Threading;

namespace XFS4IoTServer
{
    public interface IAuxiliariesService
    {
    }

    public interface IAuxiliariesServiceClass : IAuxiliariesService, IAuxiliariesUnsolicitedEvents
    {
    }
}

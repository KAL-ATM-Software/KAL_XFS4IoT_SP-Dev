/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT GermanSpecific interface.
 * GermanSpecificServiceClass.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Threading.Tasks;

using XFS4IoTFramework.German;
using XFS4IoT.German.Events;

namespace XFS4IoTServer
{
    public interface IGermanService
    {
    }

    public interface IGermanServiceClass : IGermanService, IGermanUnsolicitedEvents
    {
    }
}

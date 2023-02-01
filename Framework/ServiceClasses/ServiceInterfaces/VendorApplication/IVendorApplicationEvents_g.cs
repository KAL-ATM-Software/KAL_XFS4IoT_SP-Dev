/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT VendorApplication interface.
 * IVendorApplicationEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.VendorApplication
{
    public interface IVendorApplicationUnsolicitedEvents
    {

        Task InterfaceChangedEvent(XFS4IoT.VendorApplication.Events.InterfaceChangedEvent.PayloadData Payload);

        Task VendorAppExitedEvent();

    }
}

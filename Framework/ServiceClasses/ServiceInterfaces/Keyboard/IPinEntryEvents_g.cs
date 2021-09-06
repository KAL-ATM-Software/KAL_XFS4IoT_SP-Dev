/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * IPinEntryEvents_g.cs uses automatically generated parts.
\***********************************************************************************************/


using XFS4IoTServer;
using System.Threading.Tasks;

namespace XFS4IoTFramework.Keyboard
{
    public interface IPinEntryEvents
    {

        Task KeyEvent(XFS4IoT.Keyboard.Events.KeyEvent.PayloadData Payload);

        Task EnterDataEvent();

        Task LayoutEvent(XFS4IoT.Keyboard.Events.LayoutEvent.PayloadData Payload);

    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using XFS4IoTFramework.Common;


namespace TextTerminal
{
    public interface ITextTerminalConnection : ICommonConnection
    {

        void FieldErrorEvent(XFS4IoT.TextTerminal.Events.FieldErrorEvent.PayloadData Payload);

        void FieldWarningEvent();

        void KeyEvent(XFS4IoT.TextTerminal.Events.KeyEvent.PayloadData Payload);

    }
}

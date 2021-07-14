/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using XFS4IoTFramework.TextTerminal;
using XFS4IoTFramework.Common;
using System.Collections.Generic;

namespace XFS4IoTFramework.TextTerminal
{
    public interface ITextTerminalService : ICommonService
    {
        /// <summary>
        /// True when the SP process gets started and return false once the first GetKeyDetail command is handled.
        /// </summary>
        public bool FirstGetKeyDetailCommand { get; set; }

        record KeyDetails(string Keys, List<string> CommandKeys, XFS4IoT.TextTerminal.Completions.GetKeyDetailCompletion.PayloadData.CommandKeysClass CommandKeysClass);
        
        /// <summary>
        /// Keys supported by the TextTerminal device. Will be filled by the first GetKeyDetail command.
        /// </summary>
        public KeyDetails SupportedKeys { get; set; }


        /// <summary>
        /// Called by the framework to update the SupportedKeys stored internally.
        /// </summary>
        public void UpdateKeyDetails();
    }

    public interface ITextTerminalServiceClass : ITextTerminalService, ITextTerminalUnsolicitedEvents
    {
    }
}

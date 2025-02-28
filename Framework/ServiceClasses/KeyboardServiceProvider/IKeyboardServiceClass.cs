﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Collections.Generic;
using XFS4IoTFramework.Keyboard;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTServer
{
    public interface IKeyboardService
    {
        /// <summary>
        /// Function keys device supported
        /// </summary>
        Dictionary<EntryModeEnum, List<string>> SupportedFunctionKeys { get; set; }

        /// <summary>
        /// Function keys device supported with shift key
        /// </summary>
        Dictionary<EntryModeEnum, List<string>> SupportedFunctionKeysWithShift { get; set; }


        /// <summary>
        /// Keyboard layout device supported
        /// </summary>
        Dictionary<EntryModeEnum, List<FrameClass>> KeyboardLayouts { get; set; }
    }

    public interface IKeyboardServiceClass : IKeyboardService, IKeyboardUnsolicitedEvents
    {
    }
}

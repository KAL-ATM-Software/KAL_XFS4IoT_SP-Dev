﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT TextTerminal interface.
 * TextTerminalServiceProvider.cs.cs uses automatically generated parts.
\***********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

using XFS4IoT;

namespace XFS4IoTServer
{
    public partial class TextTerminalServiceClass : ITextTerminalServiceClass
    {
        public TextTerminalServiceClass(IServiceProvider ServiceProvider, ILogger logger)
        {
            this.ServiceProvider = ServiceProvider.IsNotNull();
        }
        public async Task FieldErrorEvent(XFS4IoT.TextTerminal.Events.FieldErrorEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.TextTerminal.Events.FieldErrorEvent(Payload));

        public async Task FieldWarningEvent()
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.TextTerminal.Events.FieldWarningEvent());

        public async Task KeyEvent(XFS4IoT.TextTerminal.Events.KeyEvent.PayloadData Payload)
            => await ServiceProvider.BroadcastEvent(new XFS4IoT.TextTerminal.Events.KeyEvent(Payload));

        private readonly IServiceProvider ServiceProvider;
    }
}

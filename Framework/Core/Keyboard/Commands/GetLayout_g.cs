/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * GetLayout_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Keyboard.Commands
{
    //Original name = GetLayout
    [DataContract]
    [Command(Name = "Keyboard.GetLayout")]
    public sealed class GetLayoutCommand : Command<GetLayoutCommand.PayloadData>
    {
        public GetLayoutCommand(int RequestId, GetLayoutCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, EntryModeEnum? EntryMode = null)
                : base(Timeout)
            {
                this.EntryMode = EntryMode;
            }

            public enum EntryModeEnum
            {
                Data,
                Pin,
                Secure,
                All
            }

            /// <summary>
            /// Specifies entry mode to be returned. The following values are possible:
            /// 
            /// * ```data``` - Specifies that the layout be applied to the DataEntry method.
            /// * ```pin``` - Specifies that the layout be applied to the PinEntry method.
            /// * ```secure``` - Specifies that the layout be applied to the SecureKeyEntry method.
            /// * ```all``` - Specifies that the layout be applied to all supported entry methods.
            /// </summary>
            [DataMember(Name = "entryMode")]
            public EntryModeEnum? EntryMode { get; init; }

        }
    }
}

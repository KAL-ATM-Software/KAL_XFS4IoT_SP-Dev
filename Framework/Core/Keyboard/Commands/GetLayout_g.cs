/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.0")]
    [Command(Name = "Keyboard.GetLayout")]
    public sealed class GetLayoutCommand : Command<GetLayoutCommand.PayloadData>
    {
        public GetLayoutCommand(int RequestId, GetLayoutCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(EntryModeEnum? EntryMode = null)
                : base()
            {
                this.EntryMode = EntryMode;
            }

            public enum EntryModeEnum
            {
                Data,
                Pin,
                Secure
            }

            /// <summary>
            /// Specifies entry mode to be returned. If this property is null, all layouts for the [Keyboard.DataEntry](#keyboard.dataentry),
            /// [Keyboard.PinEntry](#keyboard.pinentry) and [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command are returned.
            /// 
            /// The following values are possible:
            /// 
            /// * ```data``` - Get the layout for the [Keyboard.DataEntry](#keyboard.dataentry) command.
            /// * ```pin``` - Get the layout for the [Keyboard.PinEntry](#keyboard.pinentry) command.
            /// * ```secure``` - Get the layout for the [Keyboard.SecureKeyEntry](#keyboard.securekeyentry) command.
            /// </summary>
            [DataMember(Name = "entryMode")]
            public EntryModeEnum? EntryMode { get; init; }

        }
    }
}

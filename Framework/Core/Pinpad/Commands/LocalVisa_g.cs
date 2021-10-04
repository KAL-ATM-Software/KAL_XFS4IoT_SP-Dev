/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * LocalVisa_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.PinPad.Commands
{
    //Original name = LocalVisa
    [DataContract]
    [Command(Name = "PinPad.LocalVisa")]
    public sealed class LocalVisaCommand : Command<LocalVisaCommand.PayloadData>
    {
        public LocalVisaCommand(int RequestId, LocalVisaCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string Pan = null, string Pvv = null, string Key = null, string KeyEncKey = null)
                : base(Timeout)
            {
                this.Pan = Pan;
                this.Pvv = Pvv;
                this.Key = Key;
                this.KeyEncKey = KeyEncKey;
            }

            /// <summary>
            /// Primary Account Number from track data, as an ASCII string. 
            /// PAN should contain the eleven rightmost digits of the PAN (excluding the check digit), 
            /// followed by the PVKI indicator in the 12th byte.
            /// </summary>
            [DataMember(Name = "pan")]
            [DataTypes(Pattern = "^[0-9]{23}$")]
            public string Pan { get; init; }

            /// <summary>
            /// PIN Validation Value from track data.
            /// </summary>
            [DataMember(Name = "pvv")]
            [DataTypes(Pattern = "^[0-9]{4,}$")]
            public string Pvv { get; init; }

            /// <summary>
            /// Name of the validation key. The key referenced by key must have the [keyUsage](#common.capabilities.completion.properties.keymanagement.keyattributes.m0) 'V2' attribute.
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// If this property is omitted, key is used directly for PIN validation. Otherwise, key is used to decrypt the 
            /// encrypted key passed in *keyEncKey* and the result is used for PIN validation. 
            /// </summary>
            [DataMember(Name = "keyEncKey")]
            public string KeyEncKey { get; init; }

        }
    }
}

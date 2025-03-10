/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * LocalDES_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.PinPad.Commands
{
    //Original name = LocalDES
    [DataContract]
    [XFS4Version(Version = "2.0")]
    [Command(Name = "PinPad.LocalDES")]
    public sealed class LocalDESCommand : Command<LocalDESCommand.PayloadData>
    {
        public LocalDESCommand(int RequestId, LocalDESCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(string ValidationData = null, string Offset = null, string Padding = null, int? MaxPIN = null, int? ValDigits = null, bool? NoLeadingZero = null, string Key = null, string KeyEncKey = null, string DecTable = null)
                : base()
            {
                this.ValidationData = ValidationData;
                this.Offset = Offset;
                this.Padding = Padding;
                this.MaxPIN = MaxPIN;
                this.ValDigits = ValDigits;
                this.NoLeadingZero = NoLeadingZero;
                this.Key = Key;
                this.KeyEncKey = KeyEncKey;
                this.DecTable = DecTable;
            }

            /// <summary>
            /// Customer specific data (normally obtained from card track data) used to validate the correctness of
            /// the PIN. The validation data should be an ASCII string.
            /// <example>0812746533758375</example>
            /// </summary>
            [DataMember(Name = "validationData")]
            [DataTypes(Sensitive = true, Pattern = @"^[0-9]{16}$")]
            public string ValidationData { get; init; }

            /// <summary>
            /// ASCII string defining the offset data for the PIN block as an ASCII string.
            /// If this property is null then no offset is used.
            /// The character must be in the ranges '0' to '9', 'a' to 'f' and 'A' to 'F'.
            /// <example>0000000000000000</example>
            /// </summary>
            [DataMember(Name = "offset")]
            [DataTypes(Pattern = @"^[0-9a-fA-F]{1,16}$")]
            public string Offset { get; init; }

            /// <summary>
            /// Specifies the padding character for the validation data.
            /// If the validation data is less than 16-characters long then it will be padded with this character.
            /// If padding is in the range 00 to 0F in 16 character string, padding is applied after the validation
            /// data has been compressed. If the character is in the range 30 to 39 ('0' to '9'), 41 to 46 ('a' to
            /// 'f'), or 61 to 66 ('A' to 'F'), padding is applied before the validation data is compressed.
            /// </summary>
            [DataMember(Name = "padding")]
            [DataTypes(Pattern = @"^0[0-9a-fA-F]$|^3[0-9]$|^4[1-6]$|^6[1-6]$")]
            public string Padding { get; init; }

            /// <summary>
            /// Maximum number of PIN digits to be used for validation.
            /// This property corresponds to PINMINL in the IBM 3624 specification (see
            /// [[Ref. pinpad-4](#ref-pinpad-4)]).
            /// </summary>
            [DataMember(Name = "maxPIN")]
            [DataTypes(Minimum = 0)]
            public int? MaxPIN { get; init; }

            /// <summary>
            /// Number of Validation digits from the validation data to be used for validation. This is the length of
            /// the *validationData*.
            /// </summary>
            [DataMember(Name = "valDigits")]
            [DataTypes(Minimum = 0)]
            public int? ValDigits { get; init; }

            /// <summary>
            /// If true and the first digit of result of the modulo 10 addition is a 0x0, it is replaced with 0x1
            /// before performing the verification against the entered PIN. If false, a leading zero is allowed in
            /// entered PINs.
            /// </summary>
            [DataMember(Name = "noLeadingZero")]
            public bool? NoLeadingZero { get; init; }

            /// <summary>
            /// Name of the key to be used for validation. The key referenced by key must have the
            /// [keyUsage](#common.capabilities.completion.properties.keymanagement.keyattributes.m0) 'V0' attribute.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// If this value is null, *key* is used directly for PIN validation. Otherwise, *key* is used to
            /// decrypt the encrypted key passed in *keyEncKey* and the result is used for PIN validation.
            /// <example>Key02</example>
            /// </summary>
            [DataMember(Name = "keyEncKey")]
            public string KeyEncKey { get; init; }

            /// <summary>
            /// ASCII decimalization table (16-character string containing characters '0' to '9'). This table is used
            /// to convert the hexadecimal digits (0x0 to 0xF) of the encrypted validation data to decimal digits (0x0
            /// to 0x9).
            /// <example>3183042102277795</example>
            /// </summary>
            [DataMember(Name = "decTable")]
            [DataTypes(Pattern = @"^[0-9]{16}$")]
            public string DecTable { get; init; }

        }
    }
}

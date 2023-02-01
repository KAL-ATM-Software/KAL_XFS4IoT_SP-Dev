/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2023
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * GetPinBlock_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.PinPad.Commands
{
    //Original name = GetPinBlock
    [DataContract]
    [Command(Name = "PinPad.GetPinBlock")]
    public sealed class GetPinBlockCommand : Command<GetPinBlockCommand.PayloadData>
    {
        public GetPinBlockCommand(int RequestId, GetPinBlockCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, string CustomerData = null, string XorData = null, int? Padding = null, FormatEnum? Format = null, string Key = null, string SecondEncKey = null, CryptoMethodEnum? CryptoMethod = null)
                : base(Timeout)
            {
                this.CustomerData = CustomerData;
                this.XorData = XorData;
                this.Padding = Padding;
                this.Format = Format;
                this.Key = Key;
                this.SecondEncKey = SecondEncKey;
                this.CryptoMethod = CryptoMethod;
            }

            /// <summary>
            /// The customer data should be an ASCII string. Used for ANSI, ISO-0 and ISO-1 algorithm 
            /// (See [[Ref. pinpad-1](#ref-pinpad-1)], [[Ref. pinpad-2](#ref-pinpad-2)], [[Ref. pinpad-3](#ref-pinpad-3)]) to build the formatted PIN. 
            /// For ANSI and ISO-0 the PAN (Primary Account Number, without the check number) is supplied, for ISO-1 a ten digit 
            /// transaction field is required. If not used, this property can be omitted.
            /// 
            /// Used for DIEBOLD with coordination number, as a two digit coordination number.
            /// 
            /// Used for EMV with challenge number (8 bytes) coming from the chip card. 
            /// This number is passed as unpacked string, for example: 0123456789ABCDEF = 0x30 0x31 0x32 0x33 0x34 0x35 0x36 0x37 
            /// 0x38 0x39 0x41 0x42 0x43 0x44 0x45 0x46
            /// 
            /// For AP PIN blocks, the data must be a concatenation of the PAN (18 digits including the check digit), and the CCS (8 digits).
            /// <example>9385527846382726</example>
            /// </summary>
            [DataMember(Name = "customerData")]
            [DataTypes(Pattern = @"^[0-9a-fA-F]{2,}$")]
            public string CustomerData { get; init; }

            /// <summary>
            /// If the formatted PIN is encrypted twice to build the resulting PIN block, this data can be used to modify the result 
            /// of the first encryption by an XOR-operation. If this value is omitted no XOR-operation will be performed.
            /// 
            /// The format is a string of case-insensitive hexadecimal data.
            /// 
            /// If the formatted PIN is not encrypted twice (i.e. if the 
            /// [secondEncKey](#pinpad.getpinblock.command.properties.secondenckey) property is omitted) this is ignored.
            /// <example>0123456789ABCDEF</example>
            /// </summary>
            [DataMember(Name = "xorData")]
            [DataTypes(Pattern = @"^[0-9a-fA-F]{2,}$")]
            public string XorData { get; init; }

            /// <summary>
            /// Specifies the padding character. This property is not applicable for PIN block formats with fixed,
            /// sequential or random padding and can be omitted.
            /// <example>2</example>
            /// </summary>
            [DataMember(Name = "padding")]
            [DataTypes(Minimum = 0, Maximum = 15)]
            public int? Padding { get; init; }

            public enum FormatEnum
            {
                Ibm3624,
                Ansi,
                Iso0,
                Iso1,
                Eci2,
                Eci3,
                Visa,
                Diebold,
                DieboldCo,
                Visa3,
                Banksys,
                Emv,
                Iso3,
                Ap,
                Iso4
            }

            /// <summary>
            /// Specifies the format of the PIN block.
            /// For a list of valid values see [pinFormats](#common.capabilities.completion.properties.pinpad.pinformats).
            /// </summary>
            [DataMember(Name = "format")]
            public FormatEnum? Format { get; init; }

            /// <summary>
            /// Specifies the key used to encrypt the formatted PIN for the first time, this property is not required if no encryption is required. 
            /// If this specifies a double-length or triple-length key, triple DES encryption will be performed. 
            /// The key referenced by key property must have the function or pinRemote attribute. 
            /// If this specifies an RSA key, RSA encryption will be performed.
            /// <example>PinKey01</example>
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// Specifies the key used to format the once encrypted formatted PIN, this property can be omitted if no second encryption required. 
            /// The key referenced by *secondEncKey* must have the [keyUsage](#common.capabilities.completion.properties.keymanagement.keyattributes.m0) 'P0' attribute. 
            /// If this specifies a double-length or triple-length key, triple DES encryption will be performed.
            /// <example>Key01</example>
            /// </summary>
            [DataMember(Name = "secondEncKey")]
            public string SecondEncKey { get; init; }

            public enum CryptoMethodEnum
            {
                Ecb,
                Cbc,
                Cfb,
                Ofb,
                Ctr,
                Xts,
                RsaesPkcs1V15,
                RsaesOaep
            }

            /// <summary>
            /// This specifies the cryptographic method to be used for this command, this property is not required if no
            /// encryption is required. For a list of valid values see
            /// [cryptoMethod](#common.capabilities.completion.properties.pinpad.pinblockattributes.p0.t.e.cryptomethod).
            /// If specified, this must be compatible with the key identified by *key*.
            /// </summary>
            [DataMember(Name = "cryptoMethod")]
            public CryptoMethodEnum? CryptoMethod { get; init; }

        }
    }
}

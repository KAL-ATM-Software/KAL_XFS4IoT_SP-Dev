/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * SetPinBlockData_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.PinPad.Commands
{
    //Original name = SetPinBlockData
    [DataContract]
    [Command(Name = "PinPad.SetPinBlockData")]
    public sealed class SetPinBlockDataCommand : Command<SetPinBlockDataCommand.PayloadData>
    {
        public SetPinBlockDataCommand(int RequestId, SetPinBlockDataCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, Common.CompletionCodeEnumEnum? CompletionCode = null, string ErrorDescription = null, ErrorCodeEnum? ErrorCode = null, string CustomerData = null, string XorData = null, int? Padding = null, FormatEnum? Format = null, string Key = null, string SecondEncKey = null, PinBlockAttributesClass PinBlockAttributes = null)
                : base(Timeout)
            {
                this.CompletionCode = CompletionCode;
                this.ErrorDescription = ErrorDescription;
                this.ErrorCode = ErrorCode;
                this.CustomerData = CustomerData;
                this.XorData = XorData;
                this.Padding = Padding;
                this.Format = Format;
                this.Key = Key;
                this.SecondEncKey = SecondEncKey;
                this.PinBlockAttributes = PinBlockAttributes;
            }

            /// <summary>
            /// The [completion code](#api.generalinformation.commandsequence.completioncodes). If the value is
            /// *commandErrorCode*, the *errorCode* property contains the command specific completion error code.
            /// </summary>
            [DataMember(Name = "completionCode")]
            public Common.CompletionCodeEnumEnum? CompletionCode { get; init; }

            /// <summary>
            /// If included, this contains additional vendor dependent information to assist with problem resolution.
            /// 
            /// </summary>
            [DataMember(Name = "errorDescription")]
            public string ErrorDescription { get; init; }

            public enum ErrorCodeEnum
            {
                KeyNotFound,
                AccessDenied,
                KeyNoValue,
                UseViolation,
                NoPin,
                FormatNotSupported,
                InvalidKeyLength
            }

            /// <summary>
            /// Specifies the error code if applicable. The following values are possible:
            /// * ```keyNotFound``` - The specified key was not found.
            /// * ```accessDenied``` - The encryption module is either not initialized or not ready for any vendor specific reason.
            /// * ```keyNoValue``` - The specified key name was found but the corresponding key value has not been loaded.
            /// * ```useViolation``` - The use specified by keyUsage is not supported.
            /// * ```noPin``` - The PIN has not been entered was not long enough or has been cleared.
            /// * ```formatNotSupported``` - The specified format is not supported.
            /// * ```invalidKeyLength``` - The length of keyEncKey or key is not supported by this key or the length of an encryption 
            /// key is not compatible with the encryption operation required.
            /// </summary>
            [DataMember(Name = "errorCode")]
            public ErrorCodeEnum? ErrorCode { get; init; }

            /// <summary>
            /// The customer data should be an ASCII string. Used for ANSI, ISO-0 and ISO-1 algorithm to build the formatted PIN. 
            /// For ANSI and ISO-0 the PAN (Primary Account Number, without the check number) is supplied, for ISO-1 a ten digit 
            /// transaction field is required. If not used, this property can be omitted. Used for DIEBOLD with coordination number, as a 
            /// two digit coordination number. Used for EMV with challenge number (8 bytes) coming from the chip card. 
            /// This number is passed as unpacked string, for example: 0123456789ABCDEF = 0x30 0x31 0x32 0x33 0x34 0x35 0x36 0x37 
            /// 0x38 0x39 0x41 0x42 0x43 0x44 0x45 0x46 For AP PIN blocks, the data must be a concatenation of the PAN (18 digits 
            /// including the check digit), and the CCS (8 digits).
            /// </summary>
            [DataMember(Name = "customerData")]
            [DataTypes(Pattern = "^[0-9a-fA-F]{2,}$")]
            public string CustomerData { get; init; }

            /// <summary>
            /// If the formatted PIN is encrypted twice to build the resulting PIN block, this data can be used to modify the result 
            /// of the first encryption by an XOR-operation. This parameter is a string of hexadecimal data that must be converted by 
            /// the application, e.g. 0x0123456789ABCDEF must be converted to 0x30 0x31 0x32 0x33 0x34 0x35 0x36 0x37 0x38 0x39 0x41 
            /// 0x42 0x43 0x44 0x45 0x46 and terminated with 0x00. In other words the application would set xorData to “0123456789ABCDEF”. 
            /// The hex digits 0xA to 0xF can be represented by characters in the ranges ‘a’ to ‘f’ or ‘A’ to ‘F’. If this value is omitted 
            /// no XOR-operation will be performed. If the formatted PIN is not encrypted twice (i.e. if the [secondEncKey](#pinpad.getpinblock.command.properties.secondenckey) property is omitted) this parameter is ignored.
            /// </summary>
            [DataMember(Name = "xorData")]
            [DataTypes(Pattern = "^[0-9a-fA-F]{2,}$")]
            public string XorData { get; init; }

            /// <summary>
            /// Specifies the padding character. The valid range is 0 to 15. 
            /// Only the least significant nibble is used. This property is ignored for PIN block formats with fixed, sequential or random padding.
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
                Dieboldco,
                Visa3,
                Banksys,
                Emv,
                Iso3,
                Ap
            }

            /// <summary>
            /// Specifies the format of the PIN block.
            /// Possible values are: (see [pinformats](#common.capabilities.completion.properties.pinpad.pinformats))  
            /// * ```ibm3624``` - PIN left justified, filled with padding characters, PIN length 4-16 digits. The padding character is a hexadecimal digit 
            /// in the range 0x00 to 0x0F."
            /// * ```ansi``` - PIN is preceded by 0x00 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F to the right, PIN length 
            /// 4-12 digits, XORed with PAN (Primary Account Number, minimum 12 digits without check number).  
            /// * ```iso0``` - PIN is preceded by 0x00 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F to the right, PIN length 
            /// 4-12 digits, XORed with PAN (Primary Account Number without check number, no minimum length specified, missing digits are filled with 0x00).  
            /// * ```iso1``` - PIN is preceded by 0x01 and the length of the PIN (0x04 to 0x0C), padding characters are taken from a transaction field (10 digits).  
            /// * ```eci2``` - PIN left justified, filled with padding characters, PIN only 4 digits. 
            /// * ```eci3``` - PIN is preceded by the length (digit), PIN length 4-6 digits, the padding character can range from 0x0 through 0xF. 
            /// * ```visa``` - PIN is preceded by the length (digit), PIN length 4-6 digits. If the PIN length is less than six digits the PIN is filled with 0x0 
            /// to the length of six, the padding character can range from 0x0 through 0x9 (This format is also referred to as VISA2).  
            /// * ```diebold``` - PIN is padded with the padding character and may be not encrypted, single encrypted or double encrypted. 
            /// * ```dieboldco``` - PIN with the length of 4 to 12 digits, each one with a value of 0x0 to 0x9, is preceded by the one-digit coordination number 
            /// with a value from 0x0 to 0xF, padded with the padding character with a value from 0x0 to 0xF and may be not encrypted, single encrypted or double encrypted.  
            /// * ```visa3``` - PIN with the length of 4 to 12 digits, each one with a value of 0x0 to 0x9, is followed by a delimiter with the value of 0xF and then padded 
            /// by the padding character with a value between 0x0 to 0xF.  
            /// * ```banksys``` - PIN is encrypted and formatted according to the Banksys PIN block specifications.
            /// * ```emv``` - The PIN block is constructed as follows: PIN is preceded by 0x02 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F 
            /// to the right, formatted up to 248 bytes of other data as defined within the EMV 4.0 specifications and finally encrypted with an RSA key.  
            /// * ```iso3``` - PIN is preceded by 0x03 and the length of the PIN (0x04 to 0x0C), padding characters sequentially or randomly chosen, XORed with digits from PAN. 
            /// * ```ap``` - PIN is formatted according to the Italian Bancomat specifications. It is known as the Authentication Parameter PIN block and is created with a
            /// 5 digit PIN, an 18 digit PAN, and the 8 digit CCS from the track data. 
            /// </summary>
            [DataMember(Name = "format")]
            public FormatEnum? Format { get; init; }

            /// <summary>
            /// Specifies the key used to encrypt the formatted PIN for the first time, this property is not required if no encryption is required. 
            /// If this specifies a double-length or triple-length key, triple DES encryption will be performed. 
            /// The key referenced by key property must have the function or pinRemote attribute. 
            /// If this specifies an RSA key, RSA encryption will be performed
            /// </summary>
            [DataMember(Name = "key")]
            public string Key { get; init; }

            /// <summary>
            /// Specifies the _key_ used to format the once encrypted formatted PIN, this property can be omitted if no second encryption required. 
            /// The key referenced by _secondEncKey_ must have the [keyUsage](#common.capabilities.completion.properties.keymanagement.keyattributes.m0) 'P0' attribute. 
            /// If this specifies a double-length or triple-length key, triple DES encryption will be performed.
            /// </summary>
            [DataMember(Name = "secondEncKey")]
            public string SecondEncKey { get; init; }

            [DataContract]
            public sealed class PinBlockAttributesClass
            {
                public PinBlockAttributesClass(CryptoMethodEnum? CryptoMethod = null)
                {
                    this.CryptoMethod = CryptoMethod;
                }

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
                /// This parameter specifies the cryptographic method [cryptomethod](#common.capabilities.completion.properties.pinpad.pinblockattributes.p0.t.e.cryptomethod) that will be used with the encryption algorithm.
                /// If the algorithm is ['A', 'D', or 'T'](#common.capabilities.completion.properties.pinpad.pinblockattributes.p0.t), then this property can be one of the following values:" 
                /// 
                /// * ```ecb``` - The ECB encryption method. 
                /// * ```cbc``` - The CBC encryption method.  
                /// * ```cfb``` - The CFB encryption method.  
                /// * ```ofb``` - The OFB encryption method. 
                /// * ```ctr``` - The CTR method defined in NIST SP800-38A.  
                /// * ```xts``` - The XTS method defined in NIST SP800-38E. 
                ///  
                /// If the algorithm is ['R'](#common.capabilities.completion.properties.pinpad.pinblockattributes.p0.t), then this property can be one of the following values:  
                /// 
                /// * ```rsaesPkcs1V15``` - Use the RSAES_PKCS1-v1.5 algorithm. 
                /// * ```rsaesOaep``` - Use the RSAES OAEP algorithm.
                /// </summary>
                [DataMember(Name = "cryptoMethod")]
                public CryptoMethodEnum? CryptoMethod { get; init; }

            }

            /// <summary>
            /// This parameter specifies the encryption algorithm, cryptographic method, and mode to be used for this command. For a list of valid values see the 
            /// [pinBlockAttributes](#common.capabilities.completion.properties.pinpad.pinblockattributes).
            /// The values specified must be compatible with the key identified by *key*.
            /// </summary>
            [DataMember(Name = "pinBlockAttributes")]
            public PinBlockAttributesClass PinBlockAttributes { get; init; }

        }
    }
}

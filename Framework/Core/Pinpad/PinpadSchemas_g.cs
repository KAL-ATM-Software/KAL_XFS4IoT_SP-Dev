/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT PinPad interface.
 * PinPadSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.PinPad
{

    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(PinFormatsClass PinFormats = null, PresentationAlgorithmsClass PresentationAlgorithms = null, DisplayClass Display = null, bool? IdcConnect = null, ValidationAlgorithmsClass ValidationAlgorithms = null, bool? PinCanPersistAfterUse = null, bool? TypeCombined = null, bool? SetPinblockDataRequired = null, Dictionary<string, Dictionary<string, Dictionary<string, PinBlockAttributesClass>>> PinBlockAttributes = null)
        {
            this.PinFormats = PinFormats;
            this.PresentationAlgorithms = PresentationAlgorithms;
            this.Display = Display;
            this.IdcConnect = IdcConnect;
            this.ValidationAlgorithms = ValidationAlgorithms;
            this.PinCanPersistAfterUse = PinCanPersistAfterUse;
            this.TypeCombined = TypeCombined;
            this.SetPinblockDataRequired = SetPinblockDataRequired;
            this.PinBlockAttributes = PinBlockAttributes;
        }

        [DataContract]
        public sealed class PinFormatsClass
        {
            public PinFormatsClass(bool? Ibm3624 = null, bool? Ansi = null, bool? Iso0 = null, bool? Iso1 = null, bool? Eci2 = null, bool? Eci3 = null, bool? Visa = null, bool? Diebold = null, bool? DieboldCo = null, bool? Visa3 = null, bool? Emv = null, bool? Iso3 = null, bool? Ap = null)
            {
                this.Ibm3624 = Ibm3624;
                this.Ansi = Ansi;
                this.Iso0 = Iso0;
                this.Iso1 = Iso1;
                this.Eci2 = Eci2;
                this.Eci3 = Eci3;
                this.Visa = Visa;
                this.Diebold = Diebold;
                this.DieboldCo = DieboldCo;
                this.Visa3 = Visa3;
                this.Emv = Emv;
                this.Iso3 = Iso3;
                this.Ap = Ap;
            }

            /// <summary>
            /// PIN left justified, filled with padding characters, PIN length 4-16 digits.
            /// The padding character is a hexadecimal digit in the range 0x00 to 0x0F.
            /// </summary>
            [DataMember(Name = "ibm3624")]
            public bool? Ibm3624 { get; init; }

            /// <summary>
            /// PIN is preceded by 0x00 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F to the right, PIN length 4-12 digits, 
            /// XORed with PAN (Primary Account Number, minimum 12 digits without check number).
            /// </summary>
            [DataMember(Name = "ansi")]
            public bool? Ansi { get; init; }

            /// <summary>
            /// PIN is preceded by 0x00 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F to the right, PIN length 4-12 digits,
            /// XORed with PAN (Primary Account Number without check number, no minimum length specified, missing digits are filled with 0x00).
            /// </summary>
            [DataMember(Name = "iso0")]
            public bool? Iso0 { get; init; }

            /// <summary>
            /// PIN is preceded by 0x01 and the length of the PIN (0x04 to 0x0C), padding characters are taken from a transaction field (10 digits).
            /// </summary>
            [DataMember(Name = "iso1")]
            public bool? Iso1 { get; init; }

            /// <summary>
            /// PIN left justified, filled with padding characters, PIN only 4 digits.
            /// </summary>
            [DataMember(Name = "eci2")]
            public bool? Eci2 { get; init; }

            /// <summary>
            /// PIN is preceded by the length (digit), PIN length 4-6 digits, the padding character can range from 0x0 through 0xF".
            /// </summary>
            [DataMember(Name = "eci3")]
            public bool? Eci3 { get; init; }

            /// <summary>
            /// PIN is preceded by the length (digit), PIN length 4-6 digits.
            /// If the PIN length is less than six digits the PIN is filled with 0x0 to the length of six, the padding character can range from 0x0 
            /// through 0x9 (This format is also referred to as VISA2).
            /// </summary>
            [DataMember(Name = "visa")]
            public bool? Visa { get; init; }

            /// <summary>
            /// PIN is padded with the padding character and may be not encrypted, single encrypted or double encrypted.
            /// </summary>
            [DataMember(Name = "diebold")]
            public bool? Diebold { get; init; }

            /// <summary>
            /// PIN with the length of 4 to 12 digits, each one with a value of 0x0 to 0x9, is preceded by the one-digit coordination number with 
            /// a value from 0x0 to 0xF, padded with the padding character with a value from 0x0 to 0xF and may be not encrypted, single encrypted or double encrypted.
            /// </summary>
            [DataMember(Name = "dieboldCo")]
            public bool? DieboldCo { get; init; }

            /// <summary>
            /// PIN with the length of 4 to 12 digits, each one with a value of 0x0 to 0x9, is followed by a delimiter with the value of 0xF and then padded 
            /// by the padding character with a value between 0x0 to 0xF.
            /// </summary>
            [DataMember(Name = "visa3")]
            public bool? Visa3 { get; init; }

            /// <summary>
            /// The PIN block is constructed as follows: PIN is preceded by 0x02 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F 
            /// to the right, formatted up to 248 bytes of other data as defined within the EMV 4.0 specifications and finally encrypted with an RSA key.
            /// </summary>
            [DataMember(Name = "emv")]
            public bool? Emv { get; init; }

            /// <summary>
            /// PIN is preceded by 0x03 and the length of the PIN (0x04 to 0x0C), padding characters sequentially or randomly chosen, XORed with digits from PAN.
            /// </summary>
            [DataMember(Name = "iso3")]
            public bool? Iso3 { get; init; }

            /// <summary>
            /// PIN is formatted according to the Italian Bancomat specifications. It is known as the Authentication Parameter PIN block and is created with a 5 digit PIN, an 18 digit PAN, 
            /// and the 8 digit CCS from the track data.
            /// </summary>
            [DataMember(Name = "ap")]
            public bool? Ap { get; init; }

        }

        /// <summary>
        /// Supported PIN format.
        /// </summary>
        [DataMember(Name = "pinFormats")]
        public PinFormatsClass PinFormats { get; init; }

        [DataContract]
        public sealed class PresentationAlgorithmsClass
        {
            public PresentationAlgorithmsClass(bool? PresentClear = null)
            {
                this.PresentClear = PresentClear;
            }

            /// <summary>
            /// Algorithm for the presentation of a clear text PIN to a chipcard.
            /// Each digit of the clear text PIN is inserted as one nibble (=halfbyte) into ChipData.
            /// </summary>
            [DataMember(Name = "presentClear")]
            public bool? PresentClear { get; init; }

        }

        /// <summary>
        /// Supported presentation algorithms.
        /// </summary>
        [DataMember(Name = "presentationAlgorithms")]
        public PresentationAlgorithmsClass PresentationAlgorithms { get; init; }

        [DataContract]
        public sealed class DisplayClass
        {
            public DisplayClass(bool? None = null, bool? LedThrough = null, bool? Display = null)
            {
                this.None = None;
                this.LedThrough = LedThrough;
                this.Display = Display;
            }

            /// <summary>
            /// No display unit.
            /// </summary>
            [DataMember(Name = "none")]
            public bool? None { get; init; }

            /// <summary>
            /// Lights next to text guide user.
            /// </summary>
            [DataMember(Name = "ledThrough")]
            public bool? LedThrough { get; init; }

            /// <summary>
            /// A real display is available (this doesnâ€™t apply for self-service).
            /// </summary>
            [DataMember(Name = "display")]
            public bool? Display { get; init; }

        }

        /// <summary>
        /// Specifies the type of the display used in the PIN pad module.
        /// </summary>
        [DataMember(Name = "display")]
        public DisplayClass Display { get; init; }

        /// <summary>
        /// Specifies whether the PIN pad is directly physically connected to the ID card unit.
        /// If the value is true, the PIN will be transported securely during the command 
        /// [PinPad.PresentIdc](#pinpad.presentidc).
        /// </summary>
        [DataMember(Name = "idcConnect")]
        public bool? IdcConnect { get; init; }

        [DataContract]
        public sealed class ValidationAlgorithmsClass
        {
            public ValidationAlgorithmsClass(bool? Des = null, bool? Visa = null)
            {
                this.Des = Des;
                this.Visa = Visa;
            }

            /// <summary>
            /// DES algorithm.
            /// </summary>
            [DataMember(Name = "des")]
            public bool? Des { get; init; }

            /// <summary>
            /// Visa algorithm.
            /// </summary>
            [DataMember(Name = "visa")]
            public bool? Visa { get; init; }

        }

        /// <summary>
        /// Specifies the algorithms for PIN validation supported by the service.
        /// </summary>
        [DataMember(Name = "validationAlgorithms")]
        public ValidationAlgorithmsClass ValidationAlgorithms { get; init; }

        /// <summary>
        /// Specifies whether the device can retain the PIN after a PIN processing command.
        /// </summary>
        [DataMember(Name = "pinCanPersistAfterUse")]
        public bool? PinCanPersistAfterUse { get; init; }

        /// <summary>
        /// Specifies whether the keypad used in the secure PIN pad module is integrated within a 
        /// generic Win32 keyboard. true means the secure PIN keypad is integrated within a generic 
        /// Win32 keyboard and standard Win32 key events will be generated for any key when there is
        /// no active [Keyboard.GetData](#keyboard.dataentry) or [Keyboard.GetPin](#keyboard.pinentry)
        /// command.  Note that XFS continues to support defined PIN keys only, and is not extended 
        /// to support new alphanumeric keys.
        /// </summary>
        [DataMember(Name = "typeCombined")]
        public bool? TypeCombined { get; init; }

        /// <summary>
        /// Specifies whether the command [PinPad.SetPinblockData](#pinpad.setpinblockdata) must be 
        /// called before the PIN is entered via [Keyboard.GetPin](#keyboard.pinentry) and retrieved via 
        /// [PinPad.GetPinblock](#pinpad.getpinblock).
        /// </summary>
        [DataMember(Name = "setPinblockDataRequired")]
        public bool? SetPinblockDataRequired { get; init; }

        [DataContract]
        public sealed class PinBlockAttributesClass
        {
            public PinBlockAttributesClass(CryptoMethodClass CryptoMethod = null)
            {
                this.CryptoMethod = CryptoMethod;
            }

            [DataContract]
            public sealed class CryptoMethodClass
            {
                public CryptoMethodClass(bool? Ecb = null, bool? Cbc = null, bool? Cfb = null, bool? Ofb = null, bool? Ctr = null, bool? Xts = null, bool? RsaesPkcs1V15 = null, bool? RsaesOaep = null)
                {
                    this.Ecb = Ecb;
                    this.Cbc = Cbc;
                    this.Cfb = Cfb;
                    this.Ofb = Ofb;
                    this.Ctr = Ctr;
                    this.Xts = Xts;
                    this.RsaesPkcs1V15 = RsaesPkcs1V15;
                    this.RsaesOaep = RsaesOaep;
                }

                /// <summary>
                /// The ECB encryption method. 
                /// </summary>
                [DataMember(Name = "ecb")]
                public bool? Ecb { get; init; }

                /// <summary>
                /// The CBC encryption method. 
                /// </summary>
                [DataMember(Name = "cbc")]
                public bool? Cbc { get; init; }

                /// <summary>
                /// The CFB encryption method. 
                /// </summary>
                [DataMember(Name = "cfb")]
                public bool? Cfb { get; init; }

                /// <summary>
                /// The The OFB encryption method. 
                /// </summary>
                [DataMember(Name = "ofb")]
                public bool? Ofb { get; init; }

                /// <summary>
                /// The CTR method defined in NIST SP800-38A. 
                /// </summary>
                [DataMember(Name = "ctr")]
                public bool? Ctr { get; init; }

                /// <summary>
                /// The XTS method defined in NIST SP800-38E. 
                /// </summary>
                [DataMember(Name = "xts")]
                public bool? Xts { get; init; }

                /// <summary>
                /// The RSAES_PKCS1-v1.5 algorithm. 
                /// </summary>
                [DataMember(Name = "rsaesPkcs1V15")]
                public bool? RsaesPkcs1V15 { get; init; }

                /// <summary>
                /// The RSAES OAEP algorithm.
                /// </summary>
                [DataMember(Name = "rsaesOaep")]
                public bool? RsaesOaep { get; init; }

            }

            /// <summary>
            /// Specifies the cryptographic method supported. 
            /// If the algorithm is 'A', 'D', or 'T', then the following properties can be true." 
            /// 
            /// * ```ecb``` - The ECB encryption method. 
            /// * ```cbc``` - The CBC encryption method.  
            /// * ```cfb``` - The CFB encryption method.  
            /// * ```ofb``` - The OFB encryption method. 
            /// * ```ctr``` - The CTR method defined in NIST SP800-38A.  
            /// * ```xts``` - The XTS method defined in NIST SP800-38E. 
            /// 
            /// If the algorithm is 'R', then following properties can be true.  
            /// 
            /// * ```rsaesPkcs1V15``` - Use the RSAES_PKCS1-v1.5 algorithm. 
            /// * ```rsaesOaep``` - Use the RSAES OAEP algorithm.
            /// </summary>
            [DataMember(Name = "cryptoMethod")]
            public CryptoMethodClass CryptoMethod { get; init; }

        }

        /// <summary>
        /// Key-value pair of attributes supported by the [PinPad.GetPinblock](#pinpad.getpinblock) command
        /// to generate encrypted PIN block.
        /// </summary>
        [DataMember(Name = "pinBlockAttributes")]
        public Dictionary<string, Dictionary<string, Dictionary<string, PinBlockAttributesClass>>> PinBlockAttributes { get; init; }

    }


}

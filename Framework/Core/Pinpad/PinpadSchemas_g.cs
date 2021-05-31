/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Pinpad interface.
 * PinpadSchemas_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XFS4IoT.Pinpad
{

    [DataContract]
    public sealed class PinBlockAttributeClass
    {
        public PinBlockAttributeClass(KeyUsageEnum? KeyUsage = null, AlgorithmEnum? Algorithm = null, ModeOfUseEnum? ModeOfUse = null, CryptoMethodEnum? CryptoMethod = null)
        {
            this.KeyUsage = KeyUsage;
            this.Algorithm = Algorithm;
            this.ModeOfUse = ModeOfUse;
            this.CryptoMethod = CryptoMethod;
        }

        public enum KeyUsageEnum
        {
            P0
        }

        /// <summary>
        /// Specifies the key usages supported by the [Pinpad.PinBlock](#pinpad.getpinblock) command.
        /// The following values are possible:  
        /// 
        /// * ```P0``` - PIN Encryption
        /// </summary>
        [DataMember(Name = "keyUsage")]
        public KeyUsageEnum? KeyUsage { get; private set; }

        public enum AlgorithmEnum
        {
            A,
            D,
            R,
            T
        }

        /// <summary>
        /// Specifies the encryption algorithms supported by the [Pinpad.PinBlock](#pinpad.getpinblock) command.
        /// The following values are possible: 
        /// 
        /// * ```A``` - AES. 
        /// * ```D``` - DEA. 
        /// * ```R``` - RSA. 
        /// * ```T``` - Triple DEA (also referred to as TDEA).
        /// </summary>
        [DataMember(Name = "algorithm")]
        public AlgorithmEnum? Algorithm { get; private set; }

        public enum ModeOfUseEnum
        {
            E
        }

        /// <summary>
        /// Specifies the encryption modes supported by the [Pinpad.PinBlock](#pinpad.getpinblock) command.
        /// The following values are possible: 
        /// 
        /// * ```E``` - Encrypt
        /// </summary>
        [DataMember(Name = "modeOfUse")]
        public ModeOfUseEnum? ModeOfUse { get; private set; }

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
        /// This parameter specifies the cryptographic method that will be used with the encryption algorithm specified by algorithm.
        /// If algorithm is ‘A’, ‘D’, or ‘T’, then this property cryptoMethod can be one of the following values:\" 
        /// 
        /// * ```ecb``` - The ECB encryption method. 
        /// * ```cbc``` - The CBC encryption method. 
        /// * ```cfb``` - The CFB encryption method. 
        /// * ```ofb``` - The OFB encryption method. 
        /// * ```ctr``` - The CTR method defined in NIST SP800-38A. 
        /// * ```xts``` - The XTS method defined in NIST SP800-38E. 
        /// 
        /// If algorithm is ‘R’, then this property cryptoMethod can be one of the following values: 
        /// 
        /// * ```rsaesPkcs1V15``` - Use the RSAES_PKCS1-v1.5 algorithm. 
        /// * ```rsaesOaep``` - Use the RSAES OAEP algorithm.
        /// </summary>
        [DataMember(Name = "cryptoMethod")]
        public CryptoMethodEnum? CryptoMethod { get; private set; }

    }


    [DataContract]
    public sealed class CapabilitiesClass
    {
        public CapabilitiesClass(PinFormatsClass PinFormats = null, PresentationAlgorithmsClass PresentationAlgorithms = null, DisplayClass Display = null, bool? IdConnect = null, ValidationAlgorithmsClass ValidationAlgorithms = null, bool? PinCanPersistAfterUse = null, bool? TypeCombined = null, bool? SetPinblockDataRequired = null, List<PinBlockAttributeClass> PinBlockAttributes = null, CountrySpecificDKClass CountrySpecificDK = null, CountrySpecificChineseClass CountrySpecificChinese = null, CountrySpecificLuxemburgClass CountrySpecificLuxemburg = null)
        {
            this.PinFormats = PinFormats;
            this.PresentationAlgorithms = PresentationAlgorithms;
            this.Display = Display;
            this.IdConnect = IdConnect;
            this.ValidationAlgorithms = ValidationAlgorithms;
            this.PinCanPersistAfterUse = PinCanPersistAfterUse;
            this.TypeCombined = TypeCombined;
            this.SetPinblockDataRequired = SetPinblockDataRequired;
            this.PinBlockAttributes = PinBlockAttributes;
            this.CountrySpecificDK = CountrySpecificDK;
            this.CountrySpecificChinese = CountrySpecificChinese;
            this.CountrySpecificLuxemburg = CountrySpecificLuxemburg;
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
            public bool? Ibm3624 { get; private set; }

            /// <summary>
            /// PIN is preceded by 0x00 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F to the right, PIN length 4-12 digits, 
            /// XORed with PAN (Primary Account Number, minimum 12 digits without check number).
            /// </summary>
            [DataMember(Name = "ansi")]
            public bool? Ansi { get; private set; }

            /// <summary>
            /// PIN is preceded by 0x00 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F to the right, PIN length 4-12 digits,
            /// XORed with PAN (Primary Account Number without check number, no minimum length specified, missing digits are filled with 0x00).
            /// </summary>
            [DataMember(Name = "iso0")]
            public bool? Iso0 { get; private set; }

            /// <summary>
            /// PIN is preceded by 0x01 and the length of the PIN (0x04 to 0x0C), padding characters are taken from a transaction field (10 digits).
            /// </summary>
            [DataMember(Name = "iso1")]
            public bool? Iso1 { get; private set; }

            /// <summary>
            /// PIN left justified, filled with padding characters, PIN only 4 digits.
            /// </summary>
            [DataMember(Name = "eci2")]
            public bool? Eci2 { get; private set; }

            /// <summary>
            /// PIN is preceded by the length (digit), PIN length 4-6 digits, the padding character can range from 0x0 through 0xF".
            /// </summary>
            [DataMember(Name = "eci3")]
            public bool? Eci3 { get; private set; }

            /// <summary>
            /// PIN is preceded by the length (digit), PIN length 4-6 digits.
            /// If the PIN length is less than six digits the PIN is filled with 0x0 to the length of six, the padding character can range from 0x0 
            /// through 0x9 (This format is also referred to as VISA2).
            /// </summary>
            [DataMember(Name = "visa")]
            public bool? Visa { get; private set; }

            /// <summary>
            /// PIN is padded with the padding character and may be not encrypted, single encrypted or double encrypted.
            /// </summary>
            [DataMember(Name = "diebold")]
            public bool? Diebold { get; private set; }

            /// <summary>
            /// PIN with the length of 4 to 12 digits, each one with a value of 0x0 to 0x9, is preceded by the one-digit coordination number with 
            /// a value from 0x0 to 0xF, padded with the padding character with a value from 0x0 to 0xF and may be not encrypted, single encrypted or double encrypted.
            /// </summary>
            [DataMember(Name = "dieboldCo")]
            public bool? DieboldCo { get; private set; }

            /// <summary>
            /// PIN with the length of 4 to 12 digits, each one with a value of 0x0 to 0x9, is followed by a delimiter with the value of 0xF and then padded 
            /// by the padding character with a value between 0x0 to 0xF.
            /// </summary>
            [DataMember(Name = "visa3")]
            public bool? Visa3 { get; private set; }

            /// <summary>
            /// The PIN block is constructed as follows: PIN is preceded by 0x02 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F 
            /// to the right, formatted up to 248 bytes of other data as defined within the EMV 4.0 specifications and finally encrypted with an RSA key.
            /// </summary>
            [DataMember(Name = "emv")]
            public bool? Emv { get; private set; }

            /// <summary>
            /// PIN is preceded by 0x03 and the length of the PIN (0x04 to 0x0C), padding characters sequentially or randomly chosen, XORed with digits from PAN.
            /// </summary>
            [DataMember(Name = "iso3")]
            public bool? Iso3 { get; private set; }

            /// <summary>
            /// PIN is formatted according to the Italian Bancomat specifications. It is known as the Authentication Parameter PIN block and is created with a 5 digit PIN, an 18 digit PAN, 
            /// and the 8 digit CCS from the track data.
            /// </summary>
            [DataMember(Name = "ap")]
            public bool? Ap { get; private set; }

        }

        /// <summary>
        /// Supported PIN format.
        /// </summary>
        [DataMember(Name = "pinFormats")]
        public PinFormatsClass PinFormats { get; private set; }

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
            public bool? PresentClear { get; private set; }

        }

        /// <summary>
        /// Supported presentation algorithms.
        /// </summary>
        [DataMember(Name = "presentationAlgorithms")]
        public PresentationAlgorithmsClass PresentationAlgorithms { get; private set; }

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
            public bool? None { get; private set; }

            /// <summary>
            /// Lights next to text guide user.
            /// </summary>
            [DataMember(Name = "ledThrough")]
            public bool? LedThrough { get; private set; }

            /// <summary>
            /// A real display is available (this doesnâ€™t apply for self-service).
            /// </summary>
            [DataMember(Name = "display")]
            public bool? Display { get; private set; }

        }

        /// <summary>
        /// Specifies the type of the display used in the PIN pad module.
        /// </summary>
        [DataMember(Name = "display")]
        public DisplayClass Display { get; private set; }

        /// <summary>
        /// Specifies whether the PIN pad is directly physically connected to the ID card unit.
        /// If the value is true, the PIN will be transported securely during the command 
        /// [Pinpad.PresentIdc](#pinpad.presentidc).
        /// </summary>
        [DataMember(Name = "idConnect")]
        public bool? IdConnect { get; private set; }

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
            public bool? Des { get; private set; }

            /// <summary>
            /// Visa algorithm.
            /// </summary>
            [DataMember(Name = "visa")]
            public bool? Visa { get; private set; }

        }

        /// <summary>
        /// Specifies the algorithms for PIN validation supported by the service.
        /// </summary>
        [DataMember(Name = "validationAlgorithms")]
        public ValidationAlgorithmsClass ValidationAlgorithms { get; private set; }

        /// <summary>
        /// Specifies whether the device can retain the PIN after a PIN processing command.
        /// </summary>
        [DataMember(Name = "pinCanPersistAfterUse")]
        public bool? PinCanPersistAfterUse { get; private set; }

        /// <summary>
        /// Specifies whether the keypad used in the secure PIN pad module is integrated within a 
        /// generic Win32 keyboard. true means the secure PIN keypad is integrated within a generic 
        /// Win32 keyboard and standard Win32 key events will be generated for any key when there is
        /// no active [Keyboard.GetData](#keyboard.dataentry) or [Keyboard.GetPin](#keyboard.pinentry)
        /// command.  Note that XFS continues to support defined PIN keys only, and is not extended 
        /// to support new alphanumeric keys.
        /// </summary>
        [DataMember(Name = "typeCombined")]
        public bool? TypeCombined { get; private set; }

        /// <summary>
        /// Specifies whether the command [Pinpad.SetPinblockData](#pinpad.setpinblockdata) must be 
        /// called before the PIN is entered via [Keyboard.GetPin](#keyboard.pinentry) and retrieved via 
        /// [Pinpad.GetPinblock](#pinpad.getpinblock).
        /// </summary>
        [DataMember(Name = "setPinblockDataRequired")]
        public bool? SetPinblockDataRequired { get; private set; }

        /// <summary>
        /// Array of attributes supported by the [Pinpad.GetPinblock](#pinpad.getpinblock) command.
        /// </summary>
        [DataMember(Name = "pinBlockAttributes")]
        public List<PinBlockAttributeClass> PinBlockAttributes { get; private set; }

        [DataContract]
        public sealed class CountrySpecificDKClass
        {
            public CountrySpecificDKClass(bool? ProtocolSupported = null, string HsmVendor = null, bool? HsmJournaling = null, DerivationAlgorithmsClass DerivationAlgorithms = null)
            {
                this.ProtocolSupported = ProtocolSupported;
                this.HsmVendor = HsmVendor;
                this.HsmJournaling = HsmJournaling;
                this.DerivationAlgorithms = DerivationAlgorithms;
            }

            /// <summary>
            /// Specifies whether the device supports the DK (Deutsche Kreditwirtschaft) formerly known as 
            /// the ZKA (Zentraler Kreditausschuß) protocol or not.
            /// </summary>
            [DataMember(Name = "protocolSupported")]
            public bool? ProtocolSupported { get; private set; }

            /// <summary>
            /// Identifies the hsm Vendor. hsmVendor is an empty string or this field is not set when the 
            /// hsm Vendor is unknown or the HSM is not supported.
            /// </summary>
            [DataMember(Name = "hsmVendor")]
            public string HsmVendor { get; private set; }

            /// <summary>
            /// Specifies whether the hsm supports journaling by the [Pinpad.DK.GetJournal](#command-Pinpad.dk.getjournal) command.
            /// The value of this parameter is either TRUE or FALSE. TRUE means the hsm supports journaling by 
            /// [Pinpad.DK.GetJournal](#command-Pinpad.dk.getjournal).
            /// </summary>
            [DataMember(Name = "hsmJournaling")]
            public bool? HsmJournaling { get; private set; }

            [DataContract]
            public sealed class DerivationAlgorithmsClass
            {
                public DerivationAlgorithmsClass(bool? ChipZka = null)
                {
                    this.ChipZka = ChipZka;
                }

                /// <summary>
                /// Algorithm for the derivation of a chip card individual key as described by the German ZKA.
                /// </summary>
                [DataMember(Name = "chipZka")]
                public bool? ChipZka { get; private set; }

            }

            /// <summary>
            /// Supported derivation algorithms.
            /// </summary>
            [DataMember(Name = "derivationAlgorithms")]
            public DerivationAlgorithmsClass DerivationAlgorithms { get; private set; }

        }

        /// <summary>
        /// Specified capabilites of German specific protocol supports.
        /// </summary>
        [DataMember(Name = "countrySpecificDK")]
        public CountrySpecificDKClass CountrySpecificDK { get; private set; }

        [DataContract]
        public sealed class CountrySpecificChineseClass
        {
            public CountrySpecificChineseClass(bool? ProtocolSupported = null)
            {
                this.ProtocolSupported = ProtocolSupported;
            }

            /// <summary>
            /// Specifies whether device supports the protocol for China commands or not. 
            /// The reference for this specific protocol are the Financial industry standard of the People’s 
            /// Republic of China PBOC3.0 JR/T 0025 and the Password industry standard of the People's Republic of China GM/T 0003, GM/T 004.
            /// </summary>
            [DataMember(Name = "protocolSupported")]
            public bool? ProtocolSupported { get; private set; }

        }

        /// <summary>
        /// Specified capabilites of Chinese specific PBOC3.0 protocol supports.
        /// </summary>
        [DataMember(Name = "countrySpecificChinese")]
        public CountrySpecificChineseClass CountrySpecificChinese { get; private set; }

        [DataContract]
        public sealed class CountrySpecificLuxemburgClass
        {
            public CountrySpecificLuxemburgClass(bool? ProtocolSupported = null)
            {
                this.ProtocolSupported = ProtocolSupported;
            }

            /// <summary>
            /// Specifies whether the device supports Protocol for Luxemburg commands or not. 
            /// The reference for this specific protocol is the Authorization Center in Luxemburg.
            /// </summary>
            [DataMember(Name = "protocolSupported")]
            public bool? ProtocolSupported { get; private set; }

        }

        /// <summary>
        /// Specified capabilites of Luxemburg specific protocol supports.
        /// </summary>
        [DataMember(Name = "countrySpecificLuxemburg")]
        public CountrySpecificLuxemburgClass CountrySpecificLuxemburg { get; private set; }

    }


}

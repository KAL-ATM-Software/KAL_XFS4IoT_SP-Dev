/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.PinPad.Commands;
using XFS4IoT.PinPad.Completions;
using XFS4IoTFramework.Common;
using XFS4IoT;

namespace XFS4IoTFramework.PinPad
{
    public sealed class VerifyPINLocalDESRequest
    {
        public VerifyPINLocalDESRequest(string ValidationData,
                                        string Offset,
                                        byte Padding,
                                        int MaxPIN,
                                        int ValDigits,
                                        bool NoLeadingZero,
                                        string KeyName,
                                        string KeyEncKeyName,
                                        string DecTable)
        {
            this.ValidationData = ValidationData;
            this.Offset = Offset;
            this.Padding = Padding;
            this.MaxPIN = MaxPIN;
            this.ValDigits = ValDigits;
            this.NoLeadingZero = NoLeadingZero;
            this.KeyName = KeyName;
            this.KeyEncKeyName = KeyEncKeyName;
            this.DecTable = DecTable;
        }

        /// <summary>
        /// Customer specific data (normally obtained from card track data) used to validate the correctness of the PIN. 
        /// The validation data should be an ASCII string.
        /// </summary>
        public string ValidationData { get; init; }

        /// <summary>
        /// ASCII string defining the offset data for the PIN block as an ASCII string.
        /// if this property is omitted then no offset is used.
        /// The character must be in the ranges ‘0’ to ‘9’, ‘a’ to ‘f’ and ‘A’ to ‘F’
        /// </summary>
        public string Offset { get; init; }

        /// <summary>
        /// Specifies the padding character for the validation data. 
        /// If the validation data is less than 16 characters long then it will be padded with this character. 
        /// If padding is in the range 00 to 0F in 16 character string, padding is applied after the validation data has been compressed. 
        /// The valid range is 0 to 15.
        /// </summary>
        public byte Padding { get; init; }

        /// <summary>
        /// Maximum number of PIN digits to be used for validation.
        /// This parameter corresponds to PINMINL in the IBM 3624 specification.
        /// </summary>
        public int MaxPIN { get; init; }

        /// <summary>
        /// Number of Validation digits from the validation data to be used for validation.
        /// This is the length of the *validationData*.
        /// </summary>
        public int ValDigits { get; init; }

        /// <summary>
        /// If set to TRUE and the first digit of result of the modulo 10 addition is a 0x0, it is replaced with 0x1 before performing the 
        /// verification against the entered PIN. If set to FALSE, a leading zero is allowed in entered PINs.
        /// </summary>
        public bool NoLeadingZero { get; init; }

        /// <summary>
        /// Name of the key to be used for validation. 
        /// The key referenced by key must have the 'V0' attribute.
        /// </summary>
        public string KeyName { get; init; }

        /// <summary>
        /// If this property is null or empty string, key is used directly for PIN validation.
        /// Otherwise, key is used to decrypt the encrypted key used for PIN validation.
        /// </summary>
        public string KeyEncKeyName { get; init; }

        /// <summary>
        /// ASCII decimalization table (16 character string containing characters '0' to '9'). 
        /// This table is used to convert the hexadecimal digits (0x0 to 0xF) of the encrypted validation data to decimal digits (0x0 to 0x9).
        /// </summary>
        public string DecTable { get; init; }

    }

    public sealed class VerifyPINLocalResult : DeviceResult
    {
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

        public VerifyPINLocalResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                    string ErrorDescription = null,
                                    ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Verified = false;
        }

        public VerifyPINLocalResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                      bool Verified)
                : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Verified = Verified;
        }

        public ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// PIN locally verified successfully
        /// </summary>
        public bool Verified { get; init; }
    }

    public sealed class VerifyPINLocalVISARequest
    {
        public VerifyPINLocalVISARequest(string CustomerData,
                                         string PINValidationValue,
                                         string KeyName,
                                         string KeyEncKeyName)
        {
            this.CustomerData = CustomerData;
            this.PINValidationValue = PINValidationValue;
            this.KeyName = KeyName;
            this.KeyEncKeyName = KeyEncKeyName;
        }

        /// <summary>
            /// Primary Account Number from track data, as an ASCII string. 
            /// PAN should contain the eleven rightmost digits of the PAN (excluding the check digit ), 
            /// followed by the pvki indicator in the 12th byte.
            /// </summary>
            public string CustomerData { get; init; }

            /// <summary>
            /// PIN Validation Value from track data. The valid range is '0' to '9'. 
            /// This string should contain 4 digits. 
            /// </summary>
            public string PINValidationValue { get; init; }

            /// <summary>
            /// Name of the validation key. The key referenced by key must have the 'V2' attribute.
            /// </summary>
            public string KeyName { get; init; }

            /// <summary>
            /// If this property is omitted, key is used directly for PIN validation. Otherwise, key is used to decrypt the 
            /// encrypted key passed in *keyEncKey* and the result is used for PIN validation. 
            /// </summary>
            public string KeyEncKeyName { get; init; }

    }

    public sealed class PINBlockRequest
    {
        public enum PINFormatEnum
        {
            IBM3624,
            ANSI,
            ISO0,
            ISO1,
            ECI2,
            ECI3,
            VISA,
            DIEBOLD,
            DIEBOLDCO,
            VISA3,
            BANKSYS,
            EMV,
            ISO3,
            AP
        }

        public enum EncryptionAlgorithmEnum
        {
            ECB,
            CBC,
            CFB,
            OFB,
            CTR,
            XTS,
            RSAES_PKCS1_V1_5,
            RSAES_OAEP
        }

        public PINBlockRequest(string CustomerData,
                               string XorData,
                               byte Padding,
                               PINFormatEnum Format,
                               string KeyName,
                               string SecondEncKeyName,
                               EncryptionAlgorithmEnum EncryptionAlgorithm)
        {
            this.CustomerData = CustomerData;
            this.XorData = XorData;
            this.Padding = Padding;
            this.Format = Format;
            this.KeyName = KeyName;
            this.SecondEncKeyName = SecondEncKeyName;
            this.EncryptionAlgorithm = EncryptionAlgorithm;
        }

        /// <summary>
        /// The customer data should be an ASCII string. Used for ANSI, ISO-0 and ISO-1 algorithm to build the formatted PIN. 
        /// For ANSI and ISO-0 the PAN (Primary Account Number, without the check number) is supplied, for ISO-1 a ten digit 
        /// transaction field is required. If not used, this property can be omitted. Used for DIEBOLD with coordination number, as a 
        /// two digit coordination number. Used for EMV with challenge number (8 bytes) coming from the chip card. 
        /// This number is passed as unpacked string, for example: 0123456789ABCDEF = 0x30 0x31 0x32 0x33 0x34 0x35 0x36 0x37 
        /// 0x38 0x39 0x41 0x42 0x43 0x44 0x45 0x46 For AP PIN blocks, the data must be a concatenation of the PAN (18 digits 
        /// including the check digit), and the CCS (8 digits).
        /// </summary>
        public string CustomerData { get; init; }

        /// <summary>
        /// If the formatted PIN is encrypted twice to build the resulting PIN block, this data can be used to modify the result 
        /// of the first encryption by an XOR-operation. This parameter is a string of hexadecimal data that must be converted by 
        /// the application, e.g. 0x0123456789ABCDEF must be converted to 0x30 0x31 0x32 0x33 0x34 0x35 0x36 0x37 0x38 0x39 0x41 
        /// 0x42 0x43 0x44 0x45 0x46 and terminated with 0x00. In other words the application would set xorData to “0123456789ABCDEF”. 
        /// The hex digits 0xA to 0xF can be represented by characters in the ranges ‘a’ to ‘f’ or ‘A’ to ‘F’. If this value is omitted 
        /// no XOR-operation will be performed. If the formatted PIN is not encrypted twice, this value is ignored.
        /// </summary>
        public string XorData { get; init; }

        /// <summary>
        /// Specifies the padding character. The valid range is 0 to 15. 
        /// Only the least significant nibble is used. This property is ignored for PIN block formats with fixed, sequential or random padding.
        /// </summary>
        public byte Padding { get; init; }

        /// <summary>
        /// Specifies the format of the PIN block.
        /// * ```IBM3624``` - PIN left justified, filled with padding characters, PIN length 4-16 digits. The padding character is a hexadecimal digit 
        /// in the range 0x00 to 0x0F."
        /// * ```ANSI``` - PIN is preceded by 0x00 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F to the right, PIN length 
        /// 4-12 digits, XORed with PAN (Primary Account Number, minimum 12 digits without check number).  
        /// * ```ISO0``` - PIN is preceded by 0x00 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F to the right, PIN length 
        /// 4-12 digits, XORed with PAN (Primary Account Number without check number, no minimum length specified, missing digits are filled with 0x00).  
        /// * ```ISO1``` - PIN is preceded by 0x01 and the length of the PIN (0x04 to 0x0C), padding characters are taken from a transaction field (10 digits).  
        /// * ```ECI2``` - PIN left justified, filled with padding characters, PIN only 4 digits. 
        /// * ```ECI3``` - PIN is preceded by the length (digit), PIN length 4-6 digits, the padding character can range from 0x0 through 0xF. 
        /// * ```VISA``` - PIN is preceded by the length (digit), PIN length 4-6 digits. If the PIN length is less than six digits the PIN is filled with 0x0 
        /// to the length of six, the padding character can range from 0x0 through 0x9 (This format is also referred to as VISA2).  
        /// * ```DIEBOLD``` - PIN is padded with the padding character and may be not encrypted, single encrypted or double encrypted. 
        /// * ```DIEBOLDCO``` - PIN with the length of 4 to 12 digits, each one with a value of 0x0 to 0x9, is preceded by the one-digit coordination number 
        /// with a value from 0x0 to 0xF, padded with the padding character with a value from 0x0 to 0xF and may be not encrypted, single encrypted or double encrypted.  
        /// * ```VISA3``` - PIN with the length of 4 to 12 digits, each one with a value of 0x0 to 0x9, is followed by a delimiter with the value of 0xF and then padded 
        /// by the padding character with a value between 0x0 to 0xF.  
        /// * ```BANKSYS``` - PIN is encrypted and formatted according to the Banksys PIN block specifications.
        /// * ```EMV``` - The PIN block is constructed as follows: PIN is preceded by 0x02 and the length of the PIN (0x04 to 0x0C), filled with padding character 0x0F 
        /// to the right, formatted up to 248 bytes of other data as defined within the EMV 4.0 specifications and finally encrypted with an RSA key.  
        /// * ```ISO3``` - PIN is preceded by 0x03 and the length of the PIN (0x04 to 0x0C), padding characters sequentially or randomly chosen, XORed with digits from PAN. 
        /// * ```AP``` - PIN is formatted according to the Italian Bancomat specifications. It is known as the Authentication Parameter PIN block and is created with a
        /// 5 digit PIN, an 18 digit PAN, and the 8 digit CCS from the track data. 
        /// </summary>
        public PINFormatEnum Format { get; init; }

        /// <summary>
        /// Specifies the key used to encrypt the formatted PIN for the first time, this property is not required if no encryption is required. 
        /// If this specifies a double-length or triple-length key, triple DES encryption will be performed. 
        /// If this specifies an RSA key, RSA encryption will be performed
        /// </summary>
        public string KeyName { get; init; }

        /// <summary>
        /// Specifies the _key_ used to format the once encrypted formatted PIN, this property can be omitted if no second encryption required. 
        /// The key referenced by _secondEncKey_ must have the keyUsage 'P0' attribute. 
        /// If this specifies a double-length or triple-length key, triple DES encryption will be performed.
        /// </summary>
        public string SecondEncKeyName { get; init; }

       
        /// <summary>
        /// This parameter specifies the cryptographic method [cryptomethod](#common.capabilities.completion.properties.pinpad.pinblockattributes.p0.t.e.cryptomethod) that will be used with the encryption algorithm.
        /// If the algorithm is ['A', 'D', or 'T'](#common.capabilities.completion.properties.pinpad.pinblockattributes.p0.t), then this property can be one of the following values:" 
        /// 
        /// * ```ECB``` - The ECB encryption method. 
        /// * ```CBC``` - The CBC encryption method.  
        /// * ```CFB``` - The CFB encryption method.  
        /// * ```OFB``` - The OFB encryption method. 
        /// * ```CTR``` - The CTR method defined in NIST SP800-38A.  
        /// * ```XTS``` - The XTS method defined in NIST SP800-38E. 
        ///  
        /// If the algorithm is ['R'](#common.capabilities.completion.properties.pinpad.pinblockattributes.p0.t), then this property can be one of the following values:  
        /// 
        /// * ```RSAES_PKCS1-V1.5``` - Use the RSAES_PKCS1-v1.5 algorithm. 
        /// * ```RSAES_OAEP``` - Use the RSAES OAEP algorithm.
        /// </summary>
        public EncryptionAlgorithmEnum EncryptionAlgorithm { get; init; }
    }

    public sealed class PINBlockResult : DeviceResult
    {
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

        public PINBlockResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              GetPinBlockCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.PINBlock = null;
        }

        public PINBlockResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              List<byte> PINBlock)
                : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.PINBlock = PINBlock;
        }

        public GetPinBlockCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Encrypted PIN block
        /// </summary>
        public List<byte> PINBlock { get; init; }
    }

    public sealed class PresentIDCRequest
    {
        public PresentIDCRequest(string ChipProtocol,
                                 List<byte> ChipData,
                                 PresentClearClass PresentClear)
        {
            this.ChipProtocol = ChipProtocol;
            this.ChipData = ChipData;
            this.PresentClear = PresentClear;
        }
        /// <summary>
        /// Identifies the protocol that is used to communicate with the chip. Possible values are: 
        /// (see command [chipProtocols](#common.capabilities.completion.properties.cardreader.chipProtocols) in the Identification Card Device Class Interface)
        /// </summary>
        public string ChipProtocol { get; init; }

        /// <summary>
        /// The Base64 encoded data to be sent to the chip.
        /// </summary>
        public List<byte> ChipData { get; init; }

        public sealed class PresentClearClass
        {
            public PresentClearClass(int PinPointer, int PinOffset)
            {
                this.PinPointer = PinPointer;
                this.PinOffset = PinOffset;
            }

            /// <summary>
            /// The byte offset where to start inserting the PIN into chipData. 
            /// The leftmost byte is numbered zero. See below for an example
            /// </summary>
            public int PinPointer { get; init; }

            /// <summary>
            /// The bit offset within the byte specified by *pinPointer* property where to start inserting the PIN. 
            /// The leftmost bit numbered zero.
            /// </summary>
            public int PinOffset { get; init; }

        }

        /// <summary>
        /// Contains the data required
        /// </summary>
        public PresentClearClass PresentClear { get; init; }
    }

    public sealed class PresentIDCResult : DeviceResult
    {
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

        public PresentIDCResult(MessagePayload.CompletionCodeEnum CompletionCode,
                              string ErrorDescription = null,
                              PresentIDCCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.ChipProtocol = string.Empty;
            this.ChipData = null;
        }

        public PresentIDCResult(MessagePayload.CompletionCodeEnum CompletionCode,
                                string ChipProtocol,
                                List<byte> ChipData)
                : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.ChipProtocol = ChipProtocol;
            this.ChipData = ChipData;
        }

        public PresentIDCCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Identifies the protocol that was used to communicate with the chip. 
        /// This property contains the same value as the corresponding property in the input.
        /// </summary>
        public string ChipProtocol { get; init; }

        /// <summary>
        /// The Base64 encoded data responded from the chip.
        /// </summary>
        public List<byte> ChipData { get; init; }
    }
}
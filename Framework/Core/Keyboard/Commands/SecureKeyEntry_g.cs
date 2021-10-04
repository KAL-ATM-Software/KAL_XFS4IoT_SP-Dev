/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
 * This file was created automatically as part of the XFS4IoT Keyboard interface.
 * SecureKeyEntry_g.cs uses automatically generated parts.
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using XFS4IoT.Commands;

namespace XFS4IoT.Keyboard.Commands
{
    //Original name = SecureKeyEntry
    [DataContract]
    [Command(Name = "Keyboard.SecureKeyEntry")]
    public sealed class SecureKeyEntryCommand : Command<SecureKeyEntryCommand.PayloadData>
    {
        public SecureKeyEntryCommand(int RequestId, SecureKeyEntryCommand.PayloadData Payload)
            : base(RequestId, Payload)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(int Timeout, KeyLenEnum? KeyLen = null, bool? AutoEnd = null, Dictionary<string, KeyClass> ActiveKeys = null, VerificationTypeEnum? VerificationType = null, CryptoMethodEnum? CryptoMethod = null)
                : base(Timeout)
            {
                this.KeyLen = KeyLen;
                this.AutoEnd = AutoEnd;
                this.ActiveKeys = ActiveKeys;
                this.VerificationType = VerificationType;
                this.CryptoMethod = CryptoMethod;
            }

            public enum KeyLenEnum
            {
                Number16,
                Number32,
                Number48
            }

            /// <summary>
            /// Specifies the number of digits which must be entered for the encryption key, 16 for a singlelength key, 
            /// 32 for a double-length key and 48 for a triple-length key.
            /// The only valid values are 16, 32 and 48.
            /// </summary>
            [DataMember(Name = "keyLen")]
            public KeyLenEnum? KeyLen { get; init; }

            /// <summary>
            /// If autoEnd is set to true, the Service Provider terminates the command when the maximum number of encryption 
            /// key digits are entered. Otherwise, the input is terminated by the user using Enter, Cancel or any terminating key. 
            /// When keyLen is reached, the Service Provider will disable all keys associated with an encryption key digit.
            /// </summary>
            [DataMember(Name = "autoEnd")]
            public bool? AutoEnd { get; init; }

            /// <summary>
            /// Specifies all Function Keys which are active during the execution of the command.
            /// This should be the complete set or a subset of the keys returned in the payload of the 
            /// [Keyboard.GetLayout](#keyboard.getlayout) command.
            /// This should include 'zero' to 'nine' and 'a' to 'f' for all modes of secure key entry, 
            /// but should also include 'shift' on shift based systems. 
            /// The 'doubleZero', 'tripleZero' and 'decPoint' function keys must not be included in the list of active or terminate keys.
            /// 
            /// For FDKs which must terminate the execution of the command. This should include the FDKs associated with Cancel and Enter.
            /// 
            /// The following standard names are defined:
            /// 
            /// * ```zero``` - Numeric digit 0
            /// * ```one``` - Numeric digit 1
            /// * ```two``` - Numeric digit 2
            /// * ```three``` - Numeric digit 3
            /// * ```four``` - Numeric digit 4
            /// * ```five``` - Numeric digit 5
            /// * ```six``` - Numeric digit 6
            /// * ```seven``` - Numeric digit 7
            /// * ```eight``` - Numeric digit 8
            /// * ```nine``` - Numeric digit 9
            /// * ```[a-f]``` - Hex digit A to F for secure key entry
            /// * ```enter``` - Enter
            /// * ```cancel``` - Cancel
            /// * ```clear``` - Clear
            /// * ```backspace``` - Backspace
            /// * ```help``` - Help
            /// * ```decPoint``` - Decimal point
            /// * ```shift``` - Shift key used during hex entry
            /// * ```doubleZero``` - 00
            /// * ```tripleZero``` - 000
            /// * ```fdk[01-32]``` - 32 FDK keys
            /// 
            /// Additional non standard key names are also allowed for terminating the execution of the command.
            /// </summary>
            [DataMember(Name = "activeKeys")]
            public Dictionary<string, KeyClass> ActiveKeys { get; init; }

            public enum VerificationTypeEnum
            {
                Self,
                Zero
            }

            /// <summary>
            /// Specifies the type of verification to be done on the entered key.
            /// The following values are possible:
            /// 
            /// * ```self``` - The key check value is created by an encryption of the key with itself. 
            ///                For a double-length or triple-length key the kcv is generated using 3DES encryption using the first 8 bytes of the key as the source data for the encryption.
            /// * ```zero``` - The key check value is created by encrypting a zero value with the key.
            /// </summary>
            [DataMember(Name = "verificationType")]
            public VerificationTypeEnum? VerificationType { get; init; }

            public enum CryptoMethodEnum
            {
                Des,
                TripleDes,
                Aes
            }

            /// <summary>
            /// Specifies the cryptographic method to be used for the verification.
            /// If this property is omitted, *keyLen* will determine the cryptographic method used. 
            /// If *keyLen* is 16, the cryptographic method will be Single DES. 
            /// If *keyLen* is 32 or 48, the cryptographic method will be Triple DES
            /// The following values are possible:
            /// 
            /// * ```des``` - Single DES     
            /// * ```tripleDes``` - Triple DES
            /// * ```aes``` - AES
            /// </summary>
            [DataMember(Name = "cryptoMethod")]
            public CryptoMethodEnum? CryptoMethod { get; init; }

        }
    }
}

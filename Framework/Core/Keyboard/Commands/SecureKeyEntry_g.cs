/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
    [XFS4Version(Version = "2.1")]
    [Command(Name = "Keyboard.SecureKeyEntry")]
    public sealed class SecureKeyEntryCommand : Command<SecureKeyEntryCommand.PayloadData>
    {
        public SecureKeyEntryCommand(int RequestId, SecureKeyEntryCommand.PayloadData Payload, int Timeout)
            : base(RequestId, Payload, Timeout)
        { }

        [DataContract]
        public sealed class PayloadData : MessagePayload
        {

            public PayloadData(KeyLenEnum? KeyLen = null, bool? AutoEnd = null, Dictionary<string, ActiveKeysClass> ActiveKeys = null, VerificationTypeEnum? VerificationType = null, CryptoMethodEnum? CryptoMethod = null)
                : base()
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
                Number48,
                Number64
            }

            /// <summary>
            /// Specifies the number of digits which must be entered for the encryption key as one of the following:
            /// 
            /// * ```16``` - For a single-length DES key.
            /// * ```32``` - For a double-length TDES or AES 128 key.
            /// * ```48``` - For a triple-length TDES or AES 192 key.
            /// * ```64``` - For an AES 256 key.
            /// <example>32</example>
            /// </summary>
            [DataMember(Name = "keyLen")]
            public KeyLenEnum? KeyLen { get; init; }

            /// <summary>
            /// If *autoEnd* is set to true, the Service Provider terminates the command when the maximum number of
            /// encryption key digits are entered. Otherwise, the input is terminated by the user using Enter, Cancel or
            /// any terminating key.
            /// When *keyLen* is reached, the Service Provider will disable all keys associated with an encryption key
            /// digit.
            /// </summary>
            [DataMember(Name = "autoEnd")]
            public bool? AutoEnd { get; init; }

            [DataContract]
            public sealed class ActiveKeysClass
            {
                public ActiveKeysClass(bool? Terminate = null)
                {
                    this.Terminate = Terminate;
                }

                /// <summary>
                /// The key is a terminate key.
                /// </summary>
                [DataMember(Name = "terminate")]
                public bool? Terminate { get; init; }

            }

            /// <summary>
            /// Specifies all Function Keys which are active during the execution of the command.
            /// This should be the complete set or a subset of the keys returned in the payload of the
            /// [Keyboard.GetLayout](#keyboard.getlayout) command.
            /// This should include 'zero' to 'nine' and 'a' to 'f' for all modes of secure key entry, but should also
            /// include 'shift' on shift based systems.
            /// The 'doubleZero', 'tripleZero' and 'decPoint' function keys must not be included in the list of active
            /// or terminate keys.
            /// 
            /// For FDKs which must terminate the execution of the command. This should include the FDKs associated with
            /// Cancel and Enter.
            /// </summary>
            [DataMember(Name = "activeKeys")]
            public Dictionary<string, ActiveKeysClass> ActiveKeys { get; init; }

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
            ///                 For a double-length or triple-length key the KCV is generated using 3DES encryption using
            ///                 the first 8 bytes of the key as the source data for the encryption.
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
            /// If this property is null, *keyLen* will determine the cryptographic method used.
            /// If *keyLen* is 16, the cryptographic method will be Single DES.
            /// If *keyLen* is 32 or 48, the cryptographic method will be Triple DES.
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

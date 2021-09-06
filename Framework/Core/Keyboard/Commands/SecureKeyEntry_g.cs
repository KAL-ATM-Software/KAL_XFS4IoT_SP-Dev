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

            public PayloadData(int Timeout, KeyLenEnum? KeyLen = null, bool? AutoEnd = null, string ActiveFDKs = null, string ActiveKeys = null, string TerminateFDKs = null, string TrerminateKeys = null, VerificationTypeEnum? VerificationType = null)
                : base(Timeout)
            {
                this.KeyLen = KeyLen;
                this.AutoEnd = AutoEnd;
                this.ActiveFDKs = ActiveFDKs;
                this.ActiveKeys = ActiveKeys;
                this.TerminateFDKs = TerminateFDKs;
                this.TrerminateKeys = TrerminateKeys;
                this.VerificationType = VerificationType;
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
            /// Specifies those FDKs which are active during the execution of the command.
            /// This parameter should include those FDKs mapped to edit functions.
            /// </summary>
            [DataMember(Name = "activeFDKs")]
            [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
            public string ActiveFDKs { get; init; }

            /// <summary>
            /// Specifies all Function Keys(not FDKs) which are active during the execution of the command.
            /// This should be the complete set or a subset of the keys returned in the payload of the 
            /// [Keyboard.GetLayout](#keyboard.getlayout) command.
            /// </summary>
            [DataMember(Name = "activeKeys")]
            [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
            public string ActiveKeys { get; init; }

            /// <summary>
            /// Specifies those FDKs which must terminate the execution of the command.
            /// This should include the FDKs associated with Cancel and Enter.
            /// </summary>
            [DataMember(Name = "terminateFDKs")]
            [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
            public string TerminateFDKs { get; init; }

            /// <summary>
            /// Specifies those all Function Keys (not FDKs) which must terminate the execution of the command.
            /// This does not include the FDKs associated with Enter or Cancel.
            /// </summary>
            [DataMember(Name = "trerminateKeys")]
            [DataTypes(Pattern = "^(one|two|three|four|five|six|seven|eight|nine|[a-f]|enter|cancel|clear|backspace|help|decPoint|shift|res0[1-8]|oem0[1-6]|doubleZero|tripleZero)$|^fdk(0[1-9]|[12][0-9]|3[0-2])$")]
            public string TrerminateKeys { get; init; }

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

        }
    }
}

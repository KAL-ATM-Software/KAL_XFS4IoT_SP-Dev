﻿/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
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
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using XFS4IoT.Keyboard;
using XFS4IoTFramework.Common;
using XFS4IoT;

namespace XFS4IoTFramework.Keyboard
{
    [Flags]
    public enum KeyboardBeepEnum
    {
        Off = 0,
        Active = 1 << 0,
        InActive = 1 << 1,
    }

    public enum EntryCompletionEnum
    {
        Auto,
        Enter,
        Cancel,
        Continue,
        Clear,
        Backspace,
        FDK,
        Help,
        FK,
        ContinueFDK
    }

    public class ActiveKeyClass(
        string KeyName,
        bool Terminate = false)
    {
        public string KeyName { get; init; } = KeyName;
        public bool Terminate { get; init; } = Terminate;
    }

    public sealed class DefineLayoutResult : DeviceResult
    {
        public DefineLayoutResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            DefineLayoutCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
        }

        public DefineLayoutResult(MessageHeader.CompletionCodeEnum CompletionCode)
                : base(CompletionCode, null)
        {
            this.ErrorCode = null;
        }

        public DefineLayoutCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }
    }

    public sealed class DataEntryRequest(
        int MaxLen,
        bool AutoEnd,
        List<ActiveKeyClass> ActiveKeys)
    {

        /// <summary>
        /// Specifies the maximum number of digits which can be returned to the application in the output parameter. 
        /// </summary>
        public int MaxLen { get; init; } = MaxLen;

        /// <summary>
        /// If autoEnd is set to true, the Service Provider terminates the command when the maximum number of digits are entered.
        /// Otherwise, the input is terminated by the user using one of the termination keys. 
        /// autoEnd is ignored when maxLen is set to zero.
        /// </summary>
        public bool AutoEnd { get; init; } = AutoEnd;

        /// <summary>
        /// Specifies Function Keys which are active during the execution of the command.
        /// </summary>
        public List<ActiveKeyClass> ActiveKeys { get; init; } = ActiveKeys;
    }

    public sealed class DataEntryResult : DeviceResult
    {
        public sealed class EnteredKey(
            string Key,
            EntryCompletionEnum? Completion = null)
        {

            /// <summary>
            /// Key name pressed
            /// </summary>
            public string Key { get; init; } = Key;

            /// <summary>
            /// Completion of the entry
            /// </summary>
            public EntryCompletionEnum? Completion { get; init; } = Completion;
        }

        public DataEntryResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            DataEntryCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Keys = 0;
            EnteredKeys = null;
            Completion = null;
        }

        public DataEntryResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            int Keys,
            List<EnteredKey> EnteredKeys,
            EntryCompletionEnum? Completion)
                : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Keys = Keys;
            this.EnteredKeys = EnteredKeys;
            this.Completion = Completion;
        }
        public DataEntryCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Number of keys entered by the user
        /// </summary>
        public int Keys { get; init; }

        /// <summary>
        /// Array contains the keys entered by the user 
        /// </summary>
        public List<EnteredKey> EnteredKeys { get; init; }

        /// <summary>
        /// Specifies the reason for completion of the entry.
        /// </summary>
        public EntryCompletionEnum? Completion { get; init; }
    }

    public sealed class PinEntryRequest(
        int MinLen,
        int MaxLen,
        bool AutoEnd,
        string Echo,
        List<ActiveKeyClass> ActiveKeys)
    {

        /// <summary>
        /// Specifies the minimum number of digits which must be entered for the PIN. 
        /// A value of zero indicates no minimum PIN length verification.
        /// </summary>
        public int MinLen { get; init; } = MinLen;

        /// <summary>
        /// Specifies the maximum number of digits which can be entered for the PIN.
        /// A value of zero indicates no maximum PIN length verification.
        /// </summary>
        public int MaxLen { get; init; } = MaxLen;

        /// <summary>
        /// If autoEnd is set to true, the Service Provider terminates the command when the maximum number of digits are entered. 
        /// Otherwise, the input is terminated by the user using one of the termination keys. 
        /// autoEnd is ignored when maxLen is set to zero.
        /// </summary>
        public bool AutoEnd { get; init; } = AutoEnd;

        /// <summary>
        /// Specifies the replace character to be echoed on a local display for the PIN digit. 
        /// </summary>
        public string Echo { get; init; } = Echo;

        /// <summary>
        /// Specifies function keys which are active during the execution of the command.
        /// </summary>
        public List<ActiveKeyClass> ActiveKeys { get; init; } = ActiveKeys;

    }

    public sealed class PinEntryResult : DeviceResult
    {
        public PinEntryResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            PinEntryCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Digits = 0;
            Completion = null;
        }

        public PinEntryResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            int Digits,
            EntryCompletionEnum? Completion)
                : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Digits = Digits;
            this.Completion = Completion;
        }

        public PinEntryCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the number of PIN digits entered
        /// </summary>
        public int Digits { get; init; }

        /// <summary>
        /// Specifies the reason for completion of the entry. Unless otherwise specified the following values 
        /// must not be used in the execute event Keyboard.PinEntry or in the array of keys in 
        /// the completion of Keyboard.DataEntry
        /// </summary>
        public EntryCompletionEnum? Completion { get; init; }
    }

    public sealed class SecureKeyEntryRequest(
        int KeyLen,
        bool AutoEnd,
        List<ActiveKeyClass> ActiveKeys,
        SecureKeyEntryRequest.VerificationTypeEnum VerificationType,
        SecureKeyEntryRequest.CryptoMethodEnum CryptoMethod)
    {
        public enum VerificationTypeEnum
        {
            Self,
            Zero
        }

        public enum CryptoMethodEnum
        {
            Default,
            DES,
            TripleDES,
            AES
        }

        /// <summary>
        /// Specifies the number of digits which must be entered for the encryption key, 16 for a singlelength key, 
        /// 32 for a double-length key and 48 for a triple-length key.
        /// The only valid values are 16, 32 and 48.
        /// </summary>
        public int KeyLen { get; init; } = KeyLen;

        /// <summary>
        /// If autoEnd is set to true, the Service Provider terminates the command when the maximum number of encryption 
        /// key digits are entered. Otherwise, the input is terminated by the user using Enter, Cancel or any terminating key. 
        /// When keyLen is reached, the Service Provider will disable all keys associated with an encryption key digit.
        /// </summary>
        public bool AutoEnd { get; init; } = AutoEnd;

        /// <summary>
        /// Specifies those function keys which are active during the execution of the command.
        /// This parameter should include those FDKs mapped to edit functions.
        /// </summary>
        public List<ActiveKeyClass> ActiveKeys { get; init; } = ActiveKeys;

        /// <summary>
        /// Specifies the type of verification to be done on the entered key.
        /// </summary>
        public VerificationTypeEnum VerificationType { get; init; } = VerificationType;

        /// <summary>
        /// Specifies the cryptographic method to be used for the verification.
        /// If this property is omitted, *keyLen* will determine the cryptographic method used. 
        /// If *keyLen* is 16, the cryptographic method will be Single DES. 
        /// If *keyLen* is 32 or 48, the cryptographic method will be Triple DES
        /// </summary>
        public CryptoMethodEnum? CryptoMethod { get; init; } = CryptoMethod;
    }

    public sealed class SecureKeyEntryResult : DeviceResult
    {
        public SecureKeyEntryResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            string ErrorDescription = null,
            SecureKeyEntryCompletion.PayloadData.ErrorCodeEnum? ErrorCode = null)
            : base(CompletionCode, ErrorDescription)
        {
            this.ErrorCode = ErrorCode;
            this.Digits = 0;
            Completion = null;
            KeyCheckValue = null;
        }

        public SecureKeyEntryResult(
            MessageHeader.CompletionCodeEnum CompletionCode,
            int Digits,
            EntryCompletionEnum? Completion,
            List<byte> KeyCheckValue)
            : base(CompletionCode, null)
        {
            this.ErrorCode = null;
            this.Digits = Digits;
            this.Completion = Completion;
            this.KeyCheckValue = KeyCheckValue;
        }

        public SecureKeyEntryCompletion.PayloadData.ErrorCodeEnum? ErrorCode { get; init; }

        /// <summary>
        /// Specifies the number of key digits entered. Applications must ensure all required digits have been entered before trying to store the key.
        /// </summary>
        public int Digits { get; init; }


        public EntryCompletionEnum? Completion { get; init; }

        /// <summary>
        /// Contains the key check value data that can be used for verification of the entered key formatted in base 64. 
        /// This property it omitted if device does not have this capability, or the key entry was not fully entered, e.g. 
        /// the entry was terminated by Enter before the required number of digits was entered.
        /// </summary>
        public List<byte> KeyCheckValue { get; init; }
    }
}
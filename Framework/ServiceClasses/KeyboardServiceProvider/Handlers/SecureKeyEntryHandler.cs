/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Keyboard.Commands;
using XFS4IoT.Keyboard.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.KeyManagement;

namespace XFS4IoTFramework.Keyboard
{
    public partial class SecureKeyEntryHandler
    {
        private async Task<CommandResult<SecureKeyEntryCompletion.PayloadData>> HandleSecureKeyEntry(ISecureKeyEntryEvents events, SecureKeyEntryCommand secureKeyEntry, CancellationToken cancel)
        {
            if (secureKeyEntry.Payload.KeyLen is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No KeyLen specified.");
            }

            if (secureKeyEntry.Payload.VerificationType is null)
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No VerificationType specified.");
            }

            if (!Keyboard.SupportedFunctionKeys.ContainsKey(EntryModeEnum.Secure))
            {
                return new(
                    MessageHeader.CompletionCodeEnum.InvalidData,
                    $"No secure key entry layout supported.");
            }

            if (secureKeyEntry.Payload.AutoEnd is null)
                Logger.Warning(Constants.Framework, $"No AutoEnd specified. use default false.");

            List<ActiveKeyClass> keys = new();
            foreach (var key in secureKeyEntry.Payload.ActiveKeys)
            {
                if (!Keyboard.SupportedFunctionKeys[EntryModeEnum.Secure].Contains(key.Key) &&
                    !Keyboard.SupportedFunctionKeysWithShift[EntryModeEnum.Secure].Contains(key.Key))
                {
                    return new(
                        new(SecureKeyEntryCompletion.PayloadData.ErrorCodeEnum.KeyNotSupported),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Invalid key specified. {key.Key}");
                }
                keys.Add(new ActiveKeyClass(key.Key, key.Value.Terminate is not null && (bool)key.Value.Terminate));
            }

            KeyManagement.GetSecureKeyEntryStatus().ResetSecureKeyBuffered();

            Logger.Log(Constants.DeviceClass, "KeyboardDev.SecureKeyEntry()");

            var result = await Device.SecureKeyEntry(new KeyboardCommandEvents(events), 
                                                     new(secureKeyEntry.Payload.KeyLen switch
                                                         { 
                                                             SecureKeyEntryCommand.PayloadData.KeyLenEnum.Number16 => 16,
                                                             SecureKeyEntryCommand.PayloadData.KeyLenEnum.Number32 => 32,
                                                             _ => 48,
                                                         },
                                                         secureKeyEntry.Payload.AutoEnd is not null && (bool)secureKeyEntry.Payload.AutoEnd,
                                                         keys,
                                                         secureKeyEntry.Payload.VerificationType switch
                                                         {
                                                             SecureKeyEntryCommand.PayloadData.VerificationTypeEnum.Self => SecureKeyEntryRequest.VerificationTypeEnum.Self,
                                                             _ => SecureKeyEntryRequest.VerificationTypeEnum.Zero
                                                         },
                                                         secureKeyEntry.Payload.CryptoMethod is null ? SecureKeyEntryRequest.CryptoMethodEnum.Default : secureKeyEntry.Payload.CryptoMethod switch
                                                         {
                                                             SecureKeyEntryCommand.PayloadData.CryptoMethodEnum.Aes => SecureKeyEntryRequest.CryptoMethodEnum.AES,
                                                             SecureKeyEntryCommand.PayloadData.CryptoMethodEnum.Des => SecureKeyEntryRequest.CryptoMethodEnum.DES,
                                                             _ => SecureKeyEntryRequest.CryptoMethodEnum.TripleDES
                                                         }), 
                                                         cancel);

            Logger.Log(Constants.DeviceClass, $"KeyboardDev.SecureKeyEntry() -> {result.CompletionCode}, {result.ErrorCode}");

            if (result.CompletionCode == MessageHeader.CompletionCodeEnum.Success)
            {
                KeyManagement.GetSecureKeyEntryStatus().SecureKeyBuffered();
            }

            SecureKeyEntryCompletion.PayloadData payload = null;
            if (result.ErrorCode is not null ||
                result.Completion is not null ||
                result.KeyCheckValue?.Count > 0)
            {
                payload = new(
                    result.ErrorCode,
                    result.Digits,
                    result.Completion switch
                    {
                        EntryCompletionEnum.Auto => XFS4IoT.Keyboard.EntryCompletionEnum.Auto,
                        EntryCompletionEnum.Enter => XFS4IoT.Keyboard.EntryCompletionEnum.Enter,
                        EntryCompletionEnum.Cancel => XFS4IoT.Keyboard.EntryCompletionEnum.Cancel,
                        EntryCompletionEnum.Continue => XFS4IoT.Keyboard.EntryCompletionEnum.Continue,
                        EntryCompletionEnum.Clear => XFS4IoT.Keyboard.EntryCompletionEnum.Clear,
                        EntryCompletionEnum.Backspace => XFS4IoT.Keyboard.EntryCompletionEnum.Backspace,
                        EntryCompletionEnum.FDK => XFS4IoT.Keyboard.EntryCompletionEnum.Fdk,
                        EntryCompletionEnum.Help => XFS4IoT.Keyboard.EntryCompletionEnum.Help,
                        EntryCompletionEnum.FK => XFS4IoT.Keyboard.EntryCompletionEnum.Fk,
                        EntryCompletionEnum.ContinueFDK => XFS4IoT.Keyboard.EntryCompletionEnum.ContFdk,
                        _ => null,
                    },
                    result.KeyCheckValue);
            }

            return new(
                payload,
                result.CompletionCode,
                result.ErrorDescription);
        }

        private IKeyManagementService KeyManagement { get => Provider.IsA<IKeyManagementService>(); }
    }
}

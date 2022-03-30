/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.Completions;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;

namespace XFS4IoTFramework.CashAcceptor
{
    public partial class ConfigureNoteTypesHandler
    {
        private async Task<ConfigureNoteTypesCompletion.PayloadData> HandleConfigureNoteTypes(IConfigureNoteTypesEvents events, ConfigureNoteTypesCommand configureNoteTypes, CancellationToken cancel)
        {
            if (configureNoteTypes.Payload.Items == null ||
                configureNoteTypes.Payload.Items?.Count == 0)
            {
                return new ConfigureNoteTypesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                    $"No banknote items to be enabled or disabled.");
            }

            if (configureNoteTypes.Payload.Items.Select(i => string.IsNullOrEmpty(i.Item) || Regex.IsMatch(i.Item, "^type[0-9A-Z]+$")).ToList().Count == 0)
            {
                return new ConfigureNoteTypesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"Invalid item name specified.");
            }

            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new ConfigureNoteTypesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                    $"The exchange state is already in active.",
                                                                    ConfigureNoteTypesCompletion.PayloadData.ErrorCodeEnum.ExchangeActive);
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new ConfigureNoteTypesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                    $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}",
                                                                    ConfigureNoteTypesCompletion.PayloadData.ErrorCodeEnum.CashInActive);
            }

            // check specified banknote type is valid
            foreach (var item in from item in configureNoteTypes.Payload.Items
                                 where !Common.CashManagementCapabilities.AllBanknoteItems.ContainsKey(item.Item)
                                 select item)
            {
                return new ConfigureNoteTypesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.InvalidData,
                                                                    $"The specified item name doesn't exist. {item.Item}");
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.ConfigureNoteTypes()");

            var result = await Device.ConfigureNoteTypes(new ConfigureNoteTypesRequest(configureNoteTypes.Payload.Items.ToDictionary(i => i.Item, i => i.Enabled ?? false)), cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.ConfigureNoteTypes() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ConfigureNoteTypesCompletion.PayloadData(result.CompletionCode,
                                                                result.ErrorDescription,
                                                                result.ErrorCode);
        }

        private ICashManagementService CashManagement { get => Provider.IsA<ICashManagementService>(); }
    }
}

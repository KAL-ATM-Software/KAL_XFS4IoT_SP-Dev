/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2022
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoTServer;
using XFS4IoT.CashAcceptor.Commands;
using XFS4IoT.CashAcceptor.Completions;
using XFS4IoT.Completions;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.CashManagement;


namespace XFS4IoTFramework.CashAcceptor
{
    public partial class ConfigureNoteReaderHandler
    {
        private async Task<ConfigureNoteReaderCompletion.PayloadData> HandleConfigureNoteReader(IConfigureNoteReaderEvents events, ConfigureNoteReaderCommand configureNoteReader, CancellationToken cancel)
        {
            if (Common.CommonStatus.Exchange == CommonStatusClass.ExchangeEnum.Active)
            {
                return new ConfigureNoteReaderCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                     $"The exchange state is already in active.",
                                                                     ConfigureNoteReaderCompletion.PayloadData.ErrorCodeEnum.ExchangeActive);
            }

            if (CashAcceptor.CashInStatus.Status == CashInStatusClass.StatusEnum.Active)
            {
                return new ConfigureNoteReaderCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                     $"The cash-in state is in active. {CashAcceptor.CashInStatus.Status}",
                                                                     ConfigureNoteReaderCompletion.PayloadData.ErrorCodeEnum.CashInActive);
            }

            Logger.Log(Constants.DeviceClass, "CashAcceptorDev.ConfigureNoteReader()");

            var result = await Device.ConfigureNoteReader(new ConfigureNoteReaderRequest(configureNoteReader.Payload.LoadAlways is not null && (bool)configureNoteReader.Payload.LoadAlways), cancel);

            Logger.Log(Constants.DeviceClass, $"CashAcceptorDev.ConfigureNoteReader() -> {result.CompletionCode}, {result.ErrorCode}");

            return new ConfigureNoteReaderCompletion.PayloadData(result.CompletionCode,
                                                                 result.ErrorDescription,
                                                                 result.ErrorCode);
        }
    }
}

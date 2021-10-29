/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/


using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT;
using XFS4IoT.CashManagement.Commands;
using XFS4IoT.CashManagement.Completions;
using XFS4IoT.Completions;

namespace XFS4IoTFramework.CashManagement
{
    public partial class GetBankNoteTypesHandler
    {
        private Task<GetBankNoteTypesCompletion.PayloadData> HandleGetBankNoteTypes(IGetBankNoteTypesEvents events, GetBankNoteTypesCommand getBankNoteTypes, CancellationToken cancel)
        {
            // NOT SUPPORTED
            // KAL will support this command once CashAcceptor interface is supported.
            return Task.FromResult(new GetBankNoteTypesCompletion.PayloadData(MessagePayload.CompletionCodeEnum.UnsupportedCommand, null));
        }
    }
}

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
using XFS4IoT.Printer.Commands;
using XFS4IoT.Printer.Completions;

namespace XFS4IoTFramework.Printer
{
    public partial class GetQueryFormHandler
    {
        private Task<CommandResult<GetQueryFormCompletion.PayloadData>> HandleGetQueryForm(IGetQueryFormEvents events, GetQueryFormCommand getQueryForm, CancellationToken cancel)
        {
            Dictionary<string, Form> forms = Printer.GetForms();
            if (!forms.ContainsKey(getQueryForm.Payload.FormName))
            {
                return Task.FromResult(
                    new CommandResult<GetQueryFormCompletion.PayloadData>(
                        new(GetQueryFormCompletion.PayloadData.ErrorCodeEnum.FormNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified form doesn't exist. {getQueryForm.Payload.FormName}")
                    );
            }
            return Task.FromResult((forms[getQueryForm.Payload.FormName]).QueryForm());
        }
    }
}

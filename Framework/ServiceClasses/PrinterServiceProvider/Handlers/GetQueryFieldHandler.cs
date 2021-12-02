/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
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
using XFS4IoT.Completions;

namespace XFS4IoTFramework.Printer
{
    public partial class GetQueryFieldHandler
    {
        private Task<GetQueryFieldCompletion.PayloadData> HandleGetQueryField(IGetQueryFieldEvents events, GetQueryFieldCommand getQueryField, CancellationToken cancel)
        {
            Dictionary<string, Form> forms = Printer.GetForms();
            if (!forms.ContainsKey(getQueryField.Payload.FormName))
            {
                return Task.FromResult(new GetQueryFieldCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                               $"Specified form doesn't exist. {getQueryField.Payload.FormName}",
                                                                               GetQueryFieldCompletion.PayloadData.ErrorCodeEnum.FormNotFound));
            }

            Dictionary<string, GetQueryFieldCompletion.PayloadData.FieldsClass> fields = new();
            Form form = forms[getQueryField.Payload.FormName];

            if (!string.IsNullOrEmpty(getQueryField.Payload.FieldName))
            {
                if (!form.Fields.ContainsKey(getQueryField.Payload.FieldName))
                {
                    return Task.FromResult(new GetQueryFieldCompletion.PayloadData(MessagePayload.CompletionCodeEnum.CommandErrorCode,
                                                                                   $"Specified form doesn't exist. {getQueryField.Payload.FieldName}",
                                                                                   GetQueryFieldCompletion.PayloadData.ErrorCodeEnum.FieldNotFound));
                }

                fields.Add(getQueryField.Payload.FieldName, form.QueryField(getQueryField.Payload.FieldName));
            }
            else
            {
                foreach (var field in forms[getQueryField.Payload.FormName].Fields)
                {
                    fields.Add(field.Key, form.QueryField(field.Key));
                }
            }

            return Task.FromResult(new GetQueryFieldCompletion.PayloadData(MessagePayload.CompletionCodeEnum.Success,
                                                                           string.Empty,
                                                                           Fields: fields));
        }
    }
}

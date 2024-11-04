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
    public partial class GetQueryFieldHandler
    {
        private Task<CommandResult<GetQueryFieldCompletion.PayloadData>> HandleGetQueryField(IGetQueryFieldEvents events, GetQueryFieldCommand getQueryField, CancellationToken cancel)
        {
            Dictionary<string, Form> forms = Printer.GetForms();
            if (!forms.ContainsKey(getQueryField.Payload.FormName))
            {
                return Task.FromResult(
                    new CommandResult<GetQueryFieldCompletion.PayloadData>(
                        new(GetQueryFieldCompletion.PayloadData.ErrorCodeEnum.FormNotFound),
                        MessageHeader.CompletionCodeEnum.CommandErrorCode,
                        $"Specified form doesn't exist. {getQueryField.Payload.FormName}")
                    );
            }

            Dictionary<string, GetQueryFieldCompletion.PayloadData.FieldsClass> fields = [];
            Form form = forms[getQueryField.Payload.FormName];

            if (!string.IsNullOrEmpty(getQueryField.Payload.FieldName))
            {
                if (!form.Fields.ContainsKey(getQueryField.Payload.FieldName))
                {
                    return Task.FromResult(
                        new CommandResult<GetQueryFieldCompletion.PayloadData>(
                            new(GetQueryFieldCompletion.PayloadData.ErrorCodeEnum.FieldNotFound),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"Specified form doesn't exist. {getQueryField.Payload.FieldName}")
                        );
                }

                var result = form.ValidateField(getQueryField.Payload.FieldName, Device);
                if (result.Result != ValidationResultClass.ValidateResultEnum.Valid)
                {
                    return Task.FromResult(
                        new CommandResult<GetQueryFieldCompletion.PayloadData>(
                            new(result.Result == ValidationResultClass.ValidateResultEnum.Invalid ? GetQueryFieldCompletion.PayloadData.ErrorCodeEnum.FieldInvalid : GetQueryFieldCompletion.PayloadData.ErrorCodeEnum.FieldNotFound),
                            MessageHeader.CompletionCodeEnum.CommandErrorCode,
                            $"Specified field {getQueryField.Payload.FieldName} is invalid. {result.Reason}")
                        );
                }

                fields.Add(getQueryField.Payload.FieldName, form.QueryField(getQueryField.Payload.FieldName));
            }
            else
            {
                foreach (var field in forms[getQueryField.Payload.FormName].Fields)
                {
                    var result = form.ValidateField(field.Key, Device);
                    if (result.Result != ValidationResultClass.ValidateResultEnum.Valid)
                    {
                        return Task.FromResult(
                            new CommandResult<GetQueryFieldCompletion.PayloadData>(
                                new(result.Result == ValidationResultClass.ValidateResultEnum.Invalid ? GetQueryFieldCompletion.PayloadData.ErrorCodeEnum.FieldInvalid : GetQueryFieldCompletion.PayloadData.ErrorCodeEnum.FieldNotFound),
                                MessageHeader.CompletionCodeEnum.CommandErrorCode,
                                $"Specified field {field.Key} is invalid. {result.Reason}")
                            );
                    }

                    fields.Add(field.Key, form.QueryField(field.Key));
                }
            }

            return Task.FromResult(
                new CommandResult<GetQueryFieldCompletion.PayloadData>(
                    fields.Count == 0 ? null : new(Fields: fields),
                    MessageHeader.CompletionCodeEnum.Success)
                );
        }
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2025
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;
using XFS4IoT;

namespace XFS4IoTFramework.CardReader
{
    public partial class QueryIFMIdentifierHandler
    {
        private Task<CommandResult<QueryIFMIdentifierCompletion.PayloadData>> HandleQueryIFMIdentifier(IQueryIFMIdentifierEvents events, QueryIFMIdentifierCommand queryIFMIdentifier, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.QueryIFMIdentifier()");
            var result = Device.QueryIFMIdentifier();
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.QueryIFMIdentifier() -> {result.CompletionCode}");

            Dictionary<string, string> IfmIdentifiers = null;
            if (result.IFMIdentifiers?.Count > 0)
            {
                IfmIdentifiers = [];
                foreach (var identifier in result.IFMIdentifiers)
                {
                    IfmIdentifiers.Add(
                        identifier.IFMAuthority switch
                        {
                            IFMIdentifierInfo.IFMAuthorityEnum.EMV => "emv",
                            IFMIdentifierInfo.IFMAuthorityEnum.EuroPay => "europay",
                            IFMIdentifierInfo.IFMAuthorityEnum.GIECB => "giecb",
                            _ => "visa",
                        },
                        identifier.IFMIdentifier);
                }
            }

            return Task.FromResult(new CommandResult<QueryIFMIdentifierCompletion.PayloadData>(
                IfmIdentifiers is not null ? new(IfmIdentifiers) : null,
                result.CompletionCode,
                result.ErrorDescription));
        }
    }
}

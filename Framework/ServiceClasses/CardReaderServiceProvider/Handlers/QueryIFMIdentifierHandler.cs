/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using System.Threading;
using XFS4IoT.CardReader.Commands;
using XFS4IoT.CardReader.Completions;

namespace XFS4IoTFramework.CardReader
{
    public partial class QueryIFMIdentifierHandler
    {
        private Task<QueryIFMIdentifierCompletion.PayloadData> HandleQueryIFMIdentifier(IQueryIFMIdentifierEvents events, QueryIFMIdentifierCommand queryIFMIdentifier, CancellationToken cancel)
        {
            Logger.Log(Constants.DeviceClass, "CardReaderDev.QueryIFMIdentifier()");
            var result = Device.QueryIFMIdentifier();
            Logger.Log(Constants.DeviceClass, $"CardReaderDev.QueryIFMIdentifier() -> {result.CompletionCode}");

            // XFS4IoT spec has a BUG, only one output structure allows for the IFM information and should be array. April 2021 preview
            if (result.IFMIdentifiers is not null && 
                result.IFMIdentifiers.Count > 0 &&
                result.IFMIdentifiers[0].IFMIdentifier.Count > 0)
            {
                return Task.FromResult(new QueryIFMIdentifierCompletion.PayloadData(result.CompletionCode,
                                                                                    result.ErrorDescription,
                                                                                    result.IFMIdentifiers[0].IFMAuthority,
                                                                                    Convert.ToBase64String(result.IFMIdentifiers[0].IFMIdentifier.ToArray())));
            }

            return Task.FromResult(new QueryIFMIdentifierCompletion.PayloadData(result.CompletionCode,
                                                                                result.ErrorDescription));
        }
    }
}

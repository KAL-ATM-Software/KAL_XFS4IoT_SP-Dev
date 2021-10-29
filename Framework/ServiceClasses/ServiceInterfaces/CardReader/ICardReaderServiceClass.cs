/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 * 
\***********************************************************************************************/

using XFS4IoTFramework.CardReader;
using XFS4IoTFramework.Common;
using XFS4IoTFramework.Storage;

namespace XFS4IoTFramework.CardReader
{
    public interface ICardReaderService : ICommonService, IStorageService
    {
    }

    public interface ICardReaderServiceClass : ICardReaderService, ICardReaderUnsolicitedEvents
    {
    }
}

/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Threading.Tasks;

namespace XFS4IoTFramework.CardReader
{
    public interface IAcceptCardEvents
    {
        Task InsertCardEvent();

        Task MediaInsertedEvent();

        Task InvalidMediaEvent();
    }
}
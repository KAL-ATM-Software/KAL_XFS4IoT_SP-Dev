/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
\***********************************************************************************************/

using System.Collections;

namespace XFS4IoT
{
    public interface IMessageDecoder: IEnumerable
    {
        bool TryUnserialise(string JSON, out object result);
    }
}
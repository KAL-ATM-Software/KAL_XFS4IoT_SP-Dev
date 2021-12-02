/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoTFramework.Common;

namespace XFS4IoTFramework.Crypto
{
    public sealed class CryptoCommandEvents
    {
        public CryptoCommandEvents(ICryptoDataEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(CryptoCommandEvents));
            events.IsA<ICryptoDataEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(CryptoCommandEvents));
            CryptoDataEvents = events;
        }
        public CryptoCommandEvents(IGenerateAuthenticationEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(CryptoCommandEvents));
            events.IsA<IGenerateAuthenticationEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(CryptoCommandEvents));
            GenerateAuthenticationEvents = events;
        }
        public CryptoCommandEvents(IVerifyAuthenticationEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(CryptoCommandEvents));
            events.IsA<IVerifyAuthenticationEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(CryptoCommandEvents));
            VerifyAuthenticationEvents = events;
        }

        public Task DUKPTKSNEvent(string Key, string KSN)
        {
            XFS4IoT.Crypto.Events.DUKPTKSNEvent.PayloadData payload = new(Key, KSN);
                
            if (CryptoDataEvents is not null)
            {
                return CryptoDataEvents.DUKPTKSNEvent(payload);
            }
            if (GenerateAuthenticationEvents is not null)
            {
                return GenerateAuthenticationEvents.DUKPTKSNEvent(payload);
            }
            if (VerifyAuthenticationEvents is not null)
            {
                return VerifyAuthenticationEvents.DUKPTKSNEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(DUKPTKSNEvent));
        }

        private ICryptoDataEvents CryptoDataEvents { get; init; } = null;
        private IGenerateAuthenticationEvents GenerateAuthenticationEvents { get; init; } = null;
        private IVerifyAuthenticationEvents VerifyAuthenticationEvents { get; init; } = null;
    }
}

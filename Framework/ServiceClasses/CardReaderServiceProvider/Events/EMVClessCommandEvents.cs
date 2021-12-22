/***********************************************************************************************\
 * (C) KAL ATM Software GmbH, 2021
 * KAL ATM Software GmbH licenses this file to you under the MIT license.
 * See the LICENSE file in the project root for more information.
 *
\***********************************************************************************************/

using System;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.CardReader.Events;

namespace XFS4IoTFramework.CardReader
{
    public sealed class EMVClessCommandEvents
    {
        public enum ValueQualifierEnum
        {
            Amount,
            Balance,
            NotApplicable
        }

        public enum StatusEnum
        {
            NotReady,
            Idle,
            ReadyToRead,
            Processing,
            CardReadOk,
            ProcessingError
        }

        public EMVClessCommandEvents(IEMVClessIssuerUpdateEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(EMVClessCommandEvents));
            events.IsA<IEMVClessIssuerUpdateEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(EMVClessCommandEvents));
            EMVClessIssuerUpdateEvent = events;
        }
        public EMVClessCommandEvents(IEMVClessPerformTransactionEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(EMVClessCommandEvents));
            events.IsA<IEMVClessPerformTransactionEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(EMVClessCommandEvents));
            EMVClessPerformTransactionEvent = events;
        }

        public Task EMVClessReadStatusEvent(int MessageId, 
                                            StatusEnum Status, 
                                            int HoldTime, 
                                            ValueQualifierEnum ValueQualifier, 
                                            string Value, 
                                            string CurrencyCode,
                                            string LanguagePreference)
        {
            EMVClessReadStatusEvent.PayloadData payload = new(
                       MessageId,
                       Status switch
                       {
                           StatusEnum.CardReadOk => XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData.StatusEnum.CardReadOk,
                           StatusEnum.Idle => XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData.StatusEnum.Idle,
                           StatusEnum.NotReady => XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData.StatusEnum.NotReady,
                           StatusEnum.Processing => XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData.StatusEnum.Processing,
                           StatusEnum.ProcessingError => XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData.StatusEnum.ProcessingError,
                           _ => XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData.StatusEnum.ReadyToRead,
                       },
                       HoldTime,
                       ValueQualifier switch
                       {
                           ValueQualifierEnum.Amount => XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData.ValueQualifierEnum.Amount,
                           _ => XFS4IoT.CardReader.Events.EMVClessReadStatusEvent.PayloadData.ValueQualifierEnum.Balance,
                       },
                       Value,
                       CurrencyCode,
                       LanguagePreference); ;

            if (EMVClessIssuerUpdateEvent is not null)
            {
                return EMVClessIssuerUpdateEvent.EMVClessReadStatusEvent(payload);
            }
            if (EMVClessPerformTransactionEvent is not null)
            {
                return EMVClessPerformTransactionEvent.EMVClessReadStatusEvent(payload);
            }

            throw new InvalidOperationException($"Unreachable code. " + nameof(EMVClessReadStatusEvent));
        }

        private IEMVClessIssuerUpdateEvents EMVClessIssuerUpdateEvent { get; init; } = null;
        private IEMVClessPerformTransactionEvents EMVClessPerformTransactionEvent { get; init; } = null;
    }
}

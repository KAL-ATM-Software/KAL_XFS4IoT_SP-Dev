using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFS4IoT;
using XFS4IoT.CardReader.Events;
using XFS4IoTFramework.CardReader;

namespace XFS4IoTFramework.Biometric
{
    public sealed class ReadCommandEvents
    {
        public ReadCommandEvents(IReadEvents events)
        {
            events.IsNotNull($"Invalid parameter passed in. " + nameof(ReadCommandEvents));
            events.IsA<IReadEvents>($"Invalid interface passed in. {events.GetType()} " + nameof(ReadCommandEvents));
            ReadEvents = events;
        }

        public Task PresentSubjectEvent() => ReadEvents.PresentSubjectEvent();

        public Task SubjectDetectedEvent() => ReadEvents.SubjectDetectedEvent();

        public Task RemoveSubjectEvent() => ReadEvents.RemoveSubjectEvent();

        private IReadEvents ReadEvents { get; init; } = null;
    }
}

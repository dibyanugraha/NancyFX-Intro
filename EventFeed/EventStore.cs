using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ShoppingCart;

namespace ShoppingCart.EventFeed
{
    public class EventStore : IEventStore
    {
        private static int currentSequenceNumber = 0;
        private static readonly IList<Event> database = new List<Event>();

        public IEnumerable<Event> GetEvents(
            long firstEventSequenceNumber,
            long lastEventSequenceNumber
        )
        {
            return database
                .Where(data => 
                    data.SequenceNumber >= firstEventSequenceNumber 
                    && data.SequenceNumber <= lastEventSequenceNumber)
                .OrderBy(data => data.SequenceNumber);
        }
        public void Raise(string eventName, object content)
        {
            var seqNumber = Interlocked.Increment(ref currentSequenceNumber);

            database.Add(
                new Event(
                    seqNumber,
                    DateTimeOffset.UtcNow,
                    eventName,
                    content
                )
            );
        }
    }
}
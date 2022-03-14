using System;
using System.Collections.Generic;

namespace QueueSubscribers.Interfaces
{
    public interface IQueueSubscriber : IDisposable
    {
        bool Consume(Func<string, IDictionary<string, object>, bool> callback);
    }
}

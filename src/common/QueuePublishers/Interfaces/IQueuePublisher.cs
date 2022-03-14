namespace QueuePublishers.Interfaces
{
    public interface IQueuePublisher : System.IDisposable
    {
        bool Publish(object data);
    }
}

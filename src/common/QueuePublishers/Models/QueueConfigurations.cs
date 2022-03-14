namespace QueuePublishers.Models
{
    public class QueueConfigurations
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Port { get; set; }
        public string Exchange { get; set; }
        public string ChatSessionQueue { get; set; }
        public string ChatSessionRoutingKey { get; set; }
    }
}

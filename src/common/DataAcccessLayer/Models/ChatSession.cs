namespace DataAcccessLayer.Models
{
    public class ChatSession
    {
        public string SessionId { get; set; } = System.Guid.NewGuid().ToString();

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string ChatSubject { get; set; }

        public string ChatDetail { get; set; }

        public string Status { get; set; }

        public string AgentId { get; set; }

        public object Conversation { get; set; }

        public System.DateTime CreateDate { get; }

    }
}

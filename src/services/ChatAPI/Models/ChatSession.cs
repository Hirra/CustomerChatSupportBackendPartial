using System;

namespace ChatAPI.Models
{
    public class ChatSession
    {
        public string SessionId { get;} = new Guid().ToString();
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ChatSubject { get; set; }
        public string ChatDetail { get; set; }
        public System.DateTime CreateDate { get; } = DateTime.UtcNow;
    }
}

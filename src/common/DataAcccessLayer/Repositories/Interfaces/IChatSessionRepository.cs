using DataAcccessLayer.Models;

namespace DataAcccessLayer.Repositories.Interfaces
{
    public interface IChatSessionRepository
    {
        ChatSession InsertNewSession(ChatSession chatSession);
        bool UpdateSession(ChatSession session);
        int GetCurrentActiveSessionCount();
    }
}

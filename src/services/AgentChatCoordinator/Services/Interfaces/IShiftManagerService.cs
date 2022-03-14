using DataAcccessLayer.Models;

namespace AgentChatCoordinator.Services.Interfaces
{
    public interface IShiftManagerService
    { 
        bool OfficeHoursActive();
        bool ChatQueueIsFull();
        bool OverflowIsFull();
        bool CanStartOverflow();

        void TriggerOverflowIfRequired();
        ChatSession PushNewSessionToQueue(ChatSession newChatSession);
        bool AssignChatSessionToAgent(ChatSession session, Agent agent);
    }
}

using DataAcccessLayer.Models;
using System.Collections.Generic;

namespace DataAcccessLayer.InmemoryDataStore
{
    public interface IDateStore
    {
        List<Agent> GetActiveAgents();
        void SetActiveAgents(List<Agent> newTeam);
        void AddSession(ChatSession chatSession);
        void UpdateChatSession(ChatSession session);
        int GetActiveChatCount();
        void ActivateOverflowTeam(); 
        void UpdateAgentSessions(string agentId, string sessionId);
    }
}

using DataAcccessLayer.Models;
using System.Collections.Generic;

namespace DataAcccessLayer.Repositories.Interfaces
{
    public interface IAgentsRepository
    {
        bool SetActiveTeam(IEnumerable<Agent> agents);
        IEnumerable<Agent> GetActiveTeam(); 
        float GetTeamCapacity(); 
        void UpdateAgentSessions(string agentId, string sessionId);
        bool IsOverflowTeamActive();
        bool AreAciveOfficeHours();
        void ActivateOverflowTeam(); 
    }
}

using DataAcccessLayer.Models; 
using System.Collections.Generic;

namespace AgentChatCoordinator.Services.Interfaces
{
    public interface IAgentsManagerService
    {
        bool AssignTeam(IEnumerable<Agent> agents);
        IEnumerable<Agent> GetActiveTeam();
        Agent GetNextActiveAgent();
    }
     
}

using AgentChatCoordinator.Services.Interfaces;
using DataAcccessLayer.Models;
using DataAcccessLayer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace AgentChatCoordinator.Services
{
    public class AgentsManagerService : IAgentsManagerService
    { 
        private readonly IAgentsRepository agentsRepository;
        private readonly ILogger<AgentsManagerService> logger;
        public AgentsManagerService(IAgentsRepository agentsRepository, ILogger<AgentsManagerService> logger)
        { 
            this.agentsRepository = agentsRepository;
            this.logger = logger; 
        }

        public bool AssignTeam(IEnumerable<Agent> agents)
        {   
            this.agentsRepository.SetActiveTeam(agents); 
            return true;
        }

        public IEnumerable<Agent> GetActiveTeam()
        {
            return this.agentsRepository.GetActiveTeam();
        }

        public Agent GetNextActiveAgent()
        {
            return   
                this.agentsRepository
                .GetActiveTeam()
                .Where(x => x.ActiveSessionId.Count < x.SeniortyPoint * 10)
                .OrderBy(x => x.ActiveSessionId.Count)
                .OrderBy(x => x.SeniortyPoint).FirstOrDefault(); 
        }
    } 
}

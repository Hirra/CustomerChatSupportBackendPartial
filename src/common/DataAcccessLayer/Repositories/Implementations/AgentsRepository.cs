using DataAcccessLayer.InmemoryDataStore;
using DataAcccessLayer.Models;
using DataAcccessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DataAcccessLayer.Repositories.Implementations
{
    public class AgentsRepository : IAgentsRepository
    {
        private readonly IDateStore dateStore;

        public AgentsRepository(IDateStore dateStore)
        {
            this.dateStore = dateStore;
        }

        public void ActivateOverflowTeam()
        {
            this.dateStore.ActivateOverflowTeam();
        }

        public bool AreAciveOfficeHours()
        {
            //TODO pendng implementation
            return true;
        }

        public IEnumerable<Agent> GetActiveTeam()
        {
            return this.dateStore.GetActiveAgents(); 
        }

        public float GetTeamCapacity()
        {
            float capacity = 0.0f;
            foreach (var agent in this.dateStore.GetActiveAgents().Where(x => !x.FromOverflow)) 
                capacity += (float) 10 * agent.SeniortyPoint; 

            return capacity;
        }

        public bool IsOverflowTeamActive()
        {
            return (this.dateStore.GetActiveAgents().Count(x => x.FromOverflow) > 0) ;
        }

        public bool SetActiveTeam(IEnumerable<Agent> agents)
        {
            var newTeam  = new List<Agent>();
            foreach (var agent in agents) 
                newTeam.Add(agent); 

            this.dateStore.SetActiveAgents(newTeam);
            return true;
        }

        public void UpdateAgentSessions(string agentId, string sessionId)
        {
            this.dateStore.UpdateAgentSessions(agentId, sessionId);
        } 
    }
}

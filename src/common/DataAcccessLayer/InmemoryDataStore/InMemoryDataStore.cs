using DataAcccessLayer.Constants;
using DataAcccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAcccessLayer.InmemoryDataStore
{
    public class InMemoryDataStore : IDateStore
    {

        public void AddSession(ChatSession chatSession)
        {
            this.activeChatSessions.Add(chatSession);
        }

        public int GetActiveChatCount()
        {
            return this.activeChatSessions.Where(x => !x.Status.Equals(SessionStatuses.CLOSED)).Count();
        }

        public void UpdateChatSession(ChatSession session)
        {
            var itemIndex = this.activeChatSessions.FindIndex(x => x.SessionId == session.SessionId);
            if (itemIndex != -1)
            {
                this.activeChatSessions[itemIndex] = session;
            }
        }

        public List<Agent> GetActiveAgents()
        {
            return this.activeAgents;
        }

        public void SetActiveAgents(List<Agent> newTeam)
        {
            newTeam.Select(x => x.Active = true);
            this.activeAgents = newTeam; 
        }

        public void ActivateOverflowTeam()
        {
            var team = this.overflowAgents.Select(c => { c.Active = true; return c; });
            this.activeAgents.AddRange(team); 
        } 

        public void UpdateAgentSessions(string agentId, string sessionId)
        {
            this.activeAgents.Where(x => x.Id.Equals(agentId)).FirstOrDefault()?.ActiveSessionId.Add(sessionId);
        }
         
        public List<Agent> activeAgents = new List<Agent>();
        public List<ChatSession> activeChatSessions = new List<ChatSession>();
        public List<Agent> overflowAgents = new List<Agent>()
        {
            new Agent { Id = "10001" ,SeniortyLevel = Constants.SeniorityLevels.JUNIOR , FromOverflow = true},
            new Agent { Id = "10002" ,SeniortyLevel = Constants.SeniorityLevels.JUNIOR , FromOverflow = true},
            new Agent { Id = "10003" ,SeniortyLevel = Constants.SeniorityLevels.JUNIOR , FromOverflow = true},
            new Agent { Id = "10004" ,SeniortyLevel = Constants.SeniorityLevels.JUNIOR , FromOverflow = true},
            new Agent { Id = "10005" ,SeniortyLevel = Constants.SeniorityLevels.JUNIOR , FromOverflow = true},
            new Agent { Id = "10006" ,SeniortyLevel = Constants.SeniorityLevels.JUNIOR , FromOverflow = true}
        };
    }
}

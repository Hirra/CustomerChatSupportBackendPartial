using AgentChatCoordinator.Services.Interfaces;
using DataAcccessLayer.Constants;
using DataAcccessLayer.Models;
using DataAcccessLayer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;

namespace AgentChatCoordinator.Services
{
    public class ShiftManagerService : IShiftManagerService
    {
        private readonly IChatSessionRepository chatSessionRepository;
        private readonly IAgentsRepository agentsRepository;
        private readonly ILogger<ShiftManagerService> logger;

        private readonly int OverflowTeamCapacity = (int) 0.4f * 10 * 6;
         
        public ShiftManagerService(IChatSessionRepository chatSessionRepository, IAgentsRepository agentsRepository, ILogger<ShiftManagerService> logger)
        {
            this.chatSessionRepository = chatSessionRepository;
            this.agentsRepository = agentsRepository;
            this.logger = logger;
        }

        public bool OfficeHoursActive()
        {
            if (this.agentsRepository.AreAciveOfficeHours())
                return true;

            return false;
        }

        public bool ChatQueueIsFull()
        {

            if (this.chatSessionRepository.GetCurrentActiveSessionCount() >= currentQueueMaxSize())
                return true;

            return false;
        }

        public float currentQueueMaxSize()
        {
            return this.agentsRepository.GetTeamCapacity() * 1.5f;
        }


        public bool OverflowIsFull()
        {
            if (this.chatSessionRepository.GetCurrentActiveSessionCount() >= ((int)currentQueueMaxSize() + OverflowTeamCapacity))
                return true;

            return false;
        }

        public bool CanStartOverflow()
        {
            if (this.agentsRepository.AreAciveOfficeHours() && this.ChatQueueIsFull())
                return true;

            return false;
        }

        public ChatSession PushNewSessionToQueue(ChatSession newChatSession)
        {
            try
            {

                var session = this.chatSessionRepository.InsertNewSession(newChatSession); 
                return session;
            }
            catch (Exception exception)
            {
                this.logger.LogDebug(exception.Message + "\\n" + exception.StackTrace);
                return null;
            }
        }

        public bool OverflowRequired()
        {
            if (!ChatQueueIsFull())
                return false;

            if (!OfficeHoursActive())
                return false;
            
            return true;
        }

        public void TriggerOverflow()
        {
            this.agentsRepository.ActivateOverflowTeam(); 
        }


        public bool AssignChatSessionToAgent(ChatSession session , Agent agent)
        {
            try
            {
                session.Status = SessionStatuses.ASSGINED_TO_AGENT;
                session.AgentId = agent.Id;
                this.chatSessionRepository.UpdateSession(session); 
                this.agentsRepository.UpdateAgentSessions(agent.Id,session.SessionId);
            }
            catch (Exception exception)
            {
                this.logger.LogDebug(exception.Message + "\\n" + exception.StackTrace);
                return false;
            }

            return true;
        }

        public void TriggerOverflowIfRequired()
        {
            if (this.OverflowRequired())
                this.TriggerOverflow();
        }
    }
}

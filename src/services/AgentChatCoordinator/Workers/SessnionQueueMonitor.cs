using AgentChatCoordinator.Services.Interfaces;
using DataAcccessLayer.Constants;
using DataAcccessLayer.Models; 
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QueueSubscribers.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AgentChatCoordinator.Workers
{
    public class SessnionQueueMonitor : IHostedService
    {
        private readonly IAgentsManagerService agentsManager;
        private readonly IQueueSubscriber queueSubscriber;
        private readonly IShiftManagerService managerService;
        private readonly ILogger<SessnionQueueMonitor> logger;

        public SessnionQueueMonitor(IAgentsManagerService agentsManager, IQueueSubscriber queueSubscriber, IShiftManagerService managerService, ILogger<SessnionQueueMonitor> logger)
        {
            this.agentsManager = agentsManager;
            this.queueSubscriber = queueSubscriber;
            this.managerService = managerService;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.queueSubscriber.Consume(ProcessQueueMessage);
            return Task.CompletedTask;
        }

        public bool ProcessQueueMessage(string message, IDictionary<string, object> messageHeader)
        {
            try
            {
                var newChatSession = JsonConvert.DeserializeObject<ChatSession>(message);
                newChatSession.Status = SessionStatuses.CREATED; 

                var session = this.managerService.PushNewSessionToQueue(newChatSession);
                if (session != null)
                {
                    Agent agent = this.agentsManager.GetNextActiveAgent();
                    if (agent != null)
                    {
                       this.managerService.AssignChatSessionToAgent(session, agent);
                    } 
                }
                managerService.TriggerOverflowIfRequired();

                return true;
            }
            catch (System.Exception exception)
            {
                this.logger.LogDebug(exception.Message + "\\n" + exception.StackTrace);
                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

using AgentChatCoordinator.Services;
using AgentChatCoordinator.Services.Interfaces;
using DataAcccessLayer.Constants;
using DataAcccessLayer.Models;
using DataAcccessLayer.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CustomerChatSupportTests.ServicesTests
{
    [TestFixture]
    public class AgentManagerServiceTests
    {
        private Mock<IAgentsRepository> mockAgentsRepository;
        private Mock<ILogger<AgentsManagerService>> mockLogger;

        private IAgentsManagerService agentsManagerService;

        private List<Agent> mockTeamA;

        [SetUp]
        public void Setup()
        {
            mockTeamA = new List<Agent>()
            {
                new Agent { Id = "2", SeniortyLevel = SeniorityLevels.JUNIOR, Shift = Shifts.EVENING},
                new Agent { Id = "1", SeniortyLevel = SeniorityLevels.SENIOR, Shift = Shifts.EVENING},
                new Agent { Id = "3", SeniortyLevel = SeniorityLevels.JUNIOR, Shift = Shifts.EVENING}
            };

            mockAgentsRepository = new Mock<IAgentsRepository>();
            mockLogger = new Mock<ILogger<AgentsManagerService>>();

            agentsManagerService = new AgentsManagerService(mockAgentsRepository.Object, mockLogger.Object);


        }

        [Test]
        public void AssignAgent_RoundRobinAssignment_FirstChatRequest()
        {
            //arrange
            this.mockAgentsRepository.Setup(x => x.GetActiveTeam()).Returns(mockTeamA);

            //act
            var agent = this.agentsManagerService.GetNextActiveAgent();

            //assert
            Assert.That(agent.Id, Is.EqualTo("2"));
        }

        [Test]
        public void AssignAgent_RoundRobinAssignment_SecondChatRequest()
        {
            //arrange
            mockTeamA.Where(x => x.Id == "2").FirstOrDefault().ActiveSessionId.Add("mockSessioId");
            this.mockAgentsRepository.Setup(x => x.GetActiveTeam()).Returns(mockTeamA);

            //act
            var agent = this.agentsManagerService.GetNextActiveAgent();

            //assert
            Assert.That(agent.Id, Is.EqualTo("3"));
        }

        [Test]
        public void AssignAgent_RoundRobinAssignment_ThirdChatRequest()
        {
            //arrange
            mockTeamA.Where(x => x.Id == "2").FirstOrDefault().ActiveSessionId.Add("mockSessioId"); 
            mockTeamA.Where(x => x.Id == "3").FirstOrDefault().ActiveSessionId.Add("mockSessioId");
            this.mockAgentsRepository.Setup(x => x.GetActiveTeam()).Returns(mockTeamA);

            //act
            var agent = this.agentsManagerService.GetNextActiveAgent();

            //assert
            Assert.That(agent.Id, Is.EqualTo("2"));
        }

        [Test]
        public void AssignAgent_RoundRobinAssignment_ForthChatRequest()
        {
            //arrange
            mockTeamA.Where(x => x.Id == "2").FirstOrDefault().ActiveSessionId.Add("mockSessioId");
            mockTeamA.Where(x => x.Id == "2").FirstOrDefault().ActiveSessionId.Add("mockSessioId 2");
            mockTeamA.Where(x => x.Id == "3").FirstOrDefault().ActiveSessionId.Add("mockSessioId"); 
            this.mockAgentsRepository.Setup(x => x.GetActiveTeam()).Returns(mockTeamA);

            //act
            var agent = this.agentsManagerService.GetNextActiveAgent();

            //assert
            Assert.That(agent.Id, Is.EqualTo("3"));
        }

        [Test]
        public void AssignAgent_RoundRobinAssignment_FifthChatRequest()
        {
            //arrange
            mockTeamA.Where(x => x.Id == "2").FirstOrDefault().ActiveSessionId.Add("mockSessioId");
            mockTeamA.Where(x => x.Id == "2").FirstOrDefault().ActiveSessionId.Add("mockSessioId 2");
            mockTeamA.Where(x => x.Id == "3").FirstOrDefault().ActiveSessionId.Add("mockSessioId");
            mockTeamA.Where(x => x.Id == "3").FirstOrDefault().ActiveSessionId.Add("mockSessioId 3");
            this.mockAgentsRepository.Setup(x => x.GetActiveTeam()).Returns(mockTeamA);

            //act
            var agent = this.agentsManagerService.GetNextActiveAgent();

            //assert
            Assert.That(agent.Id, Is.EqualTo("2"));
        }

        [Test]
        public void AssignAgent_RoundRobinAssignment_JuniorMaxOutChatRequest()
        {
            //arrange
            mockTeamA.Where(x => x.Id == "2").FirstOrDefault().ActiveSessionId 
                = new List<string> { "string 1", "string 2" , "string 3", "string 4", "string 5", "string 6", "string 7", "string 8", "string 9", "string 10" };
            mockTeamA.Where(x => x.Id == "3").FirstOrDefault().ActiveSessionId
                = new List<string> { "string 1", "string 2", "string 3", "string 4", "string 5", "string 6", "string 7", "string 8", "string 9", "string 10" };

            this.mockAgentsRepository.Setup(x => x.GetActiveTeam()).Returns(mockTeamA);

            //act
            var agent = this.agentsManagerService.GetNextActiveAgent();

            //assert
            Assert.That(agent.Id, Is.EqualTo("1"));
        }
    }
}

using ChatAPI.Controllers;
using ChatAPI.Models;
using ChatAPI.Utilites.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using QueuePublishers.Interfaces;
using System.Threading.Tasks;

namespace CustomerChatSupportTests.ControllerTests
{
    [TestFixture]
    public class ChatRequestsControllerTests
    {
        private ChatRequestsController chatRequestsController; 
        
        private Mock<IChatSupport> mockChatSupport;
        private Mock<IQueuePublisher> mockPublisher; 
        private Mock<ILogger<ChatRequestsController>> mockLoggerMock;

        private ChatSession mockChatSession;

        [SetUp]
        public void Setup()
        {
            this.mockChatSupport = new Mock<IChatSupport>();
            this.mockPublisher = new Mock<IQueuePublisher>();
            this.mockLoggerMock = new Mock<ILogger<ChatRequestsController>>();

            this.chatRequestsController = new ChatRequestsController(
                this.mockChatSupport.Object, this.mockPublisher.Object, this.mockLoggerMock.Object);

            this.mockChatSession = new ChatSession
            {
                CustomerName = "mock customer name",
                CustomerEmail = "mockemail@mockdomai.com",
                ChatSubject = "mock subject",
                ChatDetail = "mock details"
            };
        }

        [Test]
        public async Task CustomerChatSession_QueuedSuccessfully()
        {
            //arrange
            this.mockChatSupport.Setup(x => x.RefuseNewChats()).ReturnsAsync(false);
            this.mockPublisher.Setup(x => x.Publish(mockChatSession)).Returns(true);
            
            //act
            var response = await this.chatRequestsController.Post(this.mockChatSession);

            // Assert
            Assert.IsInstanceOf<OkResult>(response); 
        }

        [Test]
        public async Task CustomerChatSession_ChatRefused()
        {
            //arrange
            this.mockChatSupport.Setup(x => x.RefuseNewChats()).ReturnsAsync(true); 

            //act
            var response = await this.chatRequestsController.Post(this.mockChatSession) as ObjectResult;

            // Assert
            Assert.IsInstanceOf<ObjectResult>(response);
            Assert.That(response.StatusCode, Is.EqualTo(500)); 
        }
    }
}

using AgentChatCoordinator.Controllers; 
using AgentChatCoordinator.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;


namespace CustomerChatSupportTests.ControllerTests
{
    [TestFixture]
    public class ChatSupportControllerTests
    {
        private ChatSupportController chatSupportController;

        private Mock<IShiftManagerService> mockShiftManager; 

        [SetUp]
        public void Setup()
        {
            this.mockShiftManager = new Mock<IShiftManagerService>(); 
            this.chatSupportController = new ChatSupportController(this.mockShiftManager.Object); 
        }

        [Test]
        public void NewChatRequest_AfterOfficeHrs_RefuseChatSupport()
        {
            //arrange
            this.mockShiftManager.Setup(x => x.OfficeHoursActive()).Returns(false); 

            //act
            var response = this.chatSupportController.Get() as ObjectResult;
            var responseValue = response.Value as JsonResult;

            // Assert
            Assert.IsInstanceOf<ObjectResult>(response);
            Assert.That(response.StatusCode.Value, Is.EqualTo(StatusCodes.Status503ServiceUnavailable));
            Assert.That(responseValue.Value, Is.EqualTo("Cannot support new chats after office hours"));
        }

        [Test]
        public void NewChatRequest_WithInOfficeHrs_QueueIsFull_OverflowCannotActivate_RefuseChatSupport()
        {
            //arrange
            this.mockShiftManager.Setup(x => x.OfficeHoursActive()).Returns(true);
            this.mockShiftManager.Setup(x => x.ChatQueueIsFull()).Returns(true);
            this.mockShiftManager.Setup(x => x.CanStartOverflow()).Returns(false);
            //act
            var response = this.chatSupportController.Get() as ObjectResult;
            var responseValue = response.Value as JsonResult;

            // Assert
            Assert.IsInstanceOf<ObjectResult>(response);
            Assert.That(response.StatusCode.Value, Is.EqualTo(StatusCodes.Status503ServiceUnavailable));
            Assert.That(responseValue.Value, Is.EqualTo("Cannot support new chats all agents are busy"));
        }

        [Test]
        public void NewChatRequest_WithInOfficeHrs_QueueIsFull_OverflowActive_OverflowIsFull_RefuseChatSupport()
        {
            //arrange
            this.mockShiftManager.Setup(x => x.OfficeHoursActive()).Returns(true);
            this.mockShiftManager.Setup(x => x.ChatQueueIsFull()).Returns(true);
            this.mockShiftManager.Setup(x => x.CanStartOverflow()).Returns(true);
            this.mockShiftManager.Setup(x => x.OverflowIsFull()).Returns(true);

            //act
            var response = this.chatSupportController.Get() as ObjectResult;
            var responseValue = response.Value as JsonResult;

            // Assert
            Assert.IsInstanceOf<ObjectResult>(response);
            Assert.That(response.StatusCode.Value, Is.EqualTo(StatusCodes.Status503ServiceUnavailable));
            Assert.That(responseValue.Value, Is.EqualTo("Cannot support new chats all agents are busy"));
        }

        [Test]
        public void NewChatRequest_WithInOfficeHrs_QueueIsFull_OverflowActive_OverflowNotFull_AcceptChatSession()
        {
            //arrange
            this.mockShiftManager.Setup(x => x.OfficeHoursActive()).Returns(true);
            this.mockShiftManager.Setup(x => x.ChatQueueIsFull()).Returns(true);
            this.mockShiftManager.Setup(x => x.CanStartOverflow()).Returns(true);
            this.mockShiftManager.Setup(x => x.OverflowIsFull()).Returns(false);

            //act
            var response = this.chatSupportController.Get() as OkResult; 

            // Assert
            Assert.IsInstanceOf<OkResult>(response); 
        }

        [Test]
        public void NewChatRequest_WithInOfficeHrs_QueueIsNotFull_AcceptChatSession()
        {
            //arrange
            this.mockShiftManager.Setup(x => x.OfficeHoursActive()).Returns(true);
            this.mockShiftManager.Setup(x => x.ChatQueueIsFull()).Returns(false);
            this.mockShiftManager.Setup(x => x.CanStartOverflow()).Returns(true); 

            //act
            var response = this.chatSupportController.Get() as OkResult;

            // Assert
            Assert.IsInstanceOf<OkResult>(response);
        }
    }
}
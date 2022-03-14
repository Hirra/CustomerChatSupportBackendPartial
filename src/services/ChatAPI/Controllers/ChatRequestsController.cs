using ChatAPI.Models;
using ChatAPI.Utilites.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QueuePublishers.Interfaces;
using System.Threading.Tasks;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRequestsController : ControllerBase
    { 
        private readonly IChatSupport chatSupport;
        private readonly IQueuePublisher publisher;
        private readonly ILogger<ChatRequestsController> logger;

        public ChatRequestsController(IChatSupport chatSupport, IQueuePublisher publisher, ILogger<ChatRequestsController> logger)
        {
            this.chatSupport = chatSupport;
            this.publisher = publisher;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatSession chatSession)
        {
            if (!await this.chatSupport.RefuseNewChats())
            {
                publisher.Publish(chatSession);
                this.logger.LogDebug("Chat session queues");
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new JsonResult("No agents available at the moment"));
        } 
    }
}

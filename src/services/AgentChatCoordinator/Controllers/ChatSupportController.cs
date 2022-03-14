using AgentChatCoordinator.Services.Interfaces;
using DataAcccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgentChatCoordinator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatSupportController : ControllerBase
    {
        private readonly IShiftManagerService supportShiftManager; 
        public ChatSupportController(IShiftManagerService agentManager)
        {
            this.supportShiftManager = agentManager;
        }

        [HttpGet("CanAcceptNewChats")]
        public IActionResult Get()
        {
            if(!this.supportShiftManager.OfficeHoursActive()) 
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new JsonResult("Cannot support new chats after office hours"));

            if (this.supportShiftManager.ChatQueueIsFull() && !this.supportShiftManager.CanStartOverflow())
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new JsonResult("Cannot support new chats all agents are busy"));

            if (this.supportShiftManager.CanStartOverflow() && this.supportShiftManager.OverflowIsFull()) 
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, new JsonResult("Cannot support new chats all agents are busy"));
                
            return Ok();
        }
    }
}

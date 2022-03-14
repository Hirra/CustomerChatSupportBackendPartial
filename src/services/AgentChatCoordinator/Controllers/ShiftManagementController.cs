using AgentChatCoordinator.Services.Interfaces;
using DataAcccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentChatCoordinator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftManagementController : ControllerBase
    {
        private readonly IAgentsManagerService agentManager;
        public ShiftManagementController(IAgentsManagerService agentManager)
        {
            this.agentManager = agentManager;
        }

        [HttpPost("AssignTeam")]
        public IActionResult Post([FromBody] IEnumerable<Agent> agents)
        {
            if (agents is null || !agents.Any())
                return BadRequest();

            if(this.agentManager.AssignTeam(agents))
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError, new JsonResult("Something broken"));
        }

        [HttpPost("GetActiveTeam")]
        public IActionResult Get()
        {
            var team = this.agentManager.GetActiveTeam();
            if(team != null )
                return Ok(team);

            return StatusCode(StatusCodes.Status500InternalServerError, new JsonResult("Something broken"));
        }
    }
}

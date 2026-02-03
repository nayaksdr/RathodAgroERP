using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/ai-agent")]
    public class AiAgentController : ControllerBase
    {
        private readonly AiAgentService _agent;

        public AiAgentController(AiAgentService agent)
        {
            _agent = agent;
        }

        // 🔹 Ask AI
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] QuestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Question))
            {
                return BadRequest(new
                {
                    answer = "कृपया प्रश्न टाइप करा किंवा बोला."
                });
            }

            var answer = await _agent.AskAsync(request.Question);

            return Ok(new
            {
                question = request.Question,
                answer
            });
        }
    }
}


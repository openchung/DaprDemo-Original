using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DaprClientDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PubSubController : ControllerBase
    {
        private readonly ILogger<PubSubController> _logger;
        public PubSubController(ILogger<PubSubController> logger)
        {
            _logger = logger;
        }

        [Topic("pubsub", "topicStatus")]
        [HttpPost("tstatus")]
        public async Task<ActionResult> Sub()
        {
            Stream stream = Request.Body;
            byte[] buffer = new byte[Request.ContentLength.Value];
            stream.Position = 0L;
            stream.ReadAsync(buffer, 0, buffer.Length);
            string content = Encoding.UTF8.GetString(buffer);
            _logger.LogInformation("topicStatus" + content);
            return Ok(content);
        }
    }
}

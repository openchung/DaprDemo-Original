using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace DaprClientDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        public StateController(ILogger<StateController> logger, DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var result = await _daprClient.GetStateAsync<string>("statestore", "guid");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync()
        {
            await _daprClient.SaveStateAsync<string>("statestore", "guid", Guid.NewGuid().ToString(), new StateOptions() { Consistency = ConsistencyMode.Strong });
            return Ok("done");
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync()
        {
            await _daprClient.DeleteStateAsync("statestore", "guid");
            return Ok("done");
        }

        [HttpPost("tag")]
        public async Task<ActionResult> PostWithTagAsync()
        {
            var (value, etag) = await _daprClient.GetStateAndETagAsync<string>("statestore", "guid");
            await _daprClient.TrySaveStateAsync<string>("statestore", "guid", Guid.NewGuid().ToString(), etag);
            return Ok("done");
        }

        [HttpDelete("tag")]
        public async Task<ActionResult> DeleteWithTagAsync()
        {
            var (value, etag) = await _daprClient.GetStateAndETagAsync<string>("statestore", "guid");
            return Ok(await _daprClient.TryDeleteStateAsync("statestore", "guid", etag));
        }
    }
}


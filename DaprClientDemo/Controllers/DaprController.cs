using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DaprClientDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaprController : ControllerBase
    {
        private readonly DaprClient _daprClient;
        public DaprController(DaprClient daprClient)
        {

            _daprClient = daprClient;
        }

        [HttpGet("HttpClient")]
        public async Task<ActionResult> GetHttpClientResultAsync()
        {
            using var httpClient = DaprClient.CreateInvokeHttpClient();
            var result = await httpClient.GetAsync("http://webapi/WeatherForecast");
            var resultContent = string.Format("result is {0} {1}", result.StatusCode, await result.Content.ReadAsStringAsync());
            return Ok(resultContent);
        }

        [HttpGet("DaprClient")]
        public async Task<ActionResult> GetDaprClientResultAsync()
        {
            using var daprClient = new DaprClientBuilder().Build();
            var result = await daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(HttpMethod.Get, "webapi", "WeatherForecast");
            return Ok(result);
        }

        [HttpGet("DaprClientWithDI")]
        public async Task<ActionResult> GetDaprClientWithDIResultAsync()
        {
            var result = await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(HttpMethod.Get, "webapi", "WeatherForecast");
            return Ok(result);
        }
    }
}

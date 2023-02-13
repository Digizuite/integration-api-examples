using System.Net.Http;
using AWExternalActionAITagging.Controllers.Ximilar;
using Digizuite.AutomationWorkflows;
using Microsoft.AspNetCore.Mvc;

namespace AWExternalActionAITagging.Controllers
{
    [Route("api/aw/aiservices")]
    public class AwController : ControllerBase
    {
        
        private readonly XimilarAIService _ximilarAiService;
        
        public AwController(XimilarAIService ximilarAiService)
        {
            _ximilarAiService = ximilarAiService;
        }

        [HttpPost("ximilarfashiontagging")]
        public ExternalInvocationResponseBody<IsLuckyResponse> IsLucky(
            [FromBody] ExternalInvocationRequestBody<IsLuckyRequest> request)
        {
            var isLucky = request.Arguments.Number == 7;
            return new ExternalInvocationResponseBody<IsLuckyResponse>(new IsLuckyResponse())
            {
                Passed = isLucky
            };
        }
    }

    public record IsLuckyRequest(int Number);

    public record IsLuckyResponse;
}
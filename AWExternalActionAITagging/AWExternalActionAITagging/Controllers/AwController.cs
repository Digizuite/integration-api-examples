using System.Threading.Tasks;
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
        public async Task<ExternalInvocationResponseBody<DigizuiteTagResponse>> IsLucky(
            [FromBody] ExternalInvocationRequestBody<XimilarAiTaggingRequest> request)
        {
            var detectedTags = await _ximilarAiService.XimilarFashionAiTagging(request.Arguments.AssetId);
            
            return new ExternalInvocationResponseBody<DigizuiteTagResponse>(detectedTags)
            {
                Passed = true
            };
        }
    }

    public record XimilarAiTaggingRequest(int AssetId);
}
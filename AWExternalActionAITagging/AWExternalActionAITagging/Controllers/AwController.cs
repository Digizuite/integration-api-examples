using System;
using System.Linq;
using System.Threading.Tasks;
using AWExternalActionAITagging.Controllers.Ximilar;
using Digizuite.AutomationWorkflows;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<ExternalInvocationResponseBody<DigizuiteSimpleResponse>> IsLucky(
            [FromBody] ExternalInvocationRequestBody<XimilarAiTaggingRequest> request)
        {
            var detectedTags = await _ximilarAiService.XimilarFashionAiTagging(request.Arguments.asset_id);

            var tags = detectedTags.tags.Select((t) => t.name);
            string tagsString = string.Join(", ", tags);
            
            return new ExternalInvocationResponseBody<DigizuiteSimpleResponse>(new DigizuiteSimpleResponse() { tagsstring = tagsString})
            {
                Passed = true
            };
        }
    }

    public record XimilarAiTaggingRequest(string asset_id);
}
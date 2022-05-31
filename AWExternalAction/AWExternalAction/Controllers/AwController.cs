using Digizuite.AutomationWorkflows;
using Microsoft.AspNetCore.Mvc;

namespace AWExternalAction.Controllers;

[Route("api/aw/my-wrapper")]
public class AwController : ControllerBase
{

    [HttpPost("is-lucky")]
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
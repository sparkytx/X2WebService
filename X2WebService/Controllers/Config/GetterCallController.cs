using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.Config;

[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "config")]
[ApiController]
public class GetterCallController : ControllerBase
{
    private readonly IGetterCallsAsync _callsAsync;

    public GetterCallController(IGetterCallsAsync getterCallsAsync )
    {
        _callsAsync = getterCallsAsync;
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<GetterCall>>> GetterCall()
    {
        var callsResult=  await _callsAsync.GetAllCallsAsync();
        return WebServiceExtension.ReturnWebResult(callsResult);

    }

    [HttpPut]
    public async Task<ActionResult<GetterInfo>> PutterCall(string sourceInfoName, string procName, string queryName, string options)
    {
        var queryInfo= await _callsAsync.UpdateFromSourceAsync(sourceInfoName, procName, queryName, options);
        return WebServiceExtension.ReturnWebResult(queryInfo);
    }
}
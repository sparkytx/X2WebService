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
    public async Task<IEnumerable<QueryCall>> GetterCall()
    {
        return await _callsAsync.GetAllQueryCallsAsync();
    }

    [HttpPut]
    public async Task<QueryInfo> PutterCall(string sourceInfoName, string procName, string queryName, string options)
    {
        return await _callsAsync.UpdateFromSourceAsync(sourceInfoName, procName, queryName, options);
    }
}
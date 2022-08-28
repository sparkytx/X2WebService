using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.Config;

[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "config")]
[ApiController]
public class LoaderInfoController : ControllerBase
{
    private readonly ILoaderCrudAsync _callsAsync;

    public LoaderInfoController(ILoaderCrudAsync callsAsync)
    {
        _callsAsync = callsAsync;
    }

    [HttpGet]
    public async Task<IEnumerable<QueryCall>> GetLoaderInfos()
    {
        return await _callsAsync.GetAllQueryCallsAsync();
    }
    [HttpPut]
    public async Task<QueryInfo> PutLoaderInfo(string sourceInfoName, string procName, string queryName, string options)
    {
        return await _callsAsync.UpdateFromSourceAsync(sourceInfoName, procName, queryName, options);
    }
}
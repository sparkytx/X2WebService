using System.Reflection;
using ComTech.Extensions.Core;
using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace X2WebService.Controllers.Config;

[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "config")]
[ApiController]
public class GetterInfoController : ControllerBase
{
    private readonly IGetterCrudAsync _crudAsync;
    private readonly AuthorizationProvider _authorizationProvider;

    public GetterInfoController(IGetterCrudAsync getterCallsAsync,AuthorizationProvider authorizationProvider)
    {
        _crudAsync = getterCallsAsync;
        _authorizationProvider = authorizationProvider;
    }

    [HttpGet]
    public async Task<IEnumerable<QueryInfo>> GetterInfos()
    {
        return await _crudAsync.GetAllQueryInfoAsync();
    }

    [HttpPost]
    [SwaggerResponse(403, "Unauthorized")]
    public async Task<QueryStatus> Post([FromBody] QueryInfo item, [FromQuery] string? options = "")
    {
        try
        {
            var requestParameters = new RequestParameters(options);
            if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name))
                throw new Exception("Not Authorized"); //TODO return option
            return await _crudAsync.CreateQueryInfoAsync(item);
        }
        catch (Exception ex)
        {
            //TODO return option
            throw;
        }
    }

    [HttpPut]
    public async Task<QueryStatus> PutterCall([FromBody] QueryInfo item, string? options="")
    {
        try
        {
            var requestParameters = new RequestParameters(options);
            if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name))
                throw new Exception("Not Authorized"); //TODO return option
            return await _crudAsync.UpdateQueryInfoAsync(item);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
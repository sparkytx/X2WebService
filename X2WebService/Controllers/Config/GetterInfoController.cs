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
    public async Task<ActionResult<IAsyncEnumerable<GetterInfo>>> GetterInfos()
    {
        var infosResults= await _crudAsync.GetAllInfosAsync();
        return WebServiceExtension.ReturnWebResult(infosResults);
    }

    [HttpPost]
    [SwaggerResponse(403, "Unauthorized")]
    public async Task<ActionResult<int>> Post([FromBody] GetterInfo item, [FromQuery] string? options = "")
    {
        try
        {
            var requestParameters = new RequestParameters(options);
            if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name??"Post"))
                return new BadRequestObjectResult("Not Authorized"); //TODO return option
            WebServiceExtension.CleanSwaggerJson(item);
            var id= await _crudAsync.CreateInfoAsync(item);
            return WebServiceExtension.ReturnWebResult(id);

        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }
    }

    [HttpPut]
    public async Task<ActionResult<GetterInfo>> PutterCall([FromBody] GetterInfo item, string? options="")
    {
        var requestParameters = new RequestParameters(options);
            if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name??"PutterCall"))
                return new BadRequestObjectResult("Not Authorized"); //TODO return option
            WebServiceExtension.CleanSwaggerJson(item);
            var queryInfoResult = await _crudAsync.UpdateInfoAsync(item);
            return WebServiceExtension.ReturnWebResult(queryInfoResult);
    }
}
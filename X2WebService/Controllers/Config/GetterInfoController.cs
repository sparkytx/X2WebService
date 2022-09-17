using System.Reflection;
using System.Text.Json;
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
    private readonly IGetterInfoCrudAsync _crudAsync;
    private readonly IGetterInfoReadOnlyAsync _getterCallsAsync;
    private readonly AuthorizationProvider _authorizationProvider;

    public GetterInfoController(IGetterInfoCrudAsync getterInfoAsync,IGetterInfoReadOnlyAsync getterCallsAsync, AuthorizationProvider authorizationProvider)
    {
        _crudAsync = getterInfoAsync;
        _getterCallsAsync = getterCallsAsync;
        _authorizationProvider = authorizationProvider;
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<GetterInfo>>> GetterInfos()
    {
        var infosResults= await _crudAsync.GetAllInfosAsync();
        return WebServiceExtension.ReturnWebResult(infosResults);
    }
    [HttpGet("Calls")]
    [SwaggerOperation(OperationId = "GetCalls")]
    public async Task<ActionResult<List<QueryInfo>>> GetterCalls()
    {
        try
        {
            var infosResult = await _getterCallsAsync.GetCallsAsync(); 
            if (infosResult.IsFailed)
                return new BadRequestObjectResultErrors(infosResult.Errors);
            var text = JsonSerializer.Serialize(infosResult.Value.ToList());
            List<QueryCall> obj = JsonSerializer.Deserialize<List<QueryCall>>(text);
            return Ok(infosResult.Value.ToList());
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(e.Message);
        }
        
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
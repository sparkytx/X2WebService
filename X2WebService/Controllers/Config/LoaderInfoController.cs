using System.Reflection;
using ComTech.Extensions.Core;
using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace X2WebService.Controllers.Config;

[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "config")]
[ApiController]
public class LoaderInfoController : ControllerBase
{
    private readonly ILoaderInfoCrudAsync _crudAsync;
    private readonly ILoaderInfoReadOnlyAsync _infoReadOnly;
    private readonly AuthorizationProvider _authorizationProvider;

    public LoaderInfoController(ILoaderInfoCrudAsync crudAsync, ILoaderInfoReadOnlyAsync infoReadOnly ,AuthorizationProvider authorizationProvider)
    {
        _crudAsync = crudAsync;
        _infoReadOnly = infoReadOnly;
        _authorizationProvider = authorizationProvider;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoaderInfo>>> GetLoaderInfos()
    {
       var infosResult= await _crudAsync.GetAllInfosAsync();
       return WebServiceExtension.ReturnWebResult(infosResult);
    }

    [HttpGet("Calls")]
    [SwaggerOperation(OperationId = "GetCalls")]
    public async Task<ActionResult<IEnumerable<QueryInfo>>> GetterCalls()
    {
        try
        {
            var infos = await _infoReadOnly.GetCallsAsync();
            return Ok(infos);
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(e.Message);
        }

    }

  
    
    [HttpPost]
    [SwaggerResponse(403, "Unauthorized")]
    public async Task<ActionResult<int>> Post([FromBody] LoaderInfo item, [FromQuery] string? options = "")
    {
        try
        {
            var requestParameters = new RequestParameters(options);
            if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name ?? "Post"))
                return new BadRequestObjectResult("Not Authorized"); //TODO return option
            WebServiceExtension.CleanSwaggerJson(item);
            var id = await _crudAsync.CreateInfoAsync(item);
            return WebServiceExtension.ReturnWebResult(id);

        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }
    

    [HttpPut]
    public async Task<ActionResult<LoaderInfo>> PutLoaderInfo(LoaderInfo item, string? options = "")
    {
        var requestParameters = new RequestParameters(options);
        if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name ?? "PutterCall"))
            return new BadRequestObjectResult("Not Authorized"); //TODO return option
        WebServiceExtension.CleanSwaggerJson(item);
       var info= await _crudAsync.UpdateInfoAsync(item);
       return WebServiceExtension.ReturnWebResult(info);
    }
}
﻿using System.Reflection;
using ComTech.Extensions.Core;
using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace X2WebService.Controllers.Config;

[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "config")]
[ApiController]
public class LoaderInfoController : ControllerBase
{
    private readonly ILoaderInfoCrudAsync _crudAsync;
    private readonly AuthorizationProvider _authorizationProvider;

    public LoaderInfoController(ILoaderInfoCrudAsync crudAsync, AuthorizationProvider authorizationProvider)
    {
        _crudAsync = crudAsync;
        _authorizationProvider = authorizationProvider;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoaderInfo>>> GetLoaderInfos()
    {
       var infosResult= await _crudAsync.GetAllInfosAsync();
       return WebServiceExtension.ReturnWebResult(infosResult);
    }
    
    //user are  not going to need this since they can use the autoloader
    /*
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
            return new BadRequestObjectResult(ex);
        }
    }
    */
   
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
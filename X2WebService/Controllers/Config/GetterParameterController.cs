﻿using System.Reflection;
using ComTech.Extensions.Core;
using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace X2WebService.Controllers.Config
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "config")]
    [ApiController]
    public class GetterParameterController : Controller
    {
        private readonly IQueryCrudAsync _crudAsync;
        private readonly AuthorizationProvider _authorizationProvider;

        public GetterParameterController(IQueryCrudAsync getterCallsAsync, AuthorizationProvider authorizationProvider)
        {
            _crudAsync = getterCallsAsync;
            _authorizationProvider = authorizationProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetterParameterInfo>>> GetterInfos()
        {
            var r = await _crudAsync.GetAllParametersAsync();
            return WebServiceExtension.ReturnWebResult(r);
        }

        /*[HttpPost]
        [SwaggerResponse(403, "Unauthorized")]
        public async Task<ActionResult<int>> Post([FromBody] QueryInfo item, [FromQuery] string options = "")
        {
            try
            {
                var requestParameters = new RequestParameters(options);
                if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name ?? "Post"))
                    return new BadRequestObjectResult("Not Authorized"); //TODO return option
                var id = await _crudAsync.CreateInfoAsync(item);
                return new OkObjectResult(id);

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }*/

        [HttpPut]
        public async Task<ActionResult<GetterInfo>> PutterCall([FromBody] QueryParameterInfo item, string options = "")
        {
            try
            {
                var requestParameters = new RequestParameters(options);
                if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name ?? "PutterCall"))
                    return new BadRequestObjectResult("Not Authorized"); //TODO return option
                WebServiceExtension.CleanSwaggerJson(item);
                var queryInfoResult = await _crudAsync.UpdateParameterAsync(item);
                return WebServiceExtension.ReturnWebResult(queryInfoResult);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
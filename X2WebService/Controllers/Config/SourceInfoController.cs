using System.Reflection;
using ComTech.Common;
using ComTech.Extensions.Core;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace X2WebService.Controllers.Config
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "config")]
    [ApiController]
    public class SourceInfoController :ControllerBase
    {
        private ISourceCrudAsync _crudAsync;
        private readonly AuthorizationProvider _authorizationProvider;

        public SourceInfoController(ISourceCrudAsync crudAsync,AuthorizationProvider authorizationProvider)
        {
            _crudAsync = crudAsync;
            _authorizationProvider = authorizationProvider;
        }

        [HttpGet]
        [SwaggerOperation(OperationId = "GetMultiInfo")]
        public async Task<ActionResult<IEnumerable<SourceInfo>>> GetterInfos()
        {
            var r = await _crudAsync.GetAllInfosAsync();
            return WebServiceExtension.ReturnWebResult(r);
        }
        /*[HttpGet]
        [SwaggerOperation(OperationId="GetSingleInfo")]
        public async Task<ActionResult<SourceInfo>> GetterInfo(string name)
        {
            var r = await _crudAsync.GetInfoAsync(name);
            return WebServiceExtension.ReturnWebResult(r);
        }*/


        [HttpPost]
        [SwaggerResponse(403, "Unauthorized")]
        public async Task<ActionResult<int>> Post([FromBody] SourceInfo item, [FromQuery] string? options = "")
        {
            try
            {
                var requestParameters = new RequestParameters(options);
                if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name ?? "Post"))
                    return new BadRequestObjectResult("Not Authorized"); //TODO return option
                WebServiceExtension.CleanSwaggerJson(item);
                if (string.IsNullOrEmpty(item.Type))
                    item.Type = "SQL";
                var id = await _crudAsync.CreateInfoAsync(item);
                return WebServiceExtension.ReturnWebResult(id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult<SourceInfo>> PutterCall([FromBody] SourceInfo item, string? options = "")
        {
            var requestParameters = new RequestParameters(options);
            if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name ?? "PutterCall"))
                return new BadRequestObjectResult("Not Authorized"); //TODO return option
            WebServiceExtension.CleanSwaggerJson(item);
            var queryInfoResult = await _crudAsync.UpdateInfoAsync(item);
            return WebServiceExtension.ReturnWebResult(queryInfoResult);
        }
    }
}

using System.Reflection;
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
        private readonly IGetterParameterInfoCrudAsync _crudAsync;
        private readonly AuthorizationProvider _authorizationProvider;

        public GetterParameterController(IGetterParameterInfoCrudAsync getterCallsAsync, AuthorizationProvider authorizationProvider)
        {
            _crudAsync = getterCallsAsync;
            _authorizationProvider = authorizationProvider;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetterParameterInfo>>> GetterInfos()
        {
            var r = await _crudAsync.GetAllInfosAsync();
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
                    return BadRequest("Not Authorized"); //TODO return option
                var id = await _crudAsync.CreateInfoAsync(item);
                return new OkObjectResult(id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }*/

        [HttpPut]
        public async Task<ActionResult<GetterInfo>> PutterCall([FromBody] GetterParameterInfo item, string? options = "")
        {
            try
            {
                var requestParameters = new RequestParameters(options);
                if (!_authorizationProvider.Authorized(requestParameters.Get("IAM"), MethodBase.GetCurrentMethod()?.Name ?? "PutterCall"))
                    return BadRequest("Not Authorized"); //TODO return option
                WebServiceExtension.CleanSwaggerJson(item);
                var queryInfoResult = await _crudAsync.UpdateInfoAsync(item);
                return WebServiceExtension.ReturnWebResult(queryInfoResult);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

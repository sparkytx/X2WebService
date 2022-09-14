using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.Config
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "config")]
    [ApiController]
    public class LoaderParameterController : ControllerBase
    {
        private readonly IInfoCrudAsync<LoaderParameterInfo> _loaderAsync;

        public LoaderParameterController(IInfoCrudAsync<LoaderParameterInfo> loaderAsync)
        {
            _loaderAsync = loaderAsync;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaderParameterInfo>>> GetLoaderParameters()
        {
            var parameterResults= await _loaderAsync.GetAllInfosAsync();
            return WebServiceExtension.ReturnWebResult(parameterResults);
        }
        [HttpPut]
        public async Task<ActionResult<LoaderParameterInfo>> PutLoaderInfo(LoaderParameterInfo item)
        {
            WebServiceExtension.CleanSwaggerJson(item);
            var parameterResults= await _loaderAsync.UpdateInfoAsync(item);
            return WebServiceExtension.ReturnWebResult(parameterResults);
        }
    }
}

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
        private readonly ILoaderParameterCrudAsync _loaderAsync;

        public LoaderParameterController(ILoaderParameterCrudAsync loaderAsync)
        {
            _loaderAsync = loaderAsync;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaderParameterInfo>>> GetLoaderParameters()
        {
            var parameterResults= await _loaderAsync.GetAllParametersAsync();
            return WebServiceExtension.ReturnWebResult(parameterResults);
        }
        [HttpPut]
        public async Task<ActionResult<LoaderParameterInfo>> PutLoaderInfo(LoaderParameterInfo item)
        {
            WebServiceExtension.CleanSwaggerJson(item);
            var parameterResults= await _loaderAsync.UpdateParameterAsync(item);
            return WebServiceExtension.ReturnWebResult(parameterResults);
        }
    }
}

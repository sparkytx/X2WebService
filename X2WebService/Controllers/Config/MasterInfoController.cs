using ComTech.X2.Common;
using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.Config
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "main")]
    [ApiController]
    public class MasterInfoController : ControllerBase
    {
        private readonly IGetterInfoCrudAsync _getterInfoAsync;
        private readonly IGetterParameterInfoCrudAsync _getterParameterInfoAsync;
        private readonly ILoaderInfoCrudAsync _loaderInfoAsync;
        private readonly ILoaderParameterInfoCrudAsync _loaderParameterInfoAsync;

        public MasterInfoController(IGetterInfoCrudAsync getterInfoAsync,
            IGetterParameterInfoCrudAsync getterParameterInfoCrudAsync, ILoaderInfoCrudAsync loaderInfoAsync,
            ILoaderParameterInfoCrudAsync loaderParameterInfoCrudAsync)
        {
            _getterInfoAsync = getterInfoAsync;
            _getterParameterInfoAsync = getterParameterInfoCrudAsync;
            _loaderInfoAsync = loaderInfoAsync;
            _loaderParameterInfoAsync = loaderParameterInfoCrudAsync;
        }

        [HttpGet]
        public async Task<ActionResult<MasterInfo>> GetMasterInfo()
        {
            var masterInfo = new MasterInfo();
            var getterInfosResults = await _getterInfoAsync.GetAllInfosAsync();
            if (getterInfosResults.IsFailed)
                return Ok(masterInfo);
            masterInfo.GetterInfos = getterInfosResults.Value.Cast<GetterInfo>();
            var getterParameterInfosResults = await _getterParameterInfoAsync.GetAllInfosAsync();
            if (getterParameterInfosResults.IsFailed)
                return Ok(masterInfo);
            masterInfo.GetterParameterInfos = getterParameterInfosResults.Value.Cast<GetterParameterInfo>();
            var loaderInfosResults = await _loaderInfoAsync.GetAllInfosAsync();
            if (loaderInfosResults.IsFailed)
                return Ok(masterInfo);
            masterInfo.LoaderInfos = loaderInfosResults.Value.Cast<LoaderInfo>();
            var loaderParameterInfosResults = await _loaderParameterInfoAsync.GetAllInfosAsync();
            if (loaderParameterInfosResults.IsFailed)
                return Ok(masterInfo);
            masterInfo.LoaderParameterInfos = loaderParameterInfosResults.Value.Cast<LoaderParameterInfo>();
            return Ok(masterInfo);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] MasterInfo item)
        {
            var statusCode = 1;
            foreach (var info in item.GetterInfos)
            {
                var result = await _getterInfoAsync.CreateInfoAsync(info);
                if (result.IsFailed)
                    statusCode = 0;
            }

            foreach (var info in item.GetterParameterInfos)
            {
                var result = await _getterParameterInfoAsync.CreateInfoAsync(info);
                if (result.IsFailed)
                    statusCode = 0;
            }

            foreach (var info in item.LoaderInfos)
            {
                var result = await _loaderInfoAsync.CreateInfoAsync(info);
                if (result.IsFailed)
                    statusCode = 0;
            }

            foreach (var info in item.LoaderParameterInfos)
            {
                var result = await _loaderParameterInfoAsync.CreateInfoAsync(info);
                if (result.IsFailed)
                    statusCode = 0;
            }

            return statusCode;
        }
    }
}

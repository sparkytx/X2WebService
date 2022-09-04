using System.Reflection.Metadata.Ecma335;
using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.Config;

    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "config")]
    [ApiController]
    public class LoaderCallController : ControllerBase
    {
        private readonly ILoaderCallsAsync _callsAsync;

        public LoaderCallController(ILoaderCallsAsync callsAsync)
        {
            _callsAsync = callsAsync;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaderCall>>> GetLoaderCall()
        {
            var calls= await  _callsAsync.GetAllCallsAsync();
            return WebServiceExtension.ReturnWebResult<IEnumerable<QueryCall>>(calls);
        }
        [HttpPut]
        public async Task<ActionResult<LoaderInfo>> PutLoaderCall(string sourceInfoName, string procName, string queryName, string options)
        {
            var queryInfoResult= await _callsAsync.UpdateFromSourceAsync(sourceInfoName, procName, queryName, options);
            return WebServiceExtension.ReturnWebResult(queryInfoResult);
        }
}
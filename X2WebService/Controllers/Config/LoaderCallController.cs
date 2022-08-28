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
        public async Task<IEnumerable<QueryCall>> GetLoaderCall()
        {
            return await  _callsAsync.GetAllQueryCallsAsync();
        }
        [HttpPut]
        public async Task<QueryInfo> PutLoaderCall(string sourceInfoName, string procName, string queryName, string options)
        {
            return await _callsAsync.UpdateFromSourceAsync(sourceInfoName, procName, queryName, options);
        }
}
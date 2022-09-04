using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.Main;

[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "main")]
[ApiController]
public class CacheController : Controller
    {
        private readonly IQueryParameterCrudAsync _getterCrudAsync;

        public CacheController(IQueryParameterCrudAsync getterCrudAsync)
        {
            _getterCrudAsync = getterCrudAsync;
        }

    [HttpPut] 
    public async Task RefreshCache()
    {
             await  _getterCrudAsync.RefreshCacheAsync();
     }
    }


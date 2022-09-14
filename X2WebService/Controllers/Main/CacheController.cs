using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.Main;

[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "main")]
[ApiController]
public class CacheController : Controller
    {
        private readonly IInfoCrudAsync<QueryParameterInfo> _getterCrudAsync;

        public CacheController(IInfoCrudAsync<QueryParameterInfo> getterCrudAsync)
        {
            _getterCrudAsync = getterCrudAsync;
        }

    [HttpPut] 
    public async Task RefreshCache()
    {
             await  _getterCrudAsync.RefreshInfoAsync();
     }
    }


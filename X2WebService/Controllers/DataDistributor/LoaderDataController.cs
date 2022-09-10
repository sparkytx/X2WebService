using System.Net;
using ComTech.Common;
using ComTech.SqlDataRepo;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;

namespace X2WebService.Controllers.DataDistributor
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Data")]
    [ApiController]
    public class LoaderDataController : ControllerBase
    {
        private readonly ILoaderInfoReadOnlyAsync _infoReadOnlyAsync;
        private readonly IDataProviderAsync _dataProviderAsync;

        public LoaderDataController(ILoaderInfoReadOnlyAsync infoReadOnlyAsync,IDataProviderAsync dataProviderAsync)
        {
            _infoReadOnlyAsync = infoReadOnlyAsync;
            _dataProviderAsync = dataProviderAsync;
        }
        public async Task<IActionResult> Post([FromBody] X2DataMessage x2DataMessage,[FromQuery]string options=null)
        {
            var usageLogEntry= RequestPropertyHelper.CreateUsageEntry(Request);
            usageLogEntry.UrlString = "/Loader";
            usageLogEntry.Command = x2DataMessage.Name;
            usageLogEntry.SystemName = "X2";
            usageLogEntry.AppName = "WebService";
            usageLogEntry.LogId = Guid.NewGuid().ToString();
            try
            {
                usageLogEntry.Count = x2DataMessage.RowCount;
                var queryDefinitionResult = await _infoReadOnlyAsync.GetQueryDefinitionAsync(x2DataMessage.Name);
                if (queryDefinitionResult.IsFailed)
                    return new BadRequestObjectResult(queryDefinitionResult.Errors);
                x2DataMessage.Name = usageLogEntry.LogId;
                return new OkObjectResult(await _dataProviderAsync.LoadData(x2DataMessage, queryDefinitionResult.Value));
            }
            catch (ArgumentException ex)
            {
                usageLogEntry.Success = false;
                usageLogEntry.ResponseMessage = ex.Message;
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                var status = new ContentResult
                {
                    StatusCode = HttpStatusCode.InternalServerError.GetHashCode(),
                    Content = ex.Message
                };
                return status;
            }

        }
    }
}

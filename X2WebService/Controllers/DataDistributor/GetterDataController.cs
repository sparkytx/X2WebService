using System.Data.SqlClient;
using System.Net;
using System.Text;
using ComTech.Common;
using ComTech.SqlDataRepo;
using ComTech.X2.Common.Config;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace X2WebService.Controllers.DataDistributor
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "main")]
    [ApiController]
    public class GetterDataController : ControllerBase
    {
        private readonly ISourceReadOnly _sourceReadOnly;
        private readonly IGetterInfoReadOnlyAsync _getterInfo;
        private readonly IDataProviderAsync _dataProviderAsync;

        public GetterDataController(ISourceReadOnly sourceReadOnly  ,IGetterInfoReadOnlyAsync getterInfo, IDataProviderAsync dataProviderAsync)
        {
            _sourceReadOnly = sourceReadOnly;
            _getterInfo = getterInfo;
            _dataProviderAsync = dataProviderAsync;
        }

        [HttpGet("{name}/{*parameters}")]
        [SwaggerOperation(OperationId = "GetData")]
        public async Task<IActionResult> Get(string name, string? parameters=" ", [FromQuery] string? format = "TEXT")
        {
            var usageLogEntry = RequestPropertyHelper.CreateUsageEntry(Request);
            usageLogEntry.Command = name;
            usageLogEntry.SystemName = "X2";
            usageLogEntry.AppName = "WebService";
            usageLogEntry.LogId = Guid.NewGuid().ToString();
            try
            {

                usageLogEntry.AppName = name;
                var curveDefintionResult = await _getterInfo.GetQueryDefinitionAsync(name);
                if (curveDefintionResult.IsFailed)
                    return NotFound();
                var parameterStrings = parameters==null?null: parameters.Trim().Split("~");
                var dataResponse = await _dataProviderAsync.ExecSqlQueryAsync(curveDefintionResult.Value.Info.Query,curveDefintionResult.Value.SourceInfo,curveDefintionResult.Value.Parameters,parameterStrings);
                if (dataResponse.IsFailed)
                {
                    return BadRequest(string.Join(";", dataResponse.Errors.ToList().Select(e => e.Message)));
                }
                    
                usageLogEntry.Count = dataResponse.Value.RowCount;
                dataResponse.Value.Name = name;
                return FormatResponseDataContent(format, dataResponse.Value);
                //TODO log

            }
            catch (ArgumentException ex)
            {
                usageLogEntry.Success = false;
                usageLogEntry.ResponseMessage = ex.Message;
                return BadRequest(ex.Message);
            }
            catch (TimeoutException te)
            {
                usageLogEntry.Success = false;
                usageLogEntry.ResponseMessage = te.Message;
                var status = new ContentResult
                {
                    StatusCode = HttpStatusCode.InternalServerError.GetHashCode(),
                    Content = te.Message
                };
                return status;
            }
            catch (SqlException ex)
            {
                var message = ex.Message;
                var statusCode = HttpStatusCode.InternalServerError.GetHashCode();
                if (ex.Errors[0].Number is 596 or 10054)
                {
                    message = "Request was terminated most like due to long running query";
                    statusCode = HttpStatusCode.RequestTimeout.GetHashCode();
                }

                usageLogEntry.Success = false;
                usageLogEntry.ResponseMessage = ex.Message;
                var status = new ContentResult
                {
                    StatusCode = HttpStatusCode.InternalServerError.GetHashCode(),
                    Content = ex.Message
                };
                return status;
            }
            catch (Exception ex)
            {
                usageLogEntry.Success = false;
                usageLogEntry.ResponseMessage = ex.Message;
                var status = new ContentResult
                {
                    StatusCode = HttpStatusCode.InternalServerError.GetHashCode(),
                    Content = ex.Message
                };
                return status;
            }
        }


        [HttpPost]
        [SwaggerOperation(OperationId = "GetDataFromQuery")]
        public async Task<IActionResult> Post([FromBody] string queryString, string sourceInfoName)
        {
            var usageLogEntry = RequestPropertyHelper.CreateUsageEntry(Request);
            try
            {
                usageLogEntry.Properties.Add(new KeyValuePair<string, string>("Query", queryString));
                usageLogEntry.Command = "/Adhoc";
                usageLogEntry.AppName = sourceInfoName;
                var sourceInfoResult = await _sourceReadOnly.GetInfoAsync(sourceInfoName);
                if (sourceInfoResult.IsFailed)
                    return new BadRequestErrors(sourceInfoResult.Errors);
                var  dataResponse= _dataProviderAsync.ExecSqlQueryAsync(queryString,sourceInfoResult.Value);
               return new OkObjectResult(dataResponse);
            }
            catch (Exception ex)
            {
                usageLogEntry.Success = false;
                usageLogEntry.ResponseMessage = ex.Message;
                var status = new ContentResult
                {
                    StatusCode = HttpStatusCode.InternalServerError.GetHashCode(),
                    Content = ex.Message
                };
                return status;
            }
        }

        private IActionResult FormatResponseDataContent(string format, X2DataMessage message)
        {
            string content;
            if (format.Equals("TEXT", StringComparison.CurrentCultureIgnoreCase))
            {
                content = message.FormatAsText();
                return base.Content(content, "text/plain", Encoding.UTF8);
            }
            if (format.Equals("HTML", StringComparison.CurrentCultureIgnoreCase))
            {
                content = message.FormatAsHtml();
                return base.Content(content, "text/HTML", Encoding.UTF8);
            }
            if (format.Equals("JX2", StringComparison.CurrentCultureIgnoreCase))
            {
                content = message.FormatAsJx2();
                return base.Content(content, "application/json", Encoding.UTF8);
            }
            if (format.Equals("Json", StringComparison.CurrentCultureIgnoreCase))
            {
                content = message.FormatAsJson();
                return base.Content(content, "application/json", Encoding.UTF8);
            }

            return BadRequest($"{format} is not a supported format");
        }
    }
}

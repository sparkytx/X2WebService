using ComTech.Common;

namespace X2WebService.Controllers.DataDistributor;

public class RequestPropertyHelper
{
    public static UsageLogEntry CreateUsageEntry(HttpRequest request)
    {
        var properties = request.Headers.Select(h => new KeyValuePair<string, string>(h.Key, h.Value));
        var result = LogPropertyAdapter.FromRequestProperties(properties);
        if (request.Path.HasValue) result.UrlString = "https://" + request.Host + request.Path.Value;
        return result;
    }
}
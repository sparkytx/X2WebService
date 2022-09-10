using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using FluentResults;
using LanguageExt.Pipes;
using static System.Security.Principal.WindowsIdentity;

namespace X2WebService.Models
{
    public class HomeViewModel
    {

        public IList<string> Values = new List<string>();
        public string VersionString  {get;}
        public string UserName { get; }
        public string BaseUrl { get; set; }
        public List<QueryCall> QueryCalls { get; set; }

        public HomeViewModel(IGetterInfoReadOnlyAsync infoReadOnlyAsync)
        {
            for (int i = 0; i < 10; i++) Values.Add("");
            try
            {
                var QueryCallsResult = infoReadOnlyAsync.GetCallsAsync().Result;
                if (QueryCallsResult.IsFailed)
                    throw new Exception(QueryCallsResult.Errors.First().ToString());
                QueryCalls = QueryCallsResult.Value.ToList();
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                UserName =  GetCurrent().Name;
            }
            catch (Exception ex)
            {
                QueryCalls = new List<QueryCall>();
                throw;
            }
        }

    }
}

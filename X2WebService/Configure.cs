using System.Buffers;
using System.Text.Json;
using Autofac;
using ComTech.Common;
using ComTech.Extensions.Core;
using ComTech.SqlDataRepo;
using ComTech.X2.Common;
using ComTech.X2.Common.Config;
using Microsoft.Net.Http.Headers;

namespace X2WebService;

public static class Configure
{
    public static void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<GetterInfoCrudAsync>().As<IGetterInfoCrudAsync>().As<IGetterInfoReadOnlyAsync>();
        containerBuilder.RegisterType<GetterParameterCrudAsync>().As<IGetterParameterInfoCrudAsync>();
        containerBuilder.RegisterType<LoaderInfoCrudAsync>().As<ILoaderInfoCrudAsync>().As<ILoaderInfoReadOnlyAsync>();
        containerBuilder.RegisterType<LoaderParameterCrudAsync>().As<ILoaderParameterInfoCrudAsync>();
        containerBuilder.RegisterType<SourceCrudAsync>().As<ISourceInfoCrudAsync>().As<ISourceReadOnly>().SingleInstance();
        containerBuilder.RegisterType<DataProviderAsync>().As<IDataProviderAsync>();
        containerBuilder.RegisterType<SqlDatabaseProvider>().As<ISqlDatabaseProvider>(); 
        containerBuilder.RegisterType<AuthorizationProvider>();
        containerBuilder.RegisterType<AuthorizationRepo>().As<IAuthorizationRepo>();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy=null);
    }

   

}
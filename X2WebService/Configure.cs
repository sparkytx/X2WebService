using Autofac;
using ComTech.Extensions.Core;
using ComTech.SqlDataRepo;
using ComTech.X2.Common.Config;

namespace X2WebService;

public static class Configure
{
    public static void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<GetterInfoCrudAsync>().As<IGetterInfoCrudAsync>().As<IGetterInfoReadOnlyAsync>();
        containerBuilder.RegisterType<GetterParameterCrudAsync>().As<IGetterParameterCrudAsync>();
        containerBuilder.RegisterType<LoaderInfoCrudAsync>().As<ILoaderInfoCrudAsync>().As<ILoaderInfoReadOnlyAsync>();
        containerBuilder.RegisterType<LoaderParameterCrudAsync>().As<ILoaderParameterCrudAsync>();
        containerBuilder.RegisterType<SourceCrudAsync>().As<ISourceCrudAsync>().As<ISourceReadOnly>().SingleInstance();
        containerBuilder.RegisterType<DataProviderAsync>().As<IDataProviderAsync>();
        containerBuilder.RegisterType<SqlDatabaseProvider>().As<ISqlDatabaseProvider>(); 
        containerBuilder.RegisterType<AuthorizationProvider>();
        containerBuilder.RegisterType<AuthorizationRepo>().As<IAuthorizationRepo>();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
    }

}
using Autofac;
using ComTech.Extensions.Core;
using ComTech.SqlDataRepo;
using ComTech.X2.Common.Config;

namespace X2WebService;

public static class Configure
{
    public static void ConfigureContainer(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<GetterInfoCrudAsync>().As<IGetterInfoCrudAsync>();
        containerBuilder.RegisterType<GetterParameterCrudAsync>().As<IGetterParameterCrudAsync>();
        containerBuilder.RegisterType<GetterCallsAsync>().As<IGetterCallsAsync>();
        containerBuilder.RegisterType<LoaderInfoCrudAsync>().As<ILoaderInfoCrudAsync>();
        containerBuilder.RegisterType<LoaderParameterCrudAsync>().As<ILoaderParameterCrudAsync>();
        containerBuilder.RegisterType<LoaderCallsAsync>().As<ILoaderCallsAsync>();
        containerBuilder.RegisterType<SourceCrudAsync>().As<ISourceCrudAsync>().As<ISourceReadOnly>().SingleInstance();
        containerBuilder.RegisterType<SqlDatabaseProvider>().As<ISqlDatabaseProvider>(); 
        containerBuilder.RegisterType<AuthorizationProvider>();
        containerBuilder.RegisterType<AuthorizationRepo>().As<IAuthorizationRepo>();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
    }

}
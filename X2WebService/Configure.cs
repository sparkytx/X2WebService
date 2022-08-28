using Autofac;

namespace X2WebService;

public static class Configure
{
    public static void ConfigureContainer(ContainerBuilder containerBuilder)
    {
      //  containerBuilder.RegisterType<SqlDataProviderAsync>().As<ISqlDataProviderAsync>();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
    }

}
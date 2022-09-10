using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using X2WebService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(Configure.ConfigureContainer)
    .ConfigureServices(Configure.ConfigureServices);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.EnableAnnotations();
        c.SwaggerDoc("main",new OpenApiInfo{Title = "X2 Main"});
        c.SwaggerDoc("getter", new OpenApiInfo { Title = "X2 Getter" });
        c.SwaggerDoc("loader", new OpenApiInfo { Title = "X2 Loader" });
        c.SwaggerDoc("config", new OpenApiInfo { Title = "X2 Config" });
    }

);

var app = builder.Build();


    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/main/swagger.json","X2 Main");
        c.SwaggerEndpoint("/swagger/getter/swagger.json", "X2 Getter");
        c.SwaggerEndpoint("/swagger/loader/swagger.json", "X2 Loader");
        c.SwaggerEndpoint("/swagger/config/swagger.json", "X2 Config");
        c.DocExpansion(DocExpansion.None);
       // c.RoutePrefix = string.Empty;
    });


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRouting();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapControllerRoute("home", "{controller=Home}/{Action=Index}");
        endpoints.MapControllerRoute("queryedit", "{controller=QueryEdit}/{Action=Index}");
    }
);
//app.MapControllers();

app.Run();

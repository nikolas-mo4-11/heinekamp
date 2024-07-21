using Heinekamp.Domain.AppConfig;
using Heinekamp.PgDb.Context;
using Heinekamp.PgDb.Repository;
using Heinekamp.PgDb.Repository.Interfaces;
using Heinekamp.Services;
using Heinekamp.Services.Interfaces;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Heinekamp;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        
        services.AddControllersWithViews();
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "wwwroot";
        });
        
        ConfigureAppServices(services);
        ConfigureRepositories(services);
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env, IDesignTimeDbContextFactory<PgContext> contextFactory)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        app.UseRouting();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "DefaultApi",
                pattern: "api/{controller}/{action=Index}/{id?}");

            // Обрабатывает все не совпадающие маршруты, направляя их на SPA
            //endpoints.MapFallbackToController("Index", "Home");
        });
        
        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "App";

            if (env.IsDevelopment())
            {
                //spa.UseProxyToSpaDevelopmentServer("http://localhost:9000");
                // Либо используйте следующий метод, если предпочитаете Webpack Dev Middleware
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        });
        
        // Применение миграций
        using (var context = contextFactory.CreateDbContext(null))
            context.Database.Migrate();

    }
    
    private void ConfigureRepositories(IServiceCollection services)
    {
        services.AddSingleton<IDesignTimeDbContextFactory<PgContext>, PgContextFactory>();
        
        services.AddSingleton<IDocumentRepository, DocumentRepository>();
        services.AddSingleton<IFileTypeRepository, FileTypeRepository>();
    }

    private void ConfigureAppServices(IServiceCollection services)
    {
        services.AddSingleton<IDocumentService, DocumentService>();
    }
}
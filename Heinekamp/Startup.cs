using Heinekamp.MsDb.Context;
using Heinekamp.MsDb.Context.Interfaces;
using Heinekamp.MsDb.Repository.Interfaces;
using Heinekamp.Services.Classes;
using Heinekamp.Services.Interfaces;

namespace Heinekamp;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(options => options.EnableEndpointRouting = false);
        
        ConfigureAppServices(services);
        ConfigureRepositories(services);
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseMvc();
        
        // Применение миграций
        /*using (var context = contextFactory.CreateDbContext())
        {
            context.Database.Migrate();
        }*/
    }
    
    private void ConfigureRepositories(IServiceCollection services)
    {
        services.AddSingleton<IRepositoryContextFactory, RepositoryContextFactory>(provider =>
            new RepositoryContextFactory(Configuration.GetConnectionString("MsDbConnectionString")));
        
        services.AddSingleton<IDocumentRepository>();
    }

    private void ConfigureAppServices(IServiceCollection services)
    {
        services.AddSingleton<IDocumentService, DocumentService>();
    }
}
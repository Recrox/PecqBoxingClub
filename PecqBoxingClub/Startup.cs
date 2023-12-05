using Configuration;
using Microsoft.OpenApi.Models;

namespace PecqBoxingClub.Controllers;

public class Startup
{
    public Startup(IWebHostEnvironment env, IConfiguration configuration)
    {
        Configuration = configuration;
        Environment = env;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; set; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddGlobalSettingsServices(Configuration);

        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VotreAPI", Version = "v1" });
        //});
    }

    public void Configure(IApplicationBuilder app, GlobalSettings globalSettings)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        //app.UseSwaggerUI(c =>
        //{
        //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VotreAPI v1");
        //    c.RoutePrefix = string.Empty;
        //});

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
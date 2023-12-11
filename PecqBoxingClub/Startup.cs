using Core.Repositories;
using Core.Repositories.Implementations;
using Core.Services;
using Core.Services.Implementations;
using Database;
using Microsoft.OpenApi.Models;
using System;

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
        services.AddDbContext<BoxingClubContext>();
        AddDependencyInjection(services);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddAutoMapper(typeof(Startup));
        

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOriginPolicy",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        services.AddHttpsRedirection(options =>
        {
            options.HttpsPort = 443; // Le port HTTPS
        });

        // Gérer les certificats?
        //services.AddHttpsRedirection(options =>
        //{
        //    options.HttpsPort = 443;
        //    options.SslPort = 443; // Port SSL
        //    options.UseHttps("chemin_vers_certificat.pfx", "mot_de_passe_certificat");
        //});

        //services.AddGlobalSettingsServices(Configuration);

        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VotreAPI", Version = "v1" });
        //});
    }

    private static void AddDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IMemberRepository, MemberRepository>();
    }

    public void Configure(IApplicationBuilder app)
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
        app.UseCors("AllowAnyOriginPolicy");

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseHttpsRedirection(); // Redirige toutes les requêtes HTTP vers HTTPS
    }
}
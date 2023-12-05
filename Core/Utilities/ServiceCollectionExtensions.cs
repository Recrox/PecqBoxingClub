using RamDam.BackEnd.Configuration;
using RamDam.BackEnd.Core.Identity;
using RamDam.BackEnd.Core.IdentityServer;
using RamDam.BackEnd.Core.Models.Table;
using RamDam.BackEnd.Core.Repositories;
using RamDam.BackEnd.Core.Services;
using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sieve.Services;
using Sieve.Models;
using Hangfire;
using RamDam.BackEnd.Core.Repositories.Implementations;
using RamDam.BackEnd.Core.Services;
using RamDam.BackEnd.Core.Services.Implementations;
using Hangfire.SqlServer;
using Configuration;

namespace RamDam.BackEnd.Core.Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityFramework(this IServiceCollection services, GlobalSettings globalSettings)
        {
            services.AddDbContext<RamDamContext>(
                options =>
                {
                    options.UseSqlServer(globalSettings.SqlServer.ConnectionString);
                });

         }

        public static void AddSqlServerRepositories(this IServiceCollection services)
        {
            services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
            
            // Hub
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleUserRepository, RoleUserRepository>();
            //services.AddScoped<IUserLogonRepository, UserLogonRepository>();
            services.AddScoped<IGrantRepository, GrantRepository>();
            services.AddScoped<IStoredProcedureRepository, StoredProcedureRepository>();
        }

        public static void AddBaseServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IEmailDeliveryService, SmtpEmailDeliveryService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IFavoritesService, FavoritesService>();
            services.AddScoped<IRatingService, RatingService>();
        }

        public static void AddFullServices(this IServiceCollection services)
        {
            services.AddBaseServices();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ISchedulingService, SchedulingService>();
        }

        public static GlobalSettings AddGlobalSettingsServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var globalSettings = new GlobalSettings();
            configuration.GetSection("GlobalSettings").Bind(globalSettings);
            services.AddSingleton(s => globalSettings);
            return globalSettings;
        }

        public static IIdentityServerBuilder AddCustomIdentityServerServices(
            this IServiceCollection services, IWebHostEnvironment env, GlobalSettings globalSettings)
        {
            var builder = services
                .AddIdentityServer(options =>
                {
                    options.Endpoints.EnableAuthorizeEndpoint = false;
                    options.Endpoints.EnableIntrospectionEndpoint = false;
                    options.Endpoints.EnableEndSessionEndpoint = false;
                    options.Endpoints.EnableUserInfoEndpoint = false;
                    options.Endpoints.EnableCheckSessionEndpoint = false;
                    options.Endpoints.EnableTokenRevocationEndpoint = false;
                    options.IssuerUri = globalSettings.BaseServiceUri.InternalIdentity;
                    options.Caching.ClientStoreExpiration = new TimeSpan(0, 5, 0);
                })
                .AddInMemoryCaching()
                .AddInMemoryApiResources(ApiResources.GetApiResources())
                .AddInMemoryIdentityResources(IdentityResources.GetIdentityResources())
                .AddCustomTokenRequestValidator<CustomTokenRequestValidator>()
                .AddClientStoreCache<ClientStore>();

            // DEBUG
            if (env.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential(false);
            }
            // PROD
            else if (!string.IsNullOrWhiteSpace(globalSettings.IdentityServer.CertificatePassword)
                && System.IO.File.Exists("identity.pfx"))
            {
                var identityServerCert = CoreHelpers.GetCertificate("identity.pfx",
                    globalSettings.IdentityServer.CertificatePassword);
                builder.AddSigningCredential(identityServerCert);
            }
            else
            {
                throw new Exception("No identity certificate to use.");
            }

            services.AddTransient<ClientStore>();
            services.AddTransient<ICorsPolicyService, AllowAllCorsPolicyService>();
            services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IPersistedGrantStore, PersistedGrantStore>();

            builder.AddExtensionGrantValidator<SocialGrantValidator>();
            builder.AddExtensionGrantValidator<AdminGrantValidator>();

            return builder;
        }

        public static IdentityBuilder AddCustomIdentityServices(this IServiceCollection services, GlobalSettings globalSettings)
        {
            services.AddTransient<ILookupNormalizer, LowerInvariantLookupNormalizer>();
            
            var builder = services.AddIdentityWithoutCookieAuth<User>(options =>
            {
                options.User = new UserOptions
                {
                    RequireUniqueEmail = false,
                    AllowedUserNameCharacters = null // all
                };
                options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequireLowercase = false,
                    RequiredLength = 5,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = false
                };
                options.ClaimsIdentity = new ClaimsIdentityOptions
                {
                    SecurityStampClaimType = "sstamp",
                    UserNameClaimType = JwtClaimTypes.Name,
                    UserIdClaimType = JwtClaimTypes.Subject,
                    RoleClaimType = JwtClaimTypes.Role
                };
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
            });

            builder
                .AddUserStore<UserStore>()
                //.AddRoleStore<PrivilegeStore>()
				.AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            if (globalSettings != null
                && globalSettings.Identity != null
                && globalSettings.Identity.DataProtectorTokenLifeSpan.HasValue)
            {
                services.Configure<DataProtectionTokenProviderOptions>(options =>
                {
                    options.TokenLifespan = TimeSpan.FromMinutes(globalSettings.Identity.DataProtectorTokenLifeSpan.Value);
                });
            }

            return builder;
        }

        public static void UseDefaultMiddleware(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add version information to response headers
            app.Use(async (httpContext, next) =>
            {
                httpContext.Response.OnStarting((state) =>
                {
                    httpContext.Response.Headers.Append("Server-Version", CoreHelpers.GetVersion());
                    return Task.FromResult(0);
                }, null);

                await next.Invoke();
            });
        }

        public static void AddSwaggerService(this IServiceCollection services, GlobalSettings globalSettings)
        {
            if (globalSettings.Swagger.IsActive)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RamDam API", Version = "v1" });

                    var securitySchema = new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.OAuth2,
                        Scheme = "bearer",
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Flows = new OpenApiOAuthFlows()
                        {
                            Password = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri($"{globalSettings.BaseServiceUri.Identity}/connect/token"),
                                Scopes = new Dictionary<string, string>
                                    {
                                        { "api offline_access", "RamDam-api" }
                                    }
                            },
                            ClientCredentials = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri($"{globalSettings.BaseServiceUri.Identity}/connect/token"),
                                Scopes = new Dictionary<string, string>
                                    {
                                        { "api.camera", "RamDam-api" }
                                    }
                            }
                        }
                    };
                    c.AddSecurityDefinition("Bearer", securitySchema);

                    var securityRequirement = new OpenApiSecurityRequirement();
                    securityRequirement.Add(securitySchema, new[] { "Bearer" });
                    c.AddSecurityRequirement(securityRequirement);
                });
            }
        }

        public static void UseSwaggerService(this IApplicationBuilder app, GlobalSettings globalSettings)
        {
            if (globalSettings.Swagger.IsActive)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.OAuthClientId("web");
                    c.SwaggerEndpoint($"{globalSettings.BaseServiceUri.Api}/swagger/v1/swagger.json", "RamDam-api");
                });
            }
        }
    
        public static void AddSieve(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISieveProcessor, ApplicationSieveService>();
            var section = configuration.GetSection("Sieve");
            if (section != null)
                services.Configure<SieveOptions>(section);
        }

        public static void AddHangfireService(this IServiceCollection services, GlobalSettings globalSettings)
        {
            services.AddHangfire(configuration => configuration
                 .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseSqlServerStorage(globalSettings.SqlServer.ConnectionString, new SqlServerStorageOptions
                 {
                     CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                     SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                     QueuePollInterval = TimeSpan.Zero,
                     UseRecommendedIsolationLevel = true,
                     UsePageLocksOnDequeue = true,
                     DisableGlobalLocks = true
                 }));
            services.AddHangfireServer(options =>
            {
                options.SchedulePollingInterval = TimeSpan.FromMinutes(1);
            });
        }
    }
}

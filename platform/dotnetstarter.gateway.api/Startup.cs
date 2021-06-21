using Common.Logging;
using Common.Utilities.Storage;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Reflection;
using dotnetstarter.gateway.api.Application.Commands.Core;
using dotnetstarter.gateway.api.Application.Middleware;
using dotnetstarter.gateway.api.Application.Services;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;
using dotnetstarter.gateway.infrastructure;

namespace dotnetstarter.gateway.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {



            #region CORS

            //https://docs.microsoft.com/en-us/aspnet/core/security/cors
            var allowedHeaders = new List<string>
            {
                "Content-Disposition",
                "name",
                "filename",
                "Access-Control-Allow-Origin",
                "Authorization",
                "Content-Type"
            };

            //var policy = new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder()
            //    .WithOrigins(Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:AllowedOrigins"].Split(';'))
            //    .AllowAnyMethod()
            //    .WithExposedHeaders(allowedHeaders.ToArray())
            //    .AllowAnyHeader()
            //    .AllowCredentials()
            //    .Build();

            var policy = new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder()
                .WithOrigins(Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:AllowedOrigins"].Split(';'))
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .Build();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy);
            });

            #endregion


            services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"{Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Firebase:Authentication:TokenIssuerAddress"]}{Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Firebase:Authentication:ProjectId"]}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"{Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Firebase:Authentication:TokenIssuerAddress"]}{Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Firebase:Authentication:ProjectId"]}",
                    ValidateAudience = true,
                    ValidAudience = $"{Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Firebase:Authentication:ProjectId"]}",
                    ValidateLifetime = true
                };
            });



            //Initialize Firebase SDK, uses GOOGLE_APPLICATION_CREDENTIALS env variable
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });


            services.AddMediatR(typeof(GetAPIAccessTokenCommand).Assembly);


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //configure injected db context
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<PortalContext>(options =>
                {
                    options.UseSqlServer(Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:ConnectionStrings:PortalConnection"],
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(PortalContext).GetTypeInfo().Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.CommandTimeout(100);
                        });
                },
                    ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                );

            services.AddScoped<IUserRepository, UserRepository>();


            services.AddScoped<IInternalAPIService, InternalAPIService>();

            //services.AddScoped<IUserQueries, UserQueries>();

            services.AddSingleton<ICustomLogger, CustomLoggerGCP>();
            services.AddSingleton<IStorage, GCPStorage>();

            services.AddSingleton<IStorage>((ctx) =>
            {
                return new GCPStorage(Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:GCSettings:project_id"], Configuration);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            //Apply migrations and seed data
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var pmDB = serviceScope.ServiceProvider.GetRequiredService<PortalContext>();
                pmDB.Database.Migrate();

            }

            app.UseCors("AllowSpecificOrigin");

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMiddleware(typeof(UserInfoMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Health}/{action=Get}");
            });
        }
    }
}

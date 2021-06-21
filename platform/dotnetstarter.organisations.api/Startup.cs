using Common.InternalApiHttp;
using Common.Logging;
using Common.PubSub;
using dotnetstarter.organisations.api.Application.BackgroundProcessors;
using dotnetstarter.organisations.api.Application.Commands;
using dotnetstarter.organisations.api.Application.Infrastructure.Authorization;
using dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth;
using dotnetstarter.organisations.api.Application.Infrastructure.services;
using dotnetstarter.organisations.api.Application.Middleware;
using dotnetstarter.organisations.api.Application.PubSub;
using dotnetstarter.organisations.api.Application.Queries;
using dotnetstarter.organisations.domain.AggregatesModel.DashboardAggregate;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;
using dotnetstarter.organisations.infrastructure;
using dotnetstarter.organisations.infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
using System.Text;

namespace dotnetstarter.organisations.api
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
                "filename",
                "Access-Control-Allow-Origin",
                "Authorization",
                "Content-Type"
            };

            var corsPolicy = new Microsoft.AspNetCore.Cors.Infrastructure.CorsPolicyBuilder()
                .WithOrigins(Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:AllowedOrigins"].Split(';'))
                .AllowAnyMethod()
                .WithExposedHeaders(allowedHeaders.ToArray())
                .AllowAnyHeader()
                .AllowCredentials()
                .Build();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", corsPolicy);
            });

            #endregion

            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAllAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, PermissionAnyAuthorizationHandler>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMediatR(typeof(BuildMenuCommand).Assembly);

            //configure injected db context
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<OrganisationsContext>(options =>
                {
                    options.UseSqlServer(Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:ConnectionStrings:OrganisationsConnection"],
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(OrganisationsContext).GetTypeInfo().Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.CommandTimeout(100);
                        });
                },
                    ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                );

            //decode and validate JWT tokens issued by authentication.api
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Authentication:Issuer"],
                        ValidAudience = Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Authentication:PrivateKey"]))
                    };
                });

            //validate authorized users against policies.
            services.AddSingleton<IAuthorizationHandler, ClaimTypeHandler>();

            services.AddAuthorization(options =>
            {
                //only dotnetstarter internal services are allowed to call this API.
                options.AddPolicy("SystemAdministrator",
                    policy => policy
                    .AddRequirements(new ClaimTypeRequirement("systemadministrator")));

            });

            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            #endregion

            #region Queries

            services.AddScoped<IUserQueries, UserQueries>();
            services.AddScoped<IRoleQueries, RoleQueries>();

            #endregion








            services.AddSingleton<ICustomLogger, CustomLoggerGCP>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IInternalAPIService, InternalAPIService>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<ICustomLogger, CustomLoggerGCP>();

            #region PUBSUB

            services.AddHostedService<PubSubListenerService>();

            services.AddScoped<ISubFactory, MessageProcessor>();
            services.AddScoped<ISubFactoryGrouped, GroupedMessageProcessor>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Apply migrations and seed data
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var pmDB = serviceScope.ServiceProvider.GetRequiredService<OrganisationsContext>();
                pmDB.Database.Migrate();

            }

            app.UseAuthentication();

            app.UseCors("AllowSpecificOrigin");

            // app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMiddleware(typeof(UserInfoMiddleware));

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Health}/{action=Get}");
            });
        }
    }
}

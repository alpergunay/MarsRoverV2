using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hb.MarsRover.Infrastructure.Configuration.Settings;
using Hb.MarsRover.Infrastructure.Core.Http.Filters;
using Hb.MarsRover.Infrastructure.EventBus;
using Hb.MarsRover.Infrastructure.EventBus.Abstractions;
using Hb.MarsRover.Infrastructure.EventBus.EFEventStore;
using Hb.MarsRover.Infrastructure.EventBus.EFEventStore.Services;
using Hb.MarsRover.Infrastructure.EventBus.RabbitMQ;
using HealthChecks.UI.Client;
using MarsRover.Api.Application.Modules.Infrastructure.IntegrationEvents;
using MarsRover.Api.Application.Modules.Infrastructure.Mapper;
using MarsRover.Api.Controllers;
using MarsRover.Api.Infrastructure.AutofacModules;
using MarsRover.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System;
using System.Data.Common;
using System.Reflection;

namespace MarsRover.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCustomMvc()
                .AddCustomConfiguration(Configuration)
                .AddCustomHealthChecks()
                .AddCustomSwagger(Configuration)
                .AddCustomDbContext()
                .AddCustomIntegrations()
                .AddEventBus()
                .AddAutoMapper();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new MediatorModule());
            builder.RegisterModule(new ApplicationModule(Configuration["ApplicationSettings:Persistence:ConnectionString"]));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ApiCallMiddleware>();
            var pathBase = Configuration["PATH_BASE"];
            app.UseCors("CorsPolicy");

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Ims.Api V1");
                    c.OAuthClientId("imsapiswaggerui");
                    c.OAuthAppName("API Swagger UI");
                });

            app.UseRouting();
            //ConfigureAuth(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });
            ConfigureEventBus(app);
        }
        private static void ConfigureEventBus(IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<IEventBus>();

            //eventBus.Subscribe<VehicleModelCreatedIntegrationEvent, IIntegrationEventHandler<VehicleModelCreatedIntegrationEvent>>();
            //Other integration events....
        }
    }
    internal static class CustomExtensionsMethods
    {
        private static ApplicationSettings _settings;
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            // Add framework services.
            services.AddControllers(options => options.Filters.Add(typeof(ExceptionHandlingFilter)))
                .AddApplicationPart(typeof(MarsRoverController).Assembly)
                .AddNewtonsoftJson(x =>
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(fv => fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddHttpContextAccessor();
            return services;
        }
        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            _settings = new ApplicationSettings();
            configuration.GetSection(nameof(ApplicationSettings)).Bind(_settings);
            services.Configure<ApplicationSettings>(configuration);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            return services;
        }
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder
                .AddNpgSql(
                    _settings.Persistence.ConnectionString,
                    name: "DB-check",
                    tags: new string[] { "db" });

            hcBuilder
                .AddRabbitMQ(
                    $"amqp://{_settings.ServiceBus.RabbitMQUrl}",
                    name: "rabbitmqbus-check",
                    tags: new string[] { "mqbus" });

            return services;
        }
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "IMS - Investment Management System Service HTTP API",
                    Version = "v1",
                    Description = "Service HTTP API"
                });
            });
            return services;
        }
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services)
        {
            services.AddDbContext<MarsRoverContext>((serviceProvider, options) =>
                options.UseNpgsql(_settings.Persistence.ConnectionString,
                        npgsqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                        }));

            services.AddDbContext<IntegrationEventLogContext>(options =>
                options.UseNpgsql(_settings.Persistence.ConnectionString,
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                    }));
            return services;
        }
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));
            services.AddTransient<IMarsRoverIntegrationEventService, MarsRoverIntegrationEventService>();
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = _settings.ServiceBus.RabbitMQUrl,
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(_settings.ServiceBus.RabbitUsername))
                {
                    factory.UserName = _settings.ServiceBus.RabbitUsername;
                }

                if (!string.IsNullOrEmpty(_settings.ServiceBus.RabbitPassword))
                {
                    factory.Password = _settings.ServiceBus.RabbitPassword;
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(_settings.ServiceBus.RetryCount))
                {
                    retryCount = int.Parse(_settings.ServiceBus.RetryCount);
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            return services;
        }
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            var subscriptionClientName = _settings.SubscriptionClientName;
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubscriptionManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(_settings.ServiceBus.RetryCount))
                {
                    retryCount = int.Parse(_settings.ServiceBus.RetryCount);
                }

                return new EventBusRabbitMQ(rabbitMqPersistentConnection, logger, iLifetimeScope, eventBusSubscriptionManager, subscriptionClientName, retryCount);
            });
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            return services;
        }
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new RequestModelMapperConfiguration());
                cfg.AddProfile(new ResponseModelMapperConfiguration());
            });
            mapperConfig.AssertConfigurationIsValid();
            services.AddSingleton(provider => mapperConfig.CreateMapper());
            return services;
        }
    }
}

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hb.MarsRover.Infrastructure.Configuration.Settings;
using Hb.MarsRover.Infrastructure.EventBus.EFEventStore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using System.Net;
using Hb.MarsRover.Infrastructure.Core.Http.Extensions;
using MarsRover.Api.Infrastructure.Data;
using MarsRover.Infrastructure;

namespace MarsRover.Api
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static void Main(string[] args)
        {
            try
            {
                var configuration = GetConfiguration();
                Log.Logger = CreateSerilogLogger(configuration);
                Log.Information("Configuring web host ({ApplicationContext})...", AppName);
                IHost host = CreateHostBuilder(args, configuration)
                    .Build();
                var lifetimeScope = host.Services.GetAutofacRoot();
                var env = lifetimeScope.Resolve<IWebHostEnvironment>();
                Log.Information("Applying migrations ({ApplicationContext})...", AppName);
                host.MigrateDbContext<MarsRoverContext>(lifetimeScope, (context, container) =>
                {
                    var settings = lifetimeScope.Resolve<IOptions<ApplicationSettings>>();
                    //Seed Data
                    var logger = lifetimeScope.Resolve<ILogger<MarsRoverContextSeed>>();
                    new MarsRoverContextSeed()
                        .SeedAsync(context, settings, logger)
                        .Wait();

                }).MigrateDbContext<IntegrationEventLogContext>(lifetimeScope, (_, __) => { });

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.Sources.Clear();
                    builder.AddConfiguration(configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        var ports = GetDefinedPorts(configuration);
                        options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                        });

                        options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseConfiguration(configuration);
                    //Uncomment when implement Vault
                    //webBuilder.UseVault();
                });
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }
        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["ApplicationSettings:Logging:SinkUrl"];

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 5006);
            var port = config.GetValue("PORT", 5005);
            return (port, grpcPort);
        }
    }
}

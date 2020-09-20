﻿using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;

namespace Hb.MarsRover.Infrastructure.Core.Http.Extensions
{
    public static class WebHostExtensions
    {
        public static IHost MigrateDbContext<TContext>(this IHost host, ILifetimeScope container, Action<TContext, ILifetimeScope> seeder) where TContext : DbContext
        {
            var logger = container.Resolve<ILogger<TContext>>();
            var context = container.Resolve<TContext>();
            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                var retry = Policy.Handle<System.Exception>()
                         .WaitAndRetry(new TimeSpan[]
                         {
                             TimeSpan.FromSeconds(3),
                             TimeSpan.FromSeconds(5),
                             TimeSpan.FromSeconds(8),
                         });
                retry.Execute(() => InvokeSeeder(seeder, context, container));
                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, ILifetimeScope> seeder, TContext context, ILifetimeScope container)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, container);
        }
    }
}
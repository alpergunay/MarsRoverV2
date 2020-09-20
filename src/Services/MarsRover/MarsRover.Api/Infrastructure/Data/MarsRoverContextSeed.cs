using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hb.MarsRover.Domain.Types;
using Hb.MarsRover.Infrastructure.Configuration.Settings;
using MarsRover.Domain.DomainModels;
using MarsRover.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using Polly;

namespace MarsRover.Api.Infrastructure.Data
{
    public class MarsRoverContextSeed
    {
        private ILogger<MarsRoverContextSeed> _logger;

        public async Task SeedAsync(MarsRoverContext context, IOptions<ApplicationSettings> settings,
            ILogger<MarsRoverContextSeed> logger)
        {
            _logger = logger;
            var contentRootPath = Directory.GetCurrentDirectory();

            var policy = CreatePolicy(logger, nameof(MarsRoverContextSeed));
            await policy.ExecuteAsync(async () =>
            {
                var useCustomizationData = settings.Value.UseCustomizationData;

                using (context)
                {
                    context.Database.Migrate();
                    foreach (var command in Enumeration.GetAll<Command>())
                    {
                        if (!context.Commands.Any(x => x.EnumId == command.EnumId))
                        {
                            await context.Commands.AddAsync(command);
                            await context.SaveChangesAsync();
                        }
                    }
                    foreach (var direction in Enumeration.GetAll<Direction>())
                    {
                        if (!context.Directions.Any(x => x.EnumId == direction.EnumId))
                        {
                            await context.Directions.AddAsync(direction);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            });
        }
        private AsyncPolicy CreatePolicy(ILogger<MarsRoverContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<NpgsqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}

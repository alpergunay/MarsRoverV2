﻿using Hb.MarsRover.Infrastructure.EventBus.Abstractions;
using Hb.MarsRover.Infrastructure.EventBus.Events;
using Hb.MarsRover.Infrastructure.EventBus.EFEventStore;
using Hb.MarsRover.Infrastructure.EventBus.EFEventStore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using MarsRover.Infrastructure;

namespace MarsRover.Api.Application.Modules.Infrastructure.IntegrationEvents
{
    public class MarsRoverIntegrationEventService : IMarsRoverIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly MarsRoverContext _mrContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<MarsRoverIntegrationEventService> _logger;

        public MarsRoverIntegrationEventService(IEventBus eventBus,
            MarsRoverContext mrContexts,
            IntegrationEventLogContext eventLogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<MarsRoverIntegrationEventService> logger)
        {
            _mrContext = mrContexts ?? throw new ArgumentNullException(nameof(mrContexts));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_mrContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, Program.AppName, logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, Program.AppName);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await _eventLogService.SaveEventAsync(evt, _mrContext.GetCurrentTransaction());
        }
    }
}
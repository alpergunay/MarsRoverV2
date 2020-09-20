using Hb.MarsRover.Infrastructure.EventBus.Events;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hb.MarsRover.Infrastructure.EventBus.EFEventStore.Services
{
    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);

        Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);

        Task MarkEventAsPublishedAsync(Guid eventId);

        Task MarkEventAsInProgressAsync(Guid eventId);

        Task MarkEventAsFailedAsync(Guid eventId);
    }
}
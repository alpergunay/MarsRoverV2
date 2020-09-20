using System;
using System.Threading.Tasks;
using Hb.MarsRover.Infrastructure.EventBus.Events;

namespace MarsRover.Api.Application.Modules.Infrastructure.IntegrationEvents
{
    public interface IMarsRoverIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);

        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
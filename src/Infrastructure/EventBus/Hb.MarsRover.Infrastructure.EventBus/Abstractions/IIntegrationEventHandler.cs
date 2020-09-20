using Hb.MarsRover.Infrastructure.EventBus.Events;
using System.Threading.Tasks;

namespace Hb.MarsRover.Infrastructure.EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
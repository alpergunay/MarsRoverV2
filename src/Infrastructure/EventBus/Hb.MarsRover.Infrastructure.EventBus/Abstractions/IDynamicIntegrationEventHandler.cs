using System.Threading.Tasks;

namespace Hb.MarsRover.Infrastructure.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
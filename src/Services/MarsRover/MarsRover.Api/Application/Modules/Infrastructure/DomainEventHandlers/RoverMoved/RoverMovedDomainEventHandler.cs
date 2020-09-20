using MarsRover.Domain.DomainModels;
using MarsRover.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Api.Application.Modules.Infrastructure.DomainEventHandlers.RoverMoved
{
    public class RoverMovedDomainEventHandler : INotificationHandler<RoverMovedDomainEvent>
    {
        private readonly ILogger<RoverMovedDomainEventHandler> _logger;
        private readonly IRoverRepository _roverRepository;

        public RoverMovedDomainEventHandler(ILogger<RoverMovedDomainEventHandler> logger, IRoverRepository roverRepository)
        {
            _logger = logger;
            _roverRepository = roverRepository;
        }
        public async Task Handle(RoverMovedDomainEvent roverMovedDomainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rover {Id} successfully moved to {x} {y} {d}",
                roverMovedDomainEvent.Rover.Id,
                roverMovedDomainEvent.Rover.CurrentCoordinate.XCoordinate,
                roverMovedDomainEvent.Rover.CurrentCoordinate.YCoordinate,
                roverMovedDomainEvent.Rover.CurrentDirection.Code);

            var roverToUpdate = await _roverRepository.FindOrDefaultAsync(roverMovedDomainEvent.Rover.Id);

            roverToUpdate.SetPosition(roverMovedDomainEvent.Rover.CurrentCoordinate.XCoordinate,
                roverMovedDomainEvent.Rover.CurrentCoordinate.YCoordinate,
                roverMovedDomainEvent.Rover.CurrentDirection.EnumId); 
            _roverRepository.Update(roverToUpdate);
            await _roverRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MarsRover.Api.Application.Modules.Infrastructure.DomainEventHandlers.RoverMoved;
using MarsRover.Api.Application.Modules.Infrastructure.IntegrationEvents;
using MarsRover.Api.Application.Modules.Infrastructure.IntegrationEvents.Events;
using MarsRover.Domain.DomainModels;
using MarsRover.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MarsRover.Api.Application.Modules.Infrastructure.DomainEventHandlers.RoverRotated
{
    public class RoverRotatedDomainEventHandler : INotificationHandler<RoverRotatedDomainEvent>
    {
        private readonly ILogger<RoverRotatedDomainEventHandler> _logger;
        private readonly IRoverRepository _roverRepository;
        private readonly IMarsRoverIntegrationEventService _marsRoverIntegrationEventService;

        public RoverRotatedDomainEventHandler(ILogger<RoverRotatedDomainEventHandler> logger,
            IRoverRepository roverRepository,
            IMarsRoverIntegrationEventService marsRoverIntegrationEventService)
        {
            _logger = logger;
            _roverRepository = roverRepository;
            _marsRoverIntegrationEventService = marsRoverIntegrationEventService;
        }
        public async Task Handle(RoverRotatedDomainEvent roverRotatedDomainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rover {Id} successfully moved to {x} {y} {d}",
                roverRotatedDomainEvent.Rover.Id,
                roverRotatedDomainEvent.Rover.CurrentCoordinate.XCoordinate,
                roverRotatedDomainEvent.Rover.CurrentCoordinate.YCoordinate,
                roverRotatedDomainEvent.Rover.Direction.Code);

            var rover = await _roverRepository.FindOrDefaultAsync(roverRotatedDomainEvent.Rover.Id);
            var commandProcessedIntegrationEvent = new CommandProcessedIntegrationEvent(rover.Id,
                rover.Plateau.Id,
                rover.CurrentCoordinate.XCoordinate,
                rover.CurrentCoordinate.YCoordinate,
                rover.Direction.EnumId,
                rover.Direction.Code);

            await _marsRoverIntegrationEventService.AddAndSaveEventAsync(commandProcessedIntegrationEvent);
        }
    }
}

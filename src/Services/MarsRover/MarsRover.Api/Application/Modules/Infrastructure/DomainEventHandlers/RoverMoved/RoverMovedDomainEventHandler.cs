﻿using MarsRover.Domain.DomainModels;
using MarsRover.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using MarsRover.Api.Application.Modules.Infrastructure.IntegrationEvents;
using MarsRover.Api.Application.Modules.Infrastructure.IntegrationEvents.Events;

namespace MarsRover.Api.Application.Modules.Infrastructure.DomainEventHandlers.RoverMoved
{
    public class RoverMovedDomainEventHandler : INotificationHandler<RoverMovedDomainEvent>
    {
        private readonly ILogger<RoverMovedDomainEventHandler> _logger;
        private readonly IRoverRepository _roverRepository;
        private readonly IMarsRoverIntegrationEventService _marsRoverIntegrationEventService;

        public RoverMovedDomainEventHandler(ILogger<RoverMovedDomainEventHandler> logger,
            IRoverRepository roverRepository,
            IMarsRoverIntegrationEventService marsRoverIntegrationEventService)
        {
            _logger = logger;
            _roverRepository = roverRepository;
            _marsRoverIntegrationEventService = marsRoverIntegrationEventService;
        }
        public async Task Handle(RoverMovedDomainEvent roverMovedDomainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rover {Id} successfully moved to {x} {y} {d}",
                roverMovedDomainEvent.Rover.Id,
                roverMovedDomainEvent.Rover.CurrentCoordinate.XCoordinate,
                roverMovedDomainEvent.Rover.CurrentCoordinate.YCoordinate,
                roverMovedDomainEvent.Rover.Direction.Code);

            var rover = await _roverRepository.FindOrDefaultAsync(roverMovedDomainEvent.Rover.Id);
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

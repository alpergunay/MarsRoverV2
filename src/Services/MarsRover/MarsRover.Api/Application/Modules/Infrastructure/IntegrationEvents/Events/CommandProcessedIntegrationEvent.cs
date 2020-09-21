using System;
using Hb.MarsRover.Infrastructure.EventBus.Events;

namespace MarsRover.Api.Application.Modules.Infrastructure.IntegrationEvents.Events
{
    public class CommandProcessedIntegrationEvent : IntegrationEvent
    {
        public Guid RoverId { get; set; }
        public Guid PlateauId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Direction { get; set; }
        public int DirectionId { get; set; }

        public CommandProcessedIntegrationEvent(Guid roverId, Guid plateauId, int x, int y, int directionId, string direction)
        {
            RoverId = roverId;
            X = x;
            Y = y;
            PlateauId = plateauId;
            Direction = direction;
            DirectionId = directionId;
        }
    }
}

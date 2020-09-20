using MarsRover.Domain.DomainModels;
using MediatR;

namespace MarsRover.Domain.Events
{
    public class RoverRotatedDomainEvent : INotification
    {
        public Rover Rover { get; }
        public RoverRotatedDomainEvent(Rover rover)
        {
            Rover = rover;
        }
    }
}

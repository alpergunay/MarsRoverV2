using System;
using System.Collections.Generic;
using System.Text;
using MarsRover.Domain.DomainModels;
using MediatR;

namespace MarsRover.Domain.Events
{
    public class RoverMovedDomainEvent : INotification
    {
        public Rover Rover { get; }

        public RoverMovedDomainEvent(Rover rover)
        {
            Rover = rover;
        }
    }
}

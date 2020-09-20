using System;
using System.Collections.Generic;
using System.Text;
using MarsRover.Domain.DomainModels;
using MediatR;

namespace MarsRover.Domain.Events
{
    public class InstructionProcessedDomainEvent : INotification
    {
        public Rover Rover { get; set; }

        public InstructionProcessedDomainEvent(Rover rover)
        {
            Rover = rover;
        }
    }

}

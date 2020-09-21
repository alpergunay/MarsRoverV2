using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Rover;
using MediatR;

namespace MarsRover.Api.Application.Modules.Infrastructure.Commands.Rover
{
    public class SendInstructionToRoverCommand : IRequest<RoverLastPositionResponseModel>
    {
        //TODO : Properties should be private set. Swagger problem...
        public Guid Id { get; set; }
        public string Instruction { get; set; }

        public SendInstructionToRoverCommand(Guid id, string instruction)
        {
            Id = id;
            Instruction = instruction;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Rover;
using MediatR;

namespace MarsRover.Api.Application.Modules.Infrastructure.Commands.Rover
{
    public class InitializeRoverCommand : IRequest<InitializeRoverResponseModel>
    {
        //TODO : Properties should be private set. Swagger problem...
        public Guid PlateauId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Direction { get; set; }

        public InitializeRoverCommand(Guid plateauId, int x, int y, string direction)
        {
            PlateauId = plateauId;
            X = x;
            Y = y;
            Direction = direction;
        }
    }
}

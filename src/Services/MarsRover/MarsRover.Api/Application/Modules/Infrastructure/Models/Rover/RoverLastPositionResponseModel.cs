using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRover.Api.Application.Modules.Infrastructure.Models.Rover
{
    public class RoverLastPositionResponseModel
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string Direction { get; set; }
    }
}

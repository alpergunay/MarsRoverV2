using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRover.Api.Application.Modules.Infrastructure.Models.Rover
{
    public class InitializeRoverResponseModel
    {
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Direction { get; set; }
    }
}

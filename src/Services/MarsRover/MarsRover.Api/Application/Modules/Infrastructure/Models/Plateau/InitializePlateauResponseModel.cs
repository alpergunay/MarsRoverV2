using System;

namespace MarsRover.Api.Application.Modules.Infrastructure.Models.Plateau
{
    public class InitializePlateauResponseModel
    {
        public Guid Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}

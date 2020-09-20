using MarsRover.Api.Application.Modules.Infrastructure.Models.Plateau;
using MediatR;

namespace MarsRover.Api.Application.Modules.Infrastructure.Commands.Plateau
{
    public class InitializePlateauCommand : IRequest<InitializePlateauResponseModel>
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}

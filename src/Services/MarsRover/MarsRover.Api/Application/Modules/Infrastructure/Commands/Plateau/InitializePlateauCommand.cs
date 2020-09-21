using MarsRover.Api.Application.Modules.Infrastructure.Models.Plateau;
using MediatR;

namespace MarsRover.Api.Application.Modules.Infrastructure.Commands.Plateau
{
    public class InitializePlateauCommand : IRequest<InitializePlateauResponseModel>
    {
        //TODO : Properties should be private set. Swagger problem...
        public int X { get; set; }
        public int Y { get; set; }

        public InitializePlateauCommand(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

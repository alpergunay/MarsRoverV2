using FluentValidation;
using Hb.MarsRover.Domain.Types;
using MarsRover.Api.Application.Modules.Infrastructure.Commands.Rover;
using MarsRover.Api.Application.Modules.Infrastructure.Validations.Plateau;
using MarsRover.Domain.DomainModels;
using Microsoft.Extensions.Logging;

namespace MarsRover.Api.Application.Modules.Infrastructure.Validations.Rover
{
    public class InitializeRoverCommandValidator : AbstractValidator<InitializeRoverCommand>
    {
        public InitializeRoverCommandValidator(ILogger<InitializeRoverCommandValidator> logger)
        {
            RuleFor(rover => rover.X).GreaterThan(0).WithMessage("X axis for plateau should be greater than 0");
            RuleFor(rover => rover.Y).GreaterThan(0).WithMessage("X axis for plateau should be greater than 0");
            RuleFor(rover => rover.Direction).NotEmpty().NotNull().WithMessage("Direction should not be empty or null");
            RuleFor(rover => rover.PlateauId).NotEmpty().NotNull().WithMessage("Plateau should not be empty or null");
            RuleFor(rover => rover.Direction).Must(d => Enumeration.FromCode<Direction>(d) != null)
                .WithMessage("Invalid direction code");
            RuleFor(rover => rover.PlateauId).SetValidator(new GuidValidator()).WithMessage("Invalid Guid Value");
        }
    }
}

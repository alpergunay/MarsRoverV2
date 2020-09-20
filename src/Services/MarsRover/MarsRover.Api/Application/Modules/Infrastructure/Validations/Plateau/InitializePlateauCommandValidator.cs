using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MarsRover.Api.Application.Modules.Infrastructure.Commands.Plateau;
using Microsoft.Extensions.Logging;

namespace MarsRover.Api.Application.Modules.Infrastructure.Validations.Plateau
{
    public class InitializePlateauCommandValidator : AbstractValidator<InitializePlateauCommand>
    {
        public InitializePlateauCommandValidator(ILogger<InitializePlateauCommandValidator> logger)
        {
            RuleFor(plateau => plateau.X).GreaterThan(0).WithMessage("X axis for plateau should be greater than 0");
            RuleFor(plateau => plateau.Y).GreaterThan(0).WithMessage("X axis for plateau should be greater than 0");
        }
    }
}

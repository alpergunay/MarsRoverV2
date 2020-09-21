using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Rover;
using MarsRover.Domain.DomainModels;
using MarsRover.Domain.Exceptions;
using MarsRover.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MarsRover.Api.Application.Modules.Infrastructure.Commands.Rover
{
    public class SendInstructionToRoverCommandHandler : IRequestHandler<SendInstructionToRoverCommand, RoverLastPositionResponseModel>
    {
        private readonly IRoverRepository _roverRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InitializeRoverCommandHandler> _logger;
        public SendInstructionToRoverCommandHandler(IRoverRepository roverRepository,
            IMapper mapper,
            ILogger<InitializeRoverCommandHandler> logger)
        {
            _roverRepository = roverRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<RoverLastPositionResponseModel> Handle(SendInstructionToRoverCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Instruction is sending to Rover {Id}", request.Id);
            var rover = await _roverRepository.FindOrDefaultAsync(request.Id);
            var commands = rover.ProcessInstruction(request.Instruction);

            foreach (var command in commands)
            {
                rover.ProcessCommand(command);
                _roverRepository.Update(rover);
                await _roverRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            return _mapper.Map<RoverLastPositionResponseModel>(rover);
        }
    }
}

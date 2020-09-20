using System;
using AutoMapper;
using Hb.MarsRover.Domain.Types;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Rover;
using MarsRover.Domain.DomainModels;
using MarsRover.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MarsRover.Api.Application.Modules.Infrastructure.Commands.Rover
{
    public class InitializeRoverCommandHandler : IRequestHandler<InitializeRoverCommand, InitializeRoverResponseModel>
    {
        private readonly IPlateauRepository _plateauRepository;
        private readonly IRoverRepository _roverRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InitializeRoverCommandHandler> _logger;
        public InitializeRoverCommandHandler(IPlateauRepository plateauRepository,
            IRoverRepository roverRepository,
            IMapper mapper,
            ILogger<InitializeRoverCommandHandler> logger)
        {
            _plateauRepository = plateauRepository;
            _roverRepository = roverRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<InitializeRoverResponseModel> Handle(InitializeRoverCommand request, CancellationToken cancellationToken)
        {
            var plateau = _plateauRepository.FindOrDefaultAsync(request.PlateauId);

            if (plateau == null)
            {
                _logger.LogError("Unable to find plateau with given id");
                throw new MarsRoverDomainException("Unable to find plateau with given id");
            }

            var direction = Enumeration.FromCode<Direction>(request.Direction);

            if (direction == null)
            {
                _logger.LogError("Unable to find direction with given code");
                throw new MarsRoverDomainException("Unable to find direction with given code");
            }
            var rover = new Domain.DomainModels.Rover(Guid.NewGuid(), request.X, request.Y, direction.EnumId, request.PlateauId);
            _roverRepository.Add(rover);

            if (await _roverRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                return _mapper.Map<InitializeRoverResponseModel>(rover);
            }
            throw new MarsRoverDomainException("Unable to initialize Rover");
        }
    }
}

using AutoMapper;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Plateau;
using MarsRover.Domain.DomainModels;
using MarsRover.Domain.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MarsRover.Api.Application.Modules.Infrastructure.Commands.Plateau
{
    public class InitializePlateauCommandHandler : IRequestHandler<InitializePlateauCommand, InitializePlateauResponseModel>
    {
        private readonly IPlateauRepository _plateauRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InitializePlateauCommandHandler> _logger;
        public InitializePlateauCommandHandler(IPlateauRepository plateauRepository,
            IMapper mapper,
            ILogger<InitializePlateauCommandHandler> logger)
        {
            _plateauRepository = plateauRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<InitializePlateauResponseModel> Handle(InitializePlateauCommand request, CancellationToken cancellationToken)
        {
            var plateau = new Domain.DomainModels.Plateau(Guid.NewGuid(), request.X, request.Y);
            _plateauRepository.Add(plateau);
            if(await _plateauRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                return _mapper.Map<InitializePlateauResponseModel>(plateau);
            }
            //TODO: Should be fixed
            throw new MarsRoverDomainException("Unable to initialize Plateau");
        }
    }
}

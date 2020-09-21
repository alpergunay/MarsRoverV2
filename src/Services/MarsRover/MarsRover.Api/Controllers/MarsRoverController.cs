using AutoMapper;
using Hb.MarsRover.Infrastructure.EventBus.Extensions;
using MarsRover.Api.Application.Modules.Infrastructure.Commands.Rover;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Rover;
using MarsRover.Domain.DomainModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MarsRover.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarsRoverController : ControllerBase
    {
        private readonly ILogger<MarsRoverController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IRoverRepository _roverRepository;
        public MarsRoverController(ILogger<MarsRoverController> logger, 
            IMediator mediator,
            IMapper mapper,
            IRoverRepository roverRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _roverRepository = roverRepository;
        }
        [Route("initialize")]
        [HttpPost]
        [ProducesResponseType(typeof(InitializeRoverResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(InitializeRoverResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<InitializeRoverResponseModel>> Initialize([FromBody] InitializeRoverCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {PlateauId} {X} {Y} {Direction} {@Command} ",
                command.GetGenericTypeName(),
                command.PlateauId,
                command.X,
                command.Y,
                command.Direction,
                command);

            var response = await _mediator.Send(command);

            if (response.Id != Guid.Empty)
                return Ok(response);
            return BadRequest(response);
        }

        [Route("instruction")]
        [HttpPost]
        [ProducesResponseType(typeof(RoverLastPositionResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(RoverLastPositionResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<RoverLastPositionResponseModel>> SendInstruction(SendInstructionToRoverCommand command)
        {
            _logger.LogInformation(
                "----- Sending command: {CommandName} - {RoverId} {Instruction} {@Command}",
                command.GetGenericTypeName(),
                command.Id,
                command.Instruction,
                command);

            var response = await _mediator.Send(command);

            if (response != null)
                return Ok(response);

            return BadRequest();
        }
        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(Rover), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(RoverLastPositionResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<RoverLastPositionResponseModel>> Get()
        {
            var rovers = await _roverRepository.GetAllAsync();
            return _mapper.Map<RoverLastPositionResponseModel>(rovers);
        }
    }
}

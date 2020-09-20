using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Hb.MarsRover.Infrastructure.EventBus.Extensions;
using MarsRover.Api.Application.Modules.Infrastructure.Commands.Rover;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Rover;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarsRover.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarsRoverController : ControllerBase
    {
        private readonly ILogger<MarsRoverController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public MarsRoverController(ILogger<MarsRoverController> logger, 
            IMediator mediator,
            IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }
        [Route("initialize")]
        [HttpPost]
        [ProducesResponseType(typeof(InitializeRoverResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(InitializeRoverResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<InitializeRoverResponseModel>> Initialize([FromBody] InitializeRoverCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {

            _logger.LogInformation(
                "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                command.GetGenericTypeName(),
                command.X,
                command.Y,
                command);

            var response = await _mediator.Send(command);

            if (response.Id != Guid.Empty)
                return Ok(response);
            return BadRequest(response);
        }
    }
}

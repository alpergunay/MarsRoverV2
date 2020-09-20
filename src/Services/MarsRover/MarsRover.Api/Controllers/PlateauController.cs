using AutoMapper;
using Hb.MarsRover.Infrastructure.EventBus.Extensions;
using MarsRover.Api.Application.Modules.Infrastructure.Commands.Plateau;
using MarsRover.Api.Application.Modules.Infrastructure.Models.Plateau;
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
    public class PlateauController : ControllerBase
    {
        private readonly ILogger<PlateauController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public PlateauController(ILogger<PlateauController> logger,
            IMediator mediator,
            IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        [Route("initialize")]
        [HttpPost]
        [ProducesResponseType(typeof(InitializePlateauResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(InitializePlateauResponseModel), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<InitializePlateauResponseModel>> Initialize([FromBody] InitializePlateauCommand command, [FromHeader(Name = "x-requestid")] string requestId)
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

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileAppCottage.Application.Cottages.Commands.CreateCottage;
using MobileAppCottage.Application.Cottages.Commands.DeleteCottage;
using MobileAppCottage.Application.Cottages.Commands.UpdateCottage;
using MobileAppCottage.Application.Cottages.Queries;
using MobileAppCottage.Application.DTOs;

namespace MobileAppCottage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CottageController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Repozytorium i Mapper zostaj¹ tutaj tylko, jeœli masz inne metody, 
        // które ich potrzebuj¹, ale w czystym CQRS MediatR wystarczy.
        public CottageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Zmienione na CQRS: Wysy³amy zapytanie (Query) do Handlera
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CottageDto>>> GetAll()
        {
            var dtos = await _mediator.Send(new GetAllCottagesQuery());
            return Ok(dtos);
        }

        // Zmienione na CQRS: Wysy³amy zapytanie (Query) o konkretne ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CottageDto>> GetById([FromRoute] int id)
        {
            var dto = await _mediator.Send(new GetCottageByIdQuery(id));
            return Ok(dto);
        }

        // TWÓJ DZIA£AJ¥CY POST - NIE RUSZAMY!
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCottageCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = id }, null);
        }

        // NOWE METODY WZOROWANE NA CREATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCottageCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteCottageCommand(id));
            return NoContent();
        }
    }
}
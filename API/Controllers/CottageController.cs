using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileAppCottage.Application.Cottages.Commands.CreateCottage;
using MobileAppCottage.Application.Cottages.Commands.DeleteCottage; // DODAJ TO
using MobileAppCottage.Application.Cottages.Commands.UpdateCottage; // DODAJ TO
using MobileAppCottage.Application.DTOs;
using MobileAppCottage.Domain.Interfaces;

namespace MobileAppCottage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CottageController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICottageRepository _cottageRepository;
        private readonly IMapper _mapper;

        public CottageController(IMediator mediator, ICottageRepository cottageRepository, IMapper mapper)
        {
            _mediator = mediator;
            _cottageRepository = cottageRepository;
            _mapper = mapper;
        }

        // Twoje dzia³aj¹ce GETy...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CottageDto>>> GetAll() => Ok(_mapper.Map<IEnumerable<CottageDto>>(await _cottageRepository.GetAll()));

        [HttpGet("{id}")]
        public async Task<ActionResult<CottageDto>> GetById([FromRoute] int id)
        {
            var cottage = await _cottageRepository.GetById(id);
            return cottage == null ? NotFound() : Ok(_mapper.Map<CottageDto>(cottage));
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
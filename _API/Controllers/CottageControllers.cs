using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileAppCottage.Application.DTOs;
using MobileAppCottage.Infrastructure.Persistence;

namespace MobileAppCottage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CottageController : ControllerBase
    {
        #region Konfiguracja i Konstruktor
        private readonly CottageDbContext _context;
        private readonly IMapper _mapper;

        public CottageController(CottageDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Metody GET (Pobieranie)

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CottageDto>>> GetAll()
        {
            // Pobieramy dane z bazy wraz z relacjami
            var cottages = await _context.Cottages
                .Include(c => c.Images)
                .Include(c => c.ContactDetails)
                .ToListAsync();

            // ZMIANA: Mapujemy na DTO, aby unikn¹æ b³êdu 500 (Circular Reference)
            var dtos = _mapper.Map<IEnumerable<CottageDto>>(cottages);

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CottageDto>> GetById([FromRoute] int id)
        {
            var cottage = await _context.Cottages
                .Include(c => c.Images)
                .Include(c => c.ContactDetails)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cottage == null) return NotFound();

            return Ok(_mapper.Map<CottageDto>(cottage));
        }

        #endregion

        #region Metody POST/DELETE (Modyfikacja)

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CottageDto cottageDto)
        {

            var cottage = _mapper.Map<Domain.Entities.Cottage>(cottageDto);
            cottage.EncodeName();

            _context.Cottages.Add(cottage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = cottage.Id }, null);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var cottage = await _context.Cottages.FirstOrDefaultAsync(c => c.Id == id);
            if (cottage == null) return NotFound();

            _context.Cottages.Remove(cottage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion
    }
}
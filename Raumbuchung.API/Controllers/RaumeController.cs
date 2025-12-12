using Microsoft.AspNetCore.Mvc;
using Raumbuchung.API.Models;
using Raumbuchung.API.Services;
using Raumbuchung.API.DTOs;

namespace Raumbuchung.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaumeController : ControllerBase
    {
        private readonly IRaumbuchungRepository _repository;
        private readonly ILogger<RaumeController> _logger;

        public RaumeController(IRaumbuchungRepository repository, ILogger<RaumeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RaumDto>>> GetAktiveRaume()
        {
            try
            {
                var raume = await _repository.GetAktiveRaumeAsync();
                var raumDtos = raume.Select(r => new RaumDto
                {
                    RaumId = r.RaumId,
                    RaumName = r.RaumName,
                    Kapazitaet = r.Kapazitaet,
                    Ausstattung = r.Ausstattung,
                    Etage = r.Etage,
                    Gebaeude = r.Gebaeude,
                    Aktiv = r.Aktiv
                });

                return Ok(raumDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen der Räume");
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RaumDto>> GetRaum(int id)
        {
            try
            {
                var raum = await _repository.GetRaumByIdAsync(id);
                if (raum == null)
                {
                    return NotFound();
                }

                var raumDto = new RaumDto
                {
                    RaumId = raum.RaumId,
                    RaumName = raum.RaumName,
                    Kapazitaet = raum.Kapazitaet,
                    Ausstattung = raum.Ausstattung,
                    Etage = raum.Etage,
                    Gebaeude = raum.Gebaeude,
                    Aktiv = raum.Aktiv
                };

                return Ok(raumDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen des Raums {Id}", id);
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        // Test-Endpoints behalten
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("✅ RaumeController funktioniert!");
        }

        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(new
            {
                Status = "Online",
                Time = DateTime.Now,
                Message = "API funktioniert",
                Database = "In-Memory (für Entwicklung)"
            });
        }
    }
}
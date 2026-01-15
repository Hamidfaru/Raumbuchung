using Microsoft.AspNetCore.Mvc;
using Raumbuchung.API.Models;
using Raumbuchung.API.Services;
using Raumbuchung.API.DTOs;

namespace Raumbuchung.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuchungenController : ControllerBase
    {
        private readonly IRaumbuchungRepository _repository;
        private readonly ILogger<BuchungenController> _logger;

        public BuchungenController(IRaumbuchungRepository repository, ILogger<BuchungenController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuchungDto>>> GetBuchungen()
        {
            try
            {
                var buchungen = await _repository.GetBuchungenAsync();
                var buchungDtos = buchungen.Select(MapToDto);
                return Ok(buchungDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen der Buchungen");
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuchungDto>> GetBuchung(int id)
        {
            try
            {
                var buchung = await _repository.GetBuchungByIdAsync(id);
                if (buchung == null)
                {
                    return NotFound();
                }

                return Ok(MapToDto(buchung));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen der Buchung {Id}", id);
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BuchungDto>> CreateBuchung(CreateBuchungDto createBuchungDto)
        {
            try
            {
                // Prüfe ob Raum existiert
                if (!await _repository.RaumExistiertAsync(createBuchungDto.RaumId))
                {
                    return BadRequest("Raum existiert nicht oder ist nicht aktiv.");
                }

                var buchung = new Buchung
                {
                    RaumId = createBuchungDto.RaumId,
                    BenutzerId = createBuchungDto.BenutzerId, // Wird vom Frontend gesendet
                    StartZeit = createBuchungDto.StartZeit,
                    EndZeit = createBuchungDto.EndZeit,
                    BuchungsZweck = createBuchungDto.BuchungsZweck,
                    TeilnehmerAnzahl = createBuchungDto.TeilnehmerAnzahl,
                    Bemerkungen = createBuchungDto.Bemerkungen
                };

                var erstellteBuchung = await _repository.CreateBuchungAsync(buchung);
                return CreatedAtAction(nameof(GetBuchung), new { id = erstellteBuchung.BuchungId }, MapToDto(erstellteBuchung));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Erstellen der Buchung");
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuchung(int id)
        {
            try
            {
                var success = await _repository.DeleteBuchungAsync(id);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Löschen der Buchung {Id}", id);
                return StatusCode(500, "Interner Serverfehler");
            }
        }


        // Hilfsmethode für Mapping (KORRIGIERT)
        private BuchungDto MapToDto(Buchung buchung)
        {
            return new BuchungDto
            {
                BuchungId = buchung.BuchungId,
                RaumId = buchung.RaumId,
                BenutzerId = buchung.BenutzerId, // WICHTIG
                BenutzerName = buchung.Benutzer?.VollerName ?? buchung.Benutzer?.Benutzername ?? "", // WICHTIG
                BenutzerEmail = buchung.Benutzer?.Email ?? "", // WICHTIG
                StartZeit = buchung.StartZeit,
                EndZeit = buchung.EndZeit,
                BuchungsZweck = buchung.BuchungsZweck,
                TeilnehmerAnzahl = buchung.TeilnehmerAnzahl,
                Status = buchung.Status,
                Bemerkungen = buchung.Bemerkungen,
                Raum = buchung.Raum != null ? new RaumDto
                {
                    RaumId = buchung.Raum.RaumId,
                    RaumName = buchung.Raum.RaumName,
                    Kapazitaet = buchung.Raum.Kapazitaet,
                    Ausstattung = buchung.Raum.Ausstattung,
                    Etage = buchung.Raum.Etage,
                    Gebaeude = buchung.Raum.Gebaeude,
                    Aktiv = buchung.Raum.Aktiv
                } : null
            };
        }
    }
}
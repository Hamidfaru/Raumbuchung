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
                var buchungDtos = buchungen.Select(b => new BuchungDto
                {
                    BuchungId = b.BuchungId,
                    RaumId = b.RaumId,
                    BenutzerName = b.BenutzerName,
                    BenutzerEmail = b.BenutzerEmail,
                    StartZeit = b.StartZeit,
                    EndZeit = b.EndZeit,
                    BuchungsZweck = b.BuchungsZweck,
                    TeilnehmerAnzahl = b.TeilnehmerAnzahl,
                    Status = b.Status,
                    Bemerkungen = b.Bemerkungen,
                    Raum = b.Raum != null ? new RaumDto
                    {
                        RaumId = b.Raum.RaumId,
                        RaumName = b.Raum.RaumName,
                        Kapazitaet = b.Raum.Kapazitaet,
                        Ausstattung = b.Raum.Ausstattung,
                        Etage = b.Raum.Etage,
                        Gebaeude = b.Raum.Gebaeude,
                        Aktiv = b.Raum.Aktiv
                    } : null
                });

                return Ok(buchungDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen der Buchungen");
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BuchungDto>> CreateBuchung(CreateBuchungDto createBuchungDto)
        {
            try
            {
                var buchung = new Buchung
                {
                    RaumId = createBuchungDto.RaumId,
                    BenutzerName = createBuchungDto.BenutzerName,
                    BenutzerEmail = createBuchungDto.BenutzerEmail,
                    StartZeit = createBuchungDto.StartZeit,
                    EndZeit = createBuchungDto.EndZeit,
                    BuchungsZweck = createBuchungDto.BuchungsZweck,
                    TeilnehmerAnzahl = createBuchungDto.TeilnehmerAnzahl,
                    Bemerkungen = createBuchungDto.Bemerkungen
                };

                var erstellteBuchung = await _repository.CreateBuchungAsync(buchung);

                // Lade den Raum für die Response
                var raum = await _repository.GetRaumByIdAsync(erstellteBuchung.RaumId);

                var buchungDto = new BuchungDto
                {
                    BuchungId = erstellteBuchung.BuchungId,
                    RaumId = erstellteBuchung.RaumId,
                    BenutzerName = erstellteBuchung.BenutzerName,
                    BenutzerEmail = erstellteBuchung.BenutzerEmail,
                    StartZeit = erstellteBuchung.StartZeit,
                    EndZeit = erstellteBuchung.EndZeit,
                    BuchungsZweck = erstellteBuchung.BuchungsZweck,
                    TeilnehmerAnzahl = erstellteBuchung.TeilnehmerAnzahl,
                    Status = erstellteBuchung.Status,
                    Bemerkungen = erstellteBuchung.Bemerkungen,
                    Raum = raum != null ? new RaumDto
                    {
                        RaumId = raum.RaumId,
                        RaumName = raum.RaumName,
                        Kapazitaet = raum.Kapazitaet,
                        Ausstattung = raum.Ausstattung,
                        Etage = raum.Etage,
                        Gebaeude = raum.Gebaeude,
                        Aktiv = raum.Aktiv
                    } : null
                };

                return CreatedAtAction(nameof(GetBuchung), new { id = buchungDto.BuchungId }, buchungDto);
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

                var buchungDto = new BuchungDto
                {
                    BuchungId = buchung.BuchungId,
                    RaumId = buchung.RaumId,
                    BenutzerName = buchung.BenutzerName,
                    BenutzerEmail = buchung.BenutzerEmail,
                    StartZeit = buchung.StartZeit,
                    EndZeit = buchung.EndZeit,
                    BuchungsZweck = buchung.BuchungsZweck,
                    TeilnehmerAnzahl = buchung.TeilnehmerAnzahl,
                    Status = buchung.Status,
                    Bemerkungen = buchung.Bemerkungen,
                    Raum = new RaumDto
                    {
                        RaumId = buchung.Raum.RaumId,
                        RaumName = buchung.Raum.RaumName,
                        Kapazitaet = buchung.Raum.Kapazitaet,
                        Ausstattung = buchung.Raum.Ausstattung,
                        Etage = buchung.Raum.Etage,
                        Gebaeude = buchung.Raum.Gebaeude,
                        Aktiv = buchung.Raum.Aktiv
                    }
                };

                return Ok(buchungDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fehler beim Abrufen der Buchung {Id}", id);
                return StatusCode(500, "Interner Serverfehler");
            }
        }
    }
}
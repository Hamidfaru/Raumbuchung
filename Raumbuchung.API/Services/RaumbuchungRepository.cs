using Microsoft.EntityFrameworkCore;
using Raumbuchung.API.Data;
using Raumbuchung.API.Models;

namespace Raumbuchung.API.Services
{
    public class RaumbuchungRepository : IRaumbuchungRepository
    {
        private readonly OracleDbContext _context;

        public RaumbuchungRepository(OracleDbContext context)
        {
            _context = context;
        }

        // Räume
        public async Task<List<Raum>> GetAktiveRaumeAsync()
        {
            return await _context.Raume
                .Where(r => r.Aktiv)
                .OrderBy(r => r.RaumName)
                .ToListAsync();
        }

        public async Task<Raum?> GetRaumByIdAsync(int id)
        {
            return await _context.Raume
                .FirstOrDefaultAsync(r => r.RaumId == id && r.Aktiv);
        }

        public async Task<Raum> CreateRaumAsync(Raum raum)
        {
            _context.Raume.Add(raum);
            await _context.SaveChangesAsync();
            return raum;
        }

        public async Task<bool> RaumExistiertAsync(int raumId)
        {
            return await _context.Raume
                .AnyAsync(r => r.RaumId == raumId && r.Aktiv);
        }

        // Buchungen
        public async Task<List<Buchung>> GetBuchungenAsync()
        {
            return await _context.Buchungen
                .Include(b => b.Raum)
                .Include(b => b.Benutzer) // WICHTIG: Benutzer mitladen
                .OrderByDescending(b => b.StartZeit)
                .ToListAsync();
        }

        public async Task<List<Buchung>> GetAktiveBuchungenAsync()
        {
            return await _context.Buchungen
                .Where(b => b.Status != "storniert" && b.EndZeit >= DateTime.Now)
                .Include(b => b.Raum)
                .Include(b => b.Benutzer) // WICHTIG: Benutzer mitladen
                .OrderBy(b => b.StartZeit)
                .ToListAsync();
        }

        public async Task<Buchung?> GetBuchungByIdAsync(int id)
        {
            return await _context.Buchungen
                .Include(b => b.Raum)
                .Include(b => b.Benutzer) // WICHTIG: Benutzer mitladen
                .FirstOrDefaultAsync(b => b.BuchungId == id);
        }

        public async Task<List<Buchung>> GetBuchungenByRaumAsync(int raumId)
        {
            return await _context.Buchungen
                .Where(b => b.RaumId == raumId)
                .Include(b => b.Raum)
                .Include(b => b.Benutzer) // WICHTIG: Benutzer mitladen
                .OrderBy(b => b.StartZeit)
                .ToListAsync();
        }

        public async Task<List<Buchung>> GetBuchungenByBenutzerAsync(int benutzerId)
        {
            return await _context.Buchungen
                .Where(b => b.BenutzerId == benutzerId)
                .Include(b => b.Raum)
                .Include(b => b.Benutzer)
                .OrderByDescending(b => b.StartZeit)
                .ToListAsync();
        }

        public async Task<List<Buchung>> GetBuchungenByZeitraumAsync(DateTime start, DateTime ende)
        {
            return await _context.Buchungen
                .Where(b => b.StartZeit >= start && b.EndZeit <= ende)
                .Include(b => b.Raum)
                .Include(b => b.Benutzer) // WICHTIG: Benutzer mitladen
                .OrderBy(b => b.StartZeit)
                .ToListAsync();
        }

        public async Task<bool> IstRaumFreiAsync(int raumId, DateTime start, DateTime ende, int? ausschlussBuchungId = null)
        {
            var query = _context.Buchungen
                .Where(b => b.RaumId == raumId
                         && b.Status != "storniert"
                         && ((b.StartZeit < ende && b.EndZeit > start)));

            if (ausschlussBuchungId.HasValue)
            {
                query = query.Where(b => b.BuchungId != ausschlussBuchungId.Value);
            }

            var kollidierendeBuchung = await query.FirstOrDefaultAsync();
            return kollidierendeBuchung == null;
        }

        public async Task<Buchung> CreateBuchungAsync(Buchung buchung)
        {
            // Prüfe ob Raum frei ist
            if (!await IstRaumFreiAsync(buchung.RaumId, buchung.StartZeit, buchung.EndZeit))
            {
                throw new InvalidOperationException("Raum ist in diesem Zeitraum bereits gebucht.");
            }

            _context.Buchungen.Add(buchung);
            await _context.SaveChangesAsync();
            return buchung;
        }

        public async Task<Buchung> UpdateBuchungAsync(Buchung buchung)
        {
            _context.Entry(buchung).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return buchung;
        }

        public async Task<bool> UpdateBuchungStatusAsync(int buchungId, string status)
        {
            var buchung = await _context.Buchungen.FindAsync(buchungId);
            if (buchung == null) return false;

            buchung.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBuchungAsync(int buchungId)
        {
            var buchung = await _context.Buchungen.FindAsync(buchungId);
            if (buchung == null) return false;

            _context.Buchungen.Remove(buchung);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
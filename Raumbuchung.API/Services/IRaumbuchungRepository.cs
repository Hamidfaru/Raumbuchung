using Raumbuchung.API.Models;

namespace Raumbuchung.API.Services
{
    public interface IRaumbuchungRepository
    {
        // Räume
        Task<List<Raum>> GetAktiveRaumeAsync();
        Task<Raum?> GetRaumByIdAsync(int id);
        Task<Raum> CreateRaumAsync(Raum raum);
        Task<bool> RaumExistiertAsync(int raumId);

        // Buchungen
        Task<List<Buchung>> GetBuchungenAsync();
        Task<List<Buchung>> GetAktiveBuchungenAsync();
        Task<Buchung?> GetBuchungByIdAsync(int id);
        Task<List<Buchung>> GetBuchungenByRaumAsync(int raumId);
        Task<List<Buchung>> GetBuchungenByBenutzerAsync(int benutzerId);
        Task<List<Buchung>> GetBuchungenByZeitraumAsync(DateTime start, DateTime ende);

        // Überprüfungen
        Task<bool> IstRaumFreiAsync(int raumId, DateTime start, DateTime ende, int? ausschlussBuchungId = null);

        // CRUD Operationen
        Task<Buchung> CreateBuchungAsync(Buchung buchung);
        Task<Buchung> UpdateBuchungAsync(Buchung buchung);
        Task<bool> UpdateBuchungStatusAsync(int buchungId, string status);
        Task<bool> DeleteBuchungAsync(int buchungId);
    }
}
// Services/IRaumbuchungRepository.cs
using Raumbuchung.API.Models;
using Raumbuchung.API.DTOs;

namespace Raumbuchung.API.Services
{
    public interface IRaumbuchungRepository
    {
        // Räume
        Task<List<Raum>> GetAktiveRaumeAsync();
        Task<Raum?> GetRaumByIdAsync(int id);
        Task<Raum> CreateRaumAsync(Raum raum);

        // Buchungen
        Task<List<Buchung>> GetBuchungenAsync();
        Task<Buchung?> GetBuchungByIdAsync(int id);
        Task<List<Buchung>> GetBuchungenByRaumAsync(int raumId);
        Task<List<Buchung>> GetBuchungenByZeitraumAsync(DateTime start, DateTime ende);
        Task<bool> IstRaumFreiAsync(int raumId, DateTime start, DateTime ende, int? ausschlussBuchungId = null);
        Task<Buchung> CreateBuchungAsync(Buchung buchung);
        Task<bool> UpdateBuchungStatusAsync(int buchungId, string status);
        Task<bool> DeleteBuchungAsync(int buchungId);
    }
}
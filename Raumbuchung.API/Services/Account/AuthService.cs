using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Raumbuchung.API.Data;
using Raumbuchung.API.DTOs.Account;
using Raumbuchung.API.Models.Account;

namespace Raumbuchung.API.Services.Account
{
    public class AuthService : IAuthService
    {
        private readonly OracleDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(OracleDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Passwort hashen
        private string HashPassword(string passwort)
        {
            using var sha256 = SHA256.Create();
            var salt = _configuration["Auth:Salt"] ?? "default-salt";
            var bytes = Encoding.UTF8.GetBytes(passwort + salt);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // Passwort verifizieren
        private bool VerifyPassword(string passwort, string passwortHash)
        {
            var hash = HashPassword(passwort);
            return hash == passwortHash;
        }

        public async Task<Benutzer?> RegistrierenAsync(RegisterDto registerDto)
        {
            // Prüfe ob Benutzer bereits existiert
            if (await BenutzerExistiertAsync(registerDto.Email, registerDto.Benutzername))
                return null;

            var benutzer = new Benutzer
            {
                Benutzername = registerDto.Benutzername,
                Email = registerDto.Email,
                Telefonnummer = registerDto.Telefonnummer,
                PasswortHash = HashPassword(registerDto.Passwort),
                VollerName = registerDto.VollerName,
                Abteilung = registerDto.Abteilung,
                ErstellungsDatum = DateTime.Now
            };

            _context.Benutzer.Add(benutzer);
            await _context.SaveChangesAsync();
            return benutzer;
        }

        public async Task<Benutzer?> LoginAsync(string loginName, string passwort)
        {
            // Suche Benutzer nach Email oder Benutzername
            var benutzer = await _context.Benutzer
                .FirstOrDefaultAsync(u =>
                    u.Email == loginName ||
                    u.Benutzername == loginName);

            if (benutzer == null || !VerifyPassword(passwort, benutzer.PasswortHash))
                return null;

            if (!benutzer.IstAktiv)
                throw new InvalidOperationException("Benutzerkonto ist deaktiviert");

            // Aktualisiere letztes Login-Datum
            benutzer.LetzterLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            return benutzer;
        }

        public async Task<Benutzer?> GetBenutzerByIdAsync(int id)
        {
            return await _context.Benutzer.FindAsync(id);
        }

        public async Task<Benutzer?> GetBenutzerByEmailAsync(string email)
        {
            return await _context.Benutzer.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Benutzer?> GetBenutzerByBenutzernameAsync(string benutzername)
        {
            return await _context.Benutzer.FirstOrDefaultAsync(u => u.Benutzername == benutzername);
        }

        public async Task<bool> BenutzerExistiertAsync(string email, string benutzername)
        {
            return await _context.Benutzer
                .AnyAsync(u => u.Email == email || u.Benutzername == benutzername);
        }

        public async Task UpdateLetzterLoginAsync(int benutzerId)
        {
            var benutzer = await _context.Benutzer.FindAsync(benutzerId);
            if (benutzer != null)
            {
                benutzer.LetzterLogin = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
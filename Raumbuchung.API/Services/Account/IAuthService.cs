using Raumbuchung.API.DTOs.Account;
using Raumbuchung.API.Models.Account;

namespace Raumbuchung.API.Services.Account
{
    public interface IAuthService
    {
        Task<Benutzer?> RegistrierenAsync(RegisterDto registerDto);
        Task<Benutzer?> LoginAsync(string loginName, string passwort);
        Task<Benutzer?> GetBenutzerByIdAsync(int id);
        Task<Benutzer?> GetBenutzerByEmailAsync(string email);
        Task<Benutzer?> GetBenutzerByBenutzernameAsync(string benutzername);
        Task<bool> BenutzerExistiertAsync(string email, string benutzername);
        Task UpdateLetzterLoginAsync(int benutzerId);
    }
}
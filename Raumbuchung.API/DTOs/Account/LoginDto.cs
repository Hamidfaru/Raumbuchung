using System.ComponentModel.DataAnnotations;

namespace Raumbuchung.API.DTOs.Account
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Benutzername oder Email ist erforderlich")]
        public string LoginName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich")]
        public string Passwort { get; set; } = string.Empty;

        public bool RememberMe { get; set; } = false;
    }
}
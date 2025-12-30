using System.ComponentModel.DataAnnotations;

namespace Raumbuchung.API.DTOs.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Benutzername ist erforderlich")]
        [StringLength(50, MinimumLength = 3)]
        public string Benutzername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email ist erforderlich")]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Ungültige Telefonnummer")]
        [StringLength(20, ErrorMessage = "Telefonnummer darf maximal 20 Zeichen lang sein")]
        public string? Telefonnummer { get; set; }

        [Required(ErrorMessage = "Passwort ist erforderlich")]
        [StringLength(100, MinimumLength = 6)]
        public string Passwort { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwortbestätigung ist erforderlich")]
        [Compare("Passwort")]
        public string PasswortBestaetigung { get; set; } = string.Empty;

        [StringLength(100)]
        public string? VollerName { get; set; }

        [StringLength(100)]
        public string? Abteilung { get; set; }
    }
}
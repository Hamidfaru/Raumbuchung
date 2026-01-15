namespace Raumbuchung.API.DTOs.Account
{
    public class UserDto
    {
        public int BenutzerId { get; set; }
        public string Benutzername { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefonnummer { get; set; }
        public string? VollerName { get; set; }
        public string? Abteilung { get; set; }
        public bool IstAdmin { get; set; }
        public bool IstAktiv { get; set; }
        public DateTime ErstellungsDatum { get; set; }
        public DateTime? LetzterLogin { get; set; }
    }

    // API-Antwort Wrapper (optional)
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
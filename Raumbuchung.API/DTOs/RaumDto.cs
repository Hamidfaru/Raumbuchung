// DTOs/RaumDto.cs
namespace Raumbuchung.API.DTOs
{
    public class RaumDto
    {
        public int RaumId { get; set; }
        public string RaumName { get; set; } = string.Empty;
        public int Kapazitaet { get; set; }
        public string? Ausstattung { get; set; }
        public int? Etage { get; set; }
        public string? Gebaeude { get; set; }
        public bool Aktiv { get; set; }
    }

    public class CreateRaumDto
    {
        public string RaumName { get; set; } = string.Empty;
        public int Kapazitaet { get; set; }
        public string? Ausstattung { get; set; }
        public int? Etage { get; set; }
        public string? Gebaeude { get; set; }
    }
}
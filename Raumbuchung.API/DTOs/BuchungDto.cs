namespace Raumbuchung.API.DTOs
{
    public class BuchungDto
    {
        public int BuchungId { get; set; }
        public int RaumId { get; set; }
        public int BenutzerId { get; set; }
        public string BenutzerName { get; set; } = string.Empty;
        public string BenutzerEmail { get; set; } = string.Empty;
        public DateTime StartZeit { get; set; }
        public DateTime EndZeit { get; set; }
        public string BuchungsZweck { get; set; } = string.Empty;
        public int? TeilnehmerAnzahl { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Bemerkungen { get; set; }
        public RaumDto? Raum { get; set; }
    }

    public class CreateBuchungDto
    {
        public int RaumId { get; set; }
        public int BenutzerId { get; set; }
        public DateTime StartZeit { get; set; }
        public DateTime EndZeit { get; set; }
        public string BuchungsZweck { get; set; } = string.Empty;
        public int? TeilnehmerAnzahl { get; set; }
        public string? Bemerkungen { get; set; }
    }
}
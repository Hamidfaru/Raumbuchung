using System.ComponentModel.DataAnnotations;

namespace Raumbuchung.API.DTOs
{
    public class UpdateBuchungDto
    {
        [Required]
        public DateTime StartZeit { get; set; }

        [Required]
        public DateTime EndZeit { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 3)]
        public string BuchungsZweck { get; set; } = string.Empty;

        [Range(1, 100)]
        public int? TeilnehmerAnzahl { get; set; }

        [StringLength(500)]
        public string? Bemerkungen { get; set; }
    }
}
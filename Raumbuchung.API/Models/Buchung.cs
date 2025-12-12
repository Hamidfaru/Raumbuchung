using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raumbuchung.API.Models
{
    [Table("BUCHUNGEN")]
    public class Buchung
    {
        [Key]
        [Column("BUCHUNGID")]
        public int BuchungId { get; set; }

        [Required]
        [Column("RAUMID")]
        public int RaumId { get; set; }

        [Required]
        [Column("BENUTZERNAME")]
        [StringLength(100)]
        public string BenutzerName { get; set; } = string.Empty;

        [Required]
        [Column("BENUTZEREMAIL")]
        [StringLength(150)]
        [EmailAddress]
        public string BenutzerEmail { get; set; } = string.Empty;

        [Required]
        [Column("STARTZEIT")]
        public DateTime StartZeit { get; set; }

        [Required]
        [Column("ENDZEIT")]
        public DateTime EndZeit { get; set; }

        [Required]
        [Column("BUCHUNGSZWECK")]
        [StringLength(300)]
        public string BuchungsZweck { get; set; } = string.Empty;

        [Column("TEILNEHMERANZAHL")]
        public int? TeilnehmerAnzahl { get; set; }

        [Column("BUCHUNGSDATUM")]
        public DateTime BuchungsDatum { get; set; } = DateTime.Now;

        [Column("STATUS")]
        [StringLength(20)]
        public string Status { get; set; } = "bestätigt";

        [Column("BEMERKUNGEN")]
        [StringLength(500)]
        public string? Bemerkungen { get; set; }

        // Navigation Property - Verknüpfung zum Raum
        [ForeignKey("RaumId")]
        public virtual Raum Raum { get; set; } = null!;
    }
}
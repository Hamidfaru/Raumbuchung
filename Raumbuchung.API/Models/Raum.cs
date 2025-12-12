using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raumbuchung.API.Models
{
    [Table("RAUME")]
    public class Raum
    {
        [Key]
        [Column("RAUMID")]
        public int RaumId { get; set; }

        [Required]
        [Column("RAUMNAME")]
        [StringLength(100)]
        public string RaumName { get; set; } = string.Empty;

        [Required]
        [Column("KAPAZITAET")]
        public int Kapazitaet { get; set; }

        [Column("AUSSTATTUNG")]
        [StringLength(500)]
        public string? Ausstattung { get; set; }
        [Column("ETAGE")]
        public int? Etage { get; set; }  // int? bleiben

        [Column("GEBAEUDE")]
        [StringLength(100)]
        public string? Gebaeude { get; set; }  // string? bleiben
        [Column("AKTIV")]
        public bool Aktiv { get; set; } = true;

        [Column("ERSTELLUNGSDATUM")]
        public DateTime ErstellungsDatum { get; set; } = DateTime.Now;

        // Navigation Property für Buchungen
        public virtual ICollection<Buchung> Buchungen { get; set; } = new List<Buchung>();
    }
}
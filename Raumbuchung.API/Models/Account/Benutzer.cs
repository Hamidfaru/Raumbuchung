using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Raumbuchung.API.Models.Account
{
    [Table("BENUTZER")]
    public class Benutzer
    {
        [Key]
        [Column("BENUTZERID")]
        public int BenutzerId { get; set; }

        [Required]
        [Column("BENUTZERNAME")]
        [StringLength(50)]
        public string Benutzername { get; set; } = string.Empty;

        [Required]
        [Column("EMAIL")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Column("TELEFONNUMMER")]
        [StringLength(20)]
        public string? Telefonnummer { get; set; }

        [Required]
        [Column("PASSWORTHASH")]
        public string PasswortHash { get; set; } = string.Empty;

        [Column("VOLLERNAME")]
        [StringLength(100)]
        public string? VollerName { get; set; }

        [Column("ABTEILUNG")]
        [StringLength(100)]
        public string? Abteilung { get; set; }

        [Column("ISTADMIN")]
        public bool IstAdmin { get; set; } = false;

        [Column("ISTAKTIV")]
        public bool IstAktiv { get; set; } = true;

        [Column("ERSTELLUNGSDATUM")]
        public DateTime ErstellungsDatum { get; set; } = DateTime.Now;

        [Column("LETZTERLOGIN")]
        public DateTime? LetzterLogin { get; set; }

        // Navigation Properties
        public virtual ICollection<Buchung> Buchungen { get; set; } = new List<Buchung>();
    }
}
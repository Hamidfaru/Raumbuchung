using Microsoft.EntityFrameworkCore;
using Raumbuchung.API.Models;
using Raumbuchung.API.Models.Account;

namespace Raumbuchung.API.Data
{
    public class OracleDbContext : DbContext
    {
        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
        {
        }

        // DbSet für jede Tabelle
        public DbSet<Raum> Raume { get; set; }
        public DbSet<Buchung> Buchungen { get; set; }
        public DbSet<Benutzer> Benutzer { get; set; } // NEU

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabellennamen konfigurieren
            modelBuilder.Entity<Raum>().ToTable("RAUME");
            modelBuilder.Entity<Buchung>().ToTable("BUCHUNGEN");
            modelBuilder.Entity<Benutzer>().ToTable("BENUTZER"); // NEU

            // Primärschlüssel konfigurieren
            modelBuilder.Entity<Raum>()
                .HasKey(r => r.RaumId);

            modelBuilder.Entity<Buchung>()
                .HasKey(b => b.BuchungId);

            modelBuilder.Entity<Benutzer>()
                .HasKey(b => b.BenutzerId); // NEU

            // Standardwerte setzen
            modelBuilder.Entity<Raum>()
                .Property(r => r.ErstellungsDatum)
                .HasDefaultValueSql("SYSDATE");

            modelBuilder.Entity<Raum>()
                .Property(r => r.Aktiv)
                .HasDefaultValue(true);

            modelBuilder.Entity<Buchung>()
                .Property(b => b.BuchungsDatum)
                .HasDefaultValueSql("SYSDATE");

            modelBuilder.Entity<Buchung>()
                .Property(b => b.Status)
                .HasDefaultValue("bestätigt");

            modelBuilder.Entity<Benutzer>()
                .Property(b => b.IstAktiv)
                .HasDefaultValue(true); // NEU

            modelBuilder.Entity<Benutzer>()
                .Property(b => b.IstAdmin)
                .HasDefaultValue(false); // NEU

            modelBuilder.Entity<Benutzer>()
                .Property(b => b.ErstellungsDatum)
                .HasDefaultValueSql("SYSDATE"); // NEU

            // Fremdschlüssel-Beziehungen
            modelBuilder.Entity<Buchung>()
                .HasOne(b => b.Raum)
                .WithMany(r => r.Buchungen)
                .HasForeignKey(b => b.RaumId);

            // NEU: Buchung gehört zu Benutzer
            modelBuilder.Entity<Buchung>()
                .HasOne(b => b.Benutzer)
                .WithMany(u => u.Buchungen)
                .HasForeignKey(b => b.BenutzerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexe konfigurieren
            modelBuilder.Entity<Raum>()
                .HasIndex(r => r.RaumName)
                .IsUnique();

            modelBuilder.Entity<Buchung>()
                .HasIndex(b => new { b.RaumId, b.StartZeit, b.EndZeit });

            modelBuilder.Entity<Benutzer>() // NEU
                .HasIndex(u => u.Benutzername)
                .IsUnique();

            modelBuilder.Entity<Benutzer>() // NEU
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Benutzer>() // NEU
                .HasIndex(u => u.Telefonnummer)
                .IsUnique()
                .HasFilter("TELEFONNUMMER IS NOT NULL");

            base.OnModelCreating(modelBuilder);
        }
    }
}
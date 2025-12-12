using Microsoft.EntityFrameworkCore;
using Raumbuchung.API.Models;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabellennamen konfigurieren
            modelBuilder.Entity<Raum>().ToTable("RAUME");
            modelBuilder.Entity<Buchung>().ToTable("BUCHUNGEN");

            // Primärschlüssel konfigurieren
            modelBuilder.Entity<Raum>()
                .HasKey(r => r.RaumId);

            modelBuilder.Entity<Buchung>()
                .HasKey(b => b.BuchungId);

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

            // Fremdschlüssel-Beziehung
            modelBuilder.Entity<Buchung>()
                .HasOne(b => b.Raum)
                .WithMany(r => r.Buchungen)
                .HasForeignKey(b => b.RaumId);

            // Indexe konfigurieren
            modelBuilder.Entity<Raum>()
                .HasIndex(r => r.RaumName)
                .IsUnique();

            modelBuilder.Entity<Buchung>()
                .HasIndex(b => new { b.RaumId, b.StartZeit, b.EndZeit });

            modelBuilder.Entity<Buchung>()
                .HasIndex(b => b.BenutzerEmail);

            base.OnModelCreating(modelBuilder);
        }
    }
}
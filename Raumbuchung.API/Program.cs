using Microsoft.EntityFrameworkCore;

using Raumbuchung.API.Data;
using Raumbuchung.API.Models;
using Raumbuchung.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context - NUR EINE KONFIGURATION!
// ENTWEDER Oracle:
// builder.Services.AddDbContext<OracleDbContext>(options =>
//     options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// ODER In-Memory (für Entwicklung):
builder.Services.AddDbContext<OracleDbContext>(options =>
    options.UseInMemoryDatabase("RaumbuchungDB"));

// Services
builder.Services.AddScoped<IRaumbuchungRepository, RaumbuchungRepository>();

// CORS für Angular Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Test-Daten einfügen
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OracleDbContext>();

    // Datenbank erstellen (falls nicht existiert)
    dbContext.Database.EnsureCreated();

    // Test-Daten nur wenn keine Räume existieren
    dbContext.Raume.AddRange(
     new Raum
     {
         RaumName = "Konferenzraum 101",
         Kapazitaet = 20,
         Ausstattung = "Beamer, Whiteboard",
         Etage = 1,        // RICHTIG: Zahl für int?
         Gebaeude = "Hauptgebäude",
         Aktiv = true,
         ErstellungsDatum = DateTime.Now.AddDays(-100)
     },
     new Raum
     {
         RaumName = "Meetingraum 201",
         Kapazitaet = 8,
         Ausstattung = "TV, Konferenztelefon",
         Etage = 2,        // RICHTIG: Zahl für int?
         Gebaeude = "Hauptgebäude",
         Aktiv = true,
         ErstellungsDatum = DateTime.Now.AddDays(-50)
     }
 ); dbContext.Raume.AddRange(
    new Raum
    {
        RaumName = "Konferenzraum 101",
        Kapazitaet = 20,
        Ausstattung = "Beamer, Whiteboard",
        Etage = 1,        // RICHTIG: Zahl für int?
        Gebaeude = "Hauptgebäude",
        Aktiv = true,
        ErstellungsDatum = DateTime.Now.AddDays(-100)
    },
    new Raum
    {
        RaumName = "Meetingraum 201",
        Kapazitaet = 8,
        Ausstattung = "TV, Konferenztelefon",
        Etage = 2,        // RICHTIG: Zahl für int?
        Gebaeude = "Hauptgebäude",
        Aktiv = true,
        ErstellungsDatum = DateTime.Now.AddDays(-50)
    }
);

    dbContext.SaveChanges();
        Console.WriteLine("✅ Test-Daten erfolgreich eingefügt!");
    }


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using Raumbuchung.API.Data;
using Raumbuchung.API.DTOs.Account;
using Raumbuchung.API.Models;
using Raumbuchung.API.Models.Account;
using Raumbuchung.API.Services;
using Raumbuchung.API.Services.Account;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context
builder.Services.AddDbContext<OracleDbContext>(options =>
    options.UseInMemoryDatabase("RaumbuchungDB"));

// Services
builder.Services.AddScoped<IRaumbuchungRepository, RaumbuchungRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// CORS für ALLE Zugriffe (Entwicklung)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Test-Daten einfügen
async Task InitializeDatabaseAsync()
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<OracleDbContext>();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

    dbContext.Database.EnsureCreated();

    // 1. ZUERST Räume erstellen
    if (!dbContext.Raume.Any())
    {
        dbContext.Raume.AddRange(
            new Raum
            {
                RaumName = "Konferenzraum 101",
                Kapazitaet = 20,
                Ausstattung = "Beamer, Whiteboard",
                Etage = 1,
                Gebaeude = "Hauptgebäude",
                Aktiv = true,
                ErstellungsDatum = DateTime.Now.AddDays(-100)
            },
            new Raum
            {
                RaumName = "Meetingraum 201",
                Kapazitaet = 8,
                Ausstattung = "TV, Konferenztelefon",
                Etage = 2,
                Gebaeude = "Hauptgebäude",
                Aktiv = true,
                ErstellungsDatum = DateTime.Now.AddDays(-50)
            }
        );

        await dbContext.SaveChangesAsync();
        Console.WriteLine("✅ Räume erstellt");
    }

    // 2. DANN Benutzer erstellen
    if (!dbContext.Benutzer.Any())
    {
        // Test-Benutzer
        var registerDto = new RegisterDto
        {
            Benutzername = "testuser",
            Email = "test@example.com",
            Passwort = "Test123!",
            PasswortBestaetigung = "Test123!",
            Telefonnummer = "+436641234567",
            VollerName = "Max Mustermann",
            Abteilung = "Vertrieb"
        };

        await authService.RegistrierenAsync(registerDto);

        // Admin-Benutzer
        var adminDto = new RegisterDto
        {
            Benutzername = "admin",
            Email = "admin@example.com",
            Passwort = "Admin123!",
            PasswortBestaetigung = "Admin123!",
            Telefonnummer = "+436648765432",
            VollerName = "Administrator",
            Abteilung = "IT"
        };

        var admin = await authService.RegistrierenAsync(adminDto);
        if (admin != null)
        {
            admin.IstAdmin = true;
            await dbContext.SaveChangesAsync();
        }

        // Weitere Test-Benutzer
        var kundeDto = new RegisterDto
        {
            Benutzername = "kunde",
            Email = "kunde@example.com",
            Passwort = "Kunde123!",
            PasswortBestaetigung = "Kunde123!",
            Telefonnummer = "+436501234567",
            VollerName = "Maria Schmidt",
            Abteilung = "Einkauf"
        };
        await authService.RegistrierenAsync(kundeDto);

        Console.WriteLine("✅ Österreichische Test-Benutzer erfolgreich eingefügt!");
    }
}

// Datenbank asynchron initialisieren
await InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Root endpoint
app.MapGet("/", () => "✅ Raumbuchung API läuft!");

// Test endpoint
app.MapGet("/api/test", () => new
{
    message = "API funktioniert",
    time = DateTime.Now,
    endpoints = new[] { "/api/raume", "/api/buchungen", "/api/account" }
});

Console.WriteLine("🚀 API läuft auf http://localhost:5012");
Console.WriteLine("📚 Swagger UI: http://localhost:5012/swagger");
Console.WriteLine("⏹️  Drücke Ctrl+C zum Beenden");

await app.RunAsync();
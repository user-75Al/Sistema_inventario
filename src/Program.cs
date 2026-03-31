using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UtmMarket.Application;
using UtmMarket.Infrastructure;
using UtmMarket.Infrastructure.Data;
using UtmMarket.WebAPI.Middleware;
using UtmMarket.WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Configurar el puerto para Render (toman el puerto de la variable de entorno PORT)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// --- Configuración de Servicios ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Registro de capas (Clean Architecture)
builder.Services.AddPersistence();
builder.Services.AddApplication();

// Configuración de CORS - Permitimos todo para asegurar que GitHub Pages pueda conectarse
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- Configuración de Archivos Estáticos (Para React) ---
app.UseDefaultFiles();
app.UseStaticFiles();

// --- Manejo de Errores ---
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
} else {
    app.UseExceptionHandler("/error");
}

app.UseCors();

// --- Inicialización de BD Automática ---
using (var scope = app.Services.CreateScope()) {
    try {
        var migrator = scope.ServiceProvider.GetRequiredService<DatabaseMigrator>();
        await migrator.EnsureStructureAsync();
    } catch (Exception ex) {
        Console.WriteLine($"[ERROR] Fallo en migración: {ex.Message}");
    }
}

app.MapControllers();

// --- Ruta para que React maneje el Navegador ---
app.MapFallbackToFile("index.html");

app.Run();

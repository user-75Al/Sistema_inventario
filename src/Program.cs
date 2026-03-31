using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UtmMarket.Application;
using UtmMarket.Infrastructure;
using UtmMarket.Infrastructure.Data;
using UtmMarket.WebAPI.Middleware;
using UtmMarket.WebAPI;

var builder = WebApplication.CreateBuilder(args);

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

// --- Manejo de Errores ---
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
} else {
    // En producción usamos un manejador de excepciones más limpio
    app.UseExceptionHandler(errorApp => {
        errorApp.Run(async context => {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\": \"Error interno del servidor en SomEE.\"}");
        });
    });
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
app.Run();

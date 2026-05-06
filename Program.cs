using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Permite sobreescribir configuración sensible localmente sin commitearla.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Agregar soporte para controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Agregar OpenAPI/Swagger
builder.Services.AddOpenApi();

// Configurar DbContext y servicios del dominio
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' no configurada. Usa appsettings.Local.json o variables de entorno.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<ProyectoService>();
builder.Services.AddScoped<TemaService>();
builder.Services.AddScoped<PruebaService>();
builder.Services.AddScoped<PreguntaService>();
builder.Services.AddScoped<RespuestaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<TutorService>();
builder.Services.AddScoped<AlumnoService>();
builder.Services.AddScoped<DocenteAlumnoService>();
builder.Services.AddScoped<DocenteService>();
builder.Services.AddScoped<CampoFormativoService>();
builder.Services.AddScoped<ResultadoService>();
builder.Services.AddScoped<ChatHistorialService>();

// Configurar Tutor de Matemáticas
builder.Services.Configure<MathTutorOptions>(builder.Configuration.GetSection(MathTutorOptions.SectionName));
builder.Services.AddHttpClient<MathTutorService>();

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AppCors", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            return;
        }

        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            return;
        }

        // Sin orígenes configurados en producción: bloquea navegadores por CORS.
        policy.AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configurar OpenAPI/Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware
app.UseHttpsRedirection();
app.UseCors("AppCors");

// Mapear controllers
app.MapControllers();

app.Run();
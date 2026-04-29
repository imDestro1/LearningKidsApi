using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar soporte para controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Agregar OpenAPI/Swagger
builder.Services.AddOpenApi();

// Configurar DbContext y servicios del dominio
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

var app = builder.Build();

// Configurar OpenAPI/Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Middleware
app.UseHttpsRedirection();

// Mapear controllers
app.MapControllers();

app.Run();
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Tutor> Tutores { get; set; } = null!;
        public DbSet<Alumno> Alumnos { get; set; } = null!;
        public DbSet<DocenteAlumno> DocenteAlumnos { get; set; } = null!;
        public DbSet<CampoFormativo> CamposFormativos { get; set; } = null!;
        public DbSet<Proyecto> Proyectos { get; set; } = null!;
        public DbSet<Tema> Temas { get; set; } = null!;
        public DbSet<Prueba> Pruebas { get; set; } = null!;
        public DbSet<Pregunta> Preguntas { get; set; } = null!;
        public DbSet<Respuesta> Respuestas { get; set; } = null!;
        public DbSet<Resultado> Resultados { get; set; } = null!;
        public DbSet<ChatHistorial> ChatHistorial { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proyecto>()
                .HasMany(p => p.Temas)
                .WithOne(t => t.Proyecto)
                .HasForeignKey(t => t.idProyecto)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tema>()
                .HasMany(t => t.Pruebas)
                .WithOne(p => p.Tema)
                .HasForeignKey(p => p.idTema)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prueba>()
                .HasMany(p => p.Preguntas)
                .WithOne(q => q.Prueba)
                .HasForeignKey(q => q.idPrueba)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pregunta>()
                .HasMany(q => q.Respuestas)
                .WithOne(r => r.Pregunta)
                .HasForeignKey(r => r.idPregunta)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.idRol);

            modelBuilder.Entity<Proyecto>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.creadoPor);

            modelBuilder.Entity<Prueba>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.creadoPor);

            modelBuilder.Entity<Alumno>()
                .HasOne(a => a.Tutor)
                .WithMany(t => t.Alumnos)
                .HasForeignKey(a => a.idTutor);

            modelBuilder.Entity<Proyecto>()
                .HasOne(p => p.CampoFormativo)
                .WithMany(c => c.Proyectos)
                .HasForeignKey(p => p.idCampo);

            modelBuilder.Entity<DocenteAlumno>()
                .HasOne(da => da.Docente)
                .WithMany()
                .HasForeignKey(da => da.idDocente)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DocenteAlumno>()
                .HasOne(da => da.Alumno)
                .WithMany(a => a.DocenteAlumnos)
                .HasForeignKey(da => da.idAlumno)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Resultado>()
                .HasOne(r => r.Alumno)
                .WithMany(a => a.Resultados)
                .HasForeignKey(r => r.idAlumno);

            modelBuilder.Entity<Resultado>()
                .HasOne(r => r.Prueba)
                .WithMany()
                .HasForeignKey(r => r.idPrueba);

            modelBuilder.Entity<ChatHistorial>()
                .HasOne(ch => ch.Alumno)
                .WithMany(a => a.ChatHistorial)
                .HasForeignKey(ch => ch.idAlumno);

            base.OnModelCreating(modelBuilder);
        }
    }
}


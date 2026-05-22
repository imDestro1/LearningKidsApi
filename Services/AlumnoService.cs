using LearningKidsAPI.Data;
using LearningKidsAPI.DTOs;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class AlumnoService
    {
        private const string AlumnoRoleName = "Alumno";
        private readonly AppDbContext _context;

        public AlumnoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Alumno>> GetAllAsync()
        {
            return await _context.Alumnos
                .Include(a => a.Tutor)
                .Include(a => a.Usuario)
                    .ThenInclude(u => u.Rol)
                .ToListAsync();
        }

        public async Task<Alumno?> GetByIdAsync(int id)
        {
            return await _context.Alumnos
                .Include(a => a.Tutor)
                .Include(a => a.Usuario)
                    .ThenInclude(u => u.Rol)
                .FirstOrDefaultAsync(a => a.idAlumno == id);
        }

        public async Task<Alumno> CreateAsync(Alumno alumno)
        {
            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();
            return alumno;
        }

        public async Task<Alumno> RegisterAsync(AlumnoRegistroDTO registro)
        {
            var cleanUsername = registro.username?.Trim() ?? string.Empty;

            var alumnoRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.nombre != null && r.nombre.ToLower() == AlumnoRoleName.ToLower());

            if (alumnoRole == null)
            {
                throw new InvalidOperationException("No existe el rol Alumno.");
            }

            var usernameExists = await _context.Usuarios
                .AnyAsync(u => u.username == cleanUsername);

            if (usernameExists)
            {
                throw new InvalidOperationException("El username ya existe.");
            }

            if (registro.idTutor.HasValue)
            {
                var tutorExists = await _context.Tutores
                    .AnyAsync(t => t.idTutor == registro.idTutor.Value);

                if (!tutorExists)
                {
                    throw new InvalidOperationException("El tutor indicado no existe.");
                }
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var usuario = new Usuario
            {
                nombre = registro.nombre?.Trim(),
                username = cleanUsername,
                password = registro.password?.Trim(),
                idRol = alumnoRole.idRol
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var alumno = new Alumno
            {
                idAlumno = usuario.idUsuario,
                idTutor = registro.idTutor,
                grado = registro.grado
            };

            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetByIdAsync(alumno.idAlumno) ?? alumno;
        }

        public async Task UpdateAsync(Alumno alumno)
        {
            _context.Alumnos.Update(alumno);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Alumno alumno)
        {
            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();
        }
    }
}

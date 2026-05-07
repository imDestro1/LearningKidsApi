using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.idUsuario == id);
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

<<<<<<< HEAD
        public async Task<Usuario?> ValidateCredentialsAsync(string? username, string? password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.username == username && u.password == password);
        }

        public async Task<Usuario?> ValidateAlumnoCredentialsAsync(string? username, string? password)
        {
            var usuario = await ValidateCredentialsAsync(username, password);
            if (usuario == null)
            {
                return null;
            }

            return IsRole(usuario, "Alumno") ? usuario : null;
        }

        public async Task<Usuario?> ValidateDocenteOrAdminCredentialsAsync(string? username, string? password)
        {
            var usuario = await ValidateCredentialsAsync(username, password);
            if (usuario == null)
            {
                return null;
            }

            return IsRole(usuario, "Docente") || IsRole(usuario, "Administrador") ? usuario : null;
        }

        private static bool IsRole(Usuario usuario, string roleName)
        {
            return string.Equals(usuario.Rol?.nombre, roleName, StringComparison.OrdinalIgnoreCase);
=======
        public async Task<Usuario?> LoginAlumnoAsync(string username, string password)
        {
            var cleanUsername = username.Trim();
            var cleanPassword = password.Trim();

            return await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.username != null && u.password != null)
                .Where(u => u.username == cleanUsername && u.password == cleanPassword)
                .Where(u => _context.Alumnos.Any(a => a.idAlumno == u.idUsuario))
                .FirstOrDefaultAsync();
>>>>>>> origin/featureAI
        }
    }
}
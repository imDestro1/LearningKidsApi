using LearningKidsAPI.Data;
using LearningKidsAPI.DTOs;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class DocenteService
    {
        private readonly AppDbContext _context;
        private const string DocenteRoleName = "Docente";

        public DocenteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DocenteDTO>> GetAllAsync()
        {
            var docenteRoleId = await GetDocenteRoleIdAsync();
            return await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.idRol == docenteRoleId)
                .Select(u => new DocenteDTO
                {
                    idUsuario = u.idUsuario,
                    nombre = u.nombre,
                    username = u.username,
                    idRol = u.idRol,
                    rolNombre = u.Rol != null ? u.Rol.nombre : null
                })
                .ToListAsync();
        }

        public async Task<DocenteDTO?> GetByIdAsync(int id)
        {
            var docenteRoleId = await GetDocenteRoleIdAsync();
            return await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.idUsuario == id && u.idRol == docenteRoleId)
                .Select(u => new DocenteDTO
                {
                    idUsuario = u.idUsuario,
                    nombre = u.nombre,
                    username = u.username,
                    idRol = u.idRol,
                    rolNombre = u.Rol != null ? u.Rol.nombre : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsDocenteRoleAsync(int? roleId)
        {
            var docenteRoleId = await GetDocenteRoleIdAsync();
            return docenteRoleId.HasValue && roleId == docenteRoleId.Value;
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

        private async Task<int?> GetDocenteRoleIdAsync()
        {
            return await _context.Roles
                .Where(r => r.nombre == DocenteRoleName)
                .Select(r => (int?)r.idRol)
                .FirstOrDefaultAsync();
        }
    }
}
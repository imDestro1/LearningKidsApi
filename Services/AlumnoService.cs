using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class AlumnoService
    {
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
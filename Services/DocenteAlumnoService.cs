using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class DocenteAlumnoService
    {
        private readonly AppDbContext _context;

        public DocenteAlumnoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DocenteAlumno>> GetAllAsync()
        {
            return await _context.DocenteAlumnos
                .Include(da => da.Docente)
                .Include(da => da.Alumno)
                .ToListAsync();
        }

        public async Task<DocenteAlumno?> GetByIdAsync(int id)
        {
            return await _context.DocenteAlumnos
                .Include(da => da.Docente)
                .Include(da => da.Alumno)
                .Where(da => da.id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<DocenteAlumno> CreateAsync(DocenteAlumno docenteAlumno)
        {
            _context.DocenteAlumnos.Add(docenteAlumno);
            await _context.SaveChangesAsync();
            return docenteAlumno;
        }

        public async Task UpdateAsync(DocenteAlumno docenteAlumno)
        {
            _context.DocenteAlumnos.Update(docenteAlumno);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DocenteAlumno docenteAlumno)
        {
            _context.DocenteAlumnos.Remove(docenteAlumno);
            await _context.SaveChangesAsync();
        }
    }
}
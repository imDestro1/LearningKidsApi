using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using LearningKidsAPI.DTOs;
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

        public async Task<List<DocenteAlumnoDTO>> GetAllAsync()
        {
            return await _context.DocenteAlumnos
                .Select(da => new DocenteAlumnoDTO
                {
                    id = da.id,
                    idDocente = da.idDocente,
                    idAlumno = da.idAlumno,
                    nombreDocente = null, // No acceder a navegación para evitar ciclos
                    nombreAlumno = null   // No acceder a navegación para evitar ciclos
                })
                .ToListAsync();
        }

        public async Task<DocenteAlumnoDTO?> GetByIdAsync(int id)
        {
            return await _context.DocenteAlumnos
                .Where(da => da.id == id)
                .Select(da => new DocenteAlumnoDTO
                {
                    id = da.id,
                    idDocente = da.idDocente,
                    idAlumno = da.idAlumno,
                    nombreDocente = null, // No acceder a navegación para evitar ciclos
                    nombreAlumno = null   // No acceder a navegación para evitar ciclos
                })
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
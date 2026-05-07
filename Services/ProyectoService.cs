using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class ProyectoService
    {
        private readonly AppDbContext _context;

        public ProyectoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proyecto>> GetAllAsync()
        {
            if (usuarioId <= 0)
            {
                return await _context.Proyectos
                    .Include(p => p.Temas)
                    .Include(p => p.CampoFormativo)
                    .Include(p => p.Usuario)
                    .ToListAsync();
            }

            return await _context.Proyectos
                .Include(p => p.Temas)
                .Include(p => p.CampoFormativo)
                .Include(p => p.Usuario)
                .ToListAsync();
        }

        public async Task<Proyecto?> GetByIdAsync(int id)
        {
            return await _context.Proyectos
                .Include(p => p.Temas)
                .Include(p => p.CampoFormativo)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.idProyecto == id);
        }

        public async Task<Proyecto> CreateAsync(Proyecto proyecto)
        {
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();
            return proyecto;
        }

        public async Task UpdateAsync(Proyecto proyecto)
        {
            _context.Proyectos.Update(proyecto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Proyecto proyecto)
        {
            _context.Proyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
        }
    }
}

using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class TemaService
    {
        private readonly AppDbContext _context;

        public TemaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tema>> GetAllAsync(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                return await _context.Temas
                    .Include(t => t.Proyecto)
                    .Include(t => t.Pruebas)
                    .ToListAsync();
            }

            return await _context.Temas
                .Include(t => t.Proyecto)
                .Include(t => t.Pruebas)
                .Where(t => t.Proyecto != null && t.Proyecto.creadoPor == usuarioId)
                .ToListAsync();
        }

        public async Task<Tema?> GetByIdAsync(int id)
        {
            return await _context.Temas
                .Include(t => t.Proyecto)
                .Include(t => t.Pruebas)
                .FirstOrDefaultAsync(t => t.idTema == id);
        }

        public async Task<Tema> CreateAsync(Tema tema)
        {
            _context.Temas.Add(tema);
            await _context.SaveChangesAsync();
            return tema;
        }

        public async Task UpdateAsync(Tema tema)
        {
            _context.Temas.Update(tema);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tema tema)
        {
            _context.Temas.Remove(tema);
            await _context.SaveChangesAsync();
        }
    }
}

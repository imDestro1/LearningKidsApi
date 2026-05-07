using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class PruebaService
    {
        private readonly AppDbContext _context;

        public PruebaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Prueba>> GetAllAsync()
        {
            if (usuarioId <= 0)
            {
                return await _context.Pruebas
                    .Include(p => p.Tema)
                    .Include(p => p.Preguntas)
                    .ThenInclude(q => q.Respuestas)
                    .Include(p => p.Usuario)
                    .ToListAsync();
            }

            return await _context.Pruebas
                .Include(p => p.Tema)
                .Include(p => p.Preguntas)
<<<<<<< HEAD
                    .ThenInclude(q => q.Respuestas)
=======
                .ThenInclude(q => q.Respuestas)
>>>>>>> origin/featureAI
                .Include(p => p.Usuario)
                .ToListAsync();
        }

        public async Task<Prueba?> GetByIdAsync(int id)
        {
            return await _context.Pruebas
                .Include(p => p.Tema)
                .Include(p => p.Preguntas)
<<<<<<< HEAD
                    .ThenInclude(q => q.Respuestas)
=======
                .ThenInclude(q => q.Respuestas)
>>>>>>> origin/featureAI
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.idPrueba == id);
        }

        public async Task<Prueba> CreateAsync(Prueba prueba)
        {
            _context.Pruebas.Add(prueba);
            await _context.SaveChangesAsync();
            return prueba;
        }

        public async Task UpdateAsync(Prueba prueba)
        {
            _context.Pruebas.Update(prueba);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Prueba prueba)
        {
            _context.Pruebas.Remove(prueba);
            await _context.SaveChangesAsync();
        }
    }
}

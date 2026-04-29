using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class PreguntaService
    {
        private readonly AppDbContext _context;

        public PreguntaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pregunta>> GetAllAsync()
        {
            return await _context.Preguntas
                .Include(p => p.Prueba)
                .Include(p => p.Respuestas)
                .ToListAsync();
        }

        public async Task<Pregunta?> GetByIdAsync(int id)
        {
            return await _context.Preguntas
                .Include(p => p.Prueba)
                .Include(p => p.Respuestas)
                .FirstOrDefaultAsync(p => p.idPregunta == id);
        }

        public async Task<Pregunta> CreateAsync(Pregunta pregunta)
        {
            _context.Preguntas.Add(pregunta);
            await _context.SaveChangesAsync();
            return pregunta;
        }

        public async Task UpdateAsync(Pregunta pregunta)
        {
            _context.Preguntas.Update(pregunta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Pregunta pregunta)
        {
            _context.Preguntas.Remove(pregunta);
            await _context.SaveChangesAsync();
        }
    }
}
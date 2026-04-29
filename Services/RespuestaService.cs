using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class RespuestaService
    {
        private readonly AppDbContext _context;

        public RespuestaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Respuesta>> GetAllAsync()
        {
            return await _context.Respuestas
                .Include(r => r.Pregunta)
                .ToListAsync();
        }

        public async Task<Respuesta?> GetByIdAsync(int id)
        {
            return await _context.Respuestas
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(r => r.idRespuesta == id);
        }

        public async Task<Respuesta> CreateAsync(Respuesta respuesta)
        {
            _context.Respuestas.Add(respuesta);
            await _context.SaveChangesAsync();
            return respuesta;
        }

        public async Task UpdateAsync(Respuesta respuesta)
        {
            _context.Respuestas.Update(respuesta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Respuesta respuesta)
        {
            _context.Respuestas.Remove(respuesta);
            await _context.SaveChangesAsync();
        }
    }
}
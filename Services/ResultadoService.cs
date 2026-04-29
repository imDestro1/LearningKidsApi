using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class ResultadoService
    {
        private readonly AppDbContext _context;

        public ResultadoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Resultado>> GetAllAsync()
        {
            return await _context.Resultados
                .Include(r => r.Alumno)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Prueba)
                .ToListAsync();
        }

        public async Task<Resultado?> GetByIdAsync(int id)
        {
            return await _context.Resultados
                .Include(r => r.Alumno)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Prueba)
                .FirstOrDefaultAsync(r => r.idResultado == id);
        }

        public async Task<Resultado> CreateAsync(Resultado resultado)
        {
            _context.Resultados.Add(resultado);
            await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task UpdateAsync(Resultado resultado)
        {
            _context.Resultados.Update(resultado);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Resultado resultado)
        {
            _context.Resultados.Remove(resultado);
            await _context.SaveChangesAsync();
        }
    }
}
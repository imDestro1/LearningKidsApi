using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class CampoFormativoService
    {
        private readonly AppDbContext _context;

        public CampoFormativoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CampoFormativo>> GetAllAsync()
        {
            return await _context.CamposFormativos
                .Include(c => c.Proyectos)
                .ToListAsync();
        }

        public async Task<CampoFormativo?> GetByIdAsync(int id)
        {
            return await _context.CamposFormativos
                .Include(c => c.Proyectos)
                .FirstOrDefaultAsync(c => c.idCampo == id);
        }

        public async Task<CampoFormativo> CreateAsync(CampoFormativo campoFormativo)
        {
            _context.CamposFormativos.Add(campoFormativo);
            await _context.SaveChangesAsync();
            return campoFormativo;
        }

        public async Task UpdateAsync(CampoFormativo campoFormativo)
        {
            _context.CamposFormativos.Update(campoFormativo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CampoFormativo campoFormativo)
        {
            _context.CamposFormativos.Remove(campoFormativo);
            await _context.SaveChangesAsync();
        }
    }
}
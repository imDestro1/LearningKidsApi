using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class TutorService
    {
        private readonly AppDbContext _context;

        public TutorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tutor>> GetAllAsync()
        {
            return await _context.Tutores
                .Include(t => t.Alumnos)
                .ToListAsync();
        }

        public async Task<Tutor?> GetByIdAsync(int id)
        {
            return await _context.Tutores
                .Include(t => t.Alumnos)
                .FirstOrDefaultAsync(t => t.idTutor == id);
        }

        public async Task<Tutor> CreateAsync(Tutor tutor)
        {
            _context.Tutores.Add(tutor);
            await _context.SaveChangesAsync();
            return tutor;
        }

        public async Task UpdateAsync(Tutor tutor)
        {
            _context.Tutores.Update(tutor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tutor tutor)
        {
            _context.Tutores.Remove(tutor);
            await _context.SaveChangesAsync();
        }
    }
}
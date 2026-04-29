using LearningKidsAPI.Data;
using LearningKidsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningKidsAPI.Services
{
    public class ChatHistorialService
    {
        private readonly AppDbContext _context;

        public ChatHistorialService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChatHistorial>> GetAllAsync()
        {
            return await _context.ChatHistorial
                .Include(ch => ch.Alumno)
                    .ThenInclude(a => a.Usuario)
                .ToListAsync();
        }

        public async Task<ChatHistorial?> GetByIdAsync(int id)
        {
            return await _context.ChatHistorial
                .Include(ch => ch.Alumno)
                    .ThenInclude(a => a.Usuario)
                .FirstOrDefaultAsync(ch => ch.idChat == id);
        }

        public async Task<ChatHistorial> CreateAsync(ChatHistorial chatHistorial)
        {
            _context.ChatHistorial.Add(chatHistorial);
            await _context.SaveChangesAsync();
            return chatHistorial;
        }

        public async Task UpdateAsync(ChatHistorial chatHistorial)
        {
            _context.ChatHistorial.Update(chatHistorial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ChatHistorial chatHistorial)
        {
            _context.ChatHistorial.Remove(chatHistorial);
            await _context.SaveChangesAsync();
        }
    }
}
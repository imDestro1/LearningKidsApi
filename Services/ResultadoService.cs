using LearningKidsAPI.Data;
using LearningKidsAPI.DTOs;
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

        /// <summary>
        /// Calcula los aciertos del alumno, guarda el resultado y devuelve el registro creado.
        /// La calificación es el porcentaje de respuestas correctas (0-100).
        /// </summary>
        public async Task<(Resultado resultado, int aciertos, int totalPreguntas)> SubmitPruebaAsync(SubmitPruebaRequest request)
        {
            // Cargar la prueba con sus preguntas y respuestas
            var prueba = await _context.Pruebas
                .Include(p => p.Preguntas)
                    .ThenInclude(q => q.Respuestas)
                .FirstOrDefaultAsync(p => p.idPrueba == request.idPrueba)
                ?? throw new KeyNotFoundException($"Prueba {request.idPrueba} no encontrada.");

            int totalPreguntas = prueba.Preguntas.Count;

            if (totalPreguntas == 0)
                throw new InvalidOperationException("La prueba no tiene preguntas.");

            // Obtener las respuestas seleccionadas de la BD para verificar esCorrecta
            var respuestasSeleccionadas = await _context.Respuestas
                .Where(r => request.respuestasSeleccionadas.Contains(r.idRespuesta))
                .ToListAsync();

            int aciertos = respuestasSeleccionadas.Count(r => r.esCorrecta == true);

            decimal calificacion = Math.Round((decimal)aciertos / totalPreguntas * 100, 2);

            var resultado = new Resultado
            {
                idAlumno = request.idAlumno,
                idPrueba = request.idPrueba,
                calificacion = calificacion,
                fecha = DateTime.UtcNow
            };

            _context.Resultados.Add(resultado);
            await _context.SaveChangesAsync();

            return (resultado, aciertos, totalPreguntas);
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

        public async Task<List<Resultado>> GetByAlumnoIdAsync(int idAlumno)
        {
            return await _context.Resultados
                .Include(r => r.Alumno)
                    .ThenInclude(a => a.Usuario)
                .Include(r => r.Prueba)
                .Where(r => r.idAlumno == idAlumno)
                .OrderByDescending(r => r.fecha)
                .ToListAsync();
        }

        public async Task<Resultado?> GetByAlumnoAndPruebaAsync(int idAlumno, int idPrueba)
        {
            return await _context.Resultados
                .Include(r => r.Prueba)
                .Where(r => r.idAlumno == idAlumno && r.idPrueba == idPrueba)
                .OrderByDescending(r => r.fecha)
                .FirstOrDefaultAsync();
        }

        public async Task<Resultado> CreateAsync(Resultado resultado)
        {
            _context.Resultados.Add(resultado);
            await _context.SaveChangesAsync();
            return resultado;
        }

        public async Task UpdateAsync(Resultado resultado)
        {
            var tracked = await _context.Resultados.FindAsync(resultado.idResultado);

            if (tracked == null)
            {
                _context.Resultados.Update(resultado);
            }
            else
            {
                _context.Entry(tracked).CurrentValues.SetValues(resultado);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Resultado resultado)
        {
            _context.Resultados.Remove(resultado);
            await _context.SaveChangesAsync();
        }
    }
}
using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using LearningKidsAPI.DTOs;
using LearningKidsAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocenteAlumnosController : ControllerBase
    {
        private readonly DocenteAlumnoService _docenteAlumnoService;
        private readonly AppDbContext _context;

        public DocenteAlumnosController(DocenteAlumnoService docenteAlumnoService, AppDbContext context)
        {
            _docenteAlumnoService = docenteAlumnoService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var docenteAlumnos = await _docenteAlumnoService.GetAllAsync();
            return Ok(docenteAlumnos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var docenteAlumno = await _docenteAlumnoService.GetByIdAsync(id);
            if (docenteAlumno == null)
            {
                return NotFound();
            }

            return Ok(docenteAlumno);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DocenteAlumno docenteAlumno)
        {
            var created = await _docenteAlumnoService.CreateAsync(docenteAlumno);
            return CreatedAtAction(nameof(Get), new { id = created.id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] DocenteAlumno docenteAlumno)
        {
            // Verificar si existe
            var existingDto = await _docenteAlumnoService.GetByIdAsync(id);
            if (existingDto == null)
            {
                return NotFound();
            }

            docenteAlumno.id = id;
            await _docenteAlumnoService.UpdateAsync(docenteAlumno);

            return Ok(docenteAlumno);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Verificar si existe
            var existingDto = await _docenteAlumnoService.GetByIdAsync(id);
            if (existingDto == null)
            {
                return NotFound();
            }

            // Obtener el objeto completo para eliminar
            var docenteAlumnoToDelete = await _context.DocenteAlumnos.FindAsync(id);
            if (docenteAlumnoToDelete == null)
            {
                return NotFound();
            }

            await _docenteAlumnoService.DeleteAsync(docenteAlumnoToDelete);
            return Ok();
        }
    }
}
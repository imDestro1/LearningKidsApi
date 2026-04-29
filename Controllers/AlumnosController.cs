using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnosController : ControllerBase
    {
        private readonly AlumnoService _alumnoService;

        public AlumnosController(AlumnoService alumnoService)
        {
            _alumnoService = alumnoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var alumnos = await _alumnoService.GetAllAsync();
            return Ok(alumnos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var alumno = await _alumnoService.GetByIdAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }

            return Ok(alumno);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Alumno alumno)
        {
            var created = await _alumnoService.CreateAsync(alumno);
            return CreatedAtAction(nameof(Get), new { id = created.idAlumno }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Alumno alumno)
        {
            var existing = await _alumnoService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            alumno.idAlumno = id;
            await _alumnoService.UpdateAsync(alumno);

            return Ok(alumno);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _alumnoService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _alumnoService.DeleteAsync(existing);
            return Ok();
        }
    }
}
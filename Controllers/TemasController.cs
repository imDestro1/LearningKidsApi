using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemasController : ControllerBase
    {
        private const int AuthenticatedUserId = 1;
        private readonly TemaService _temaService;
        private readonly ProyectoService _proyectoService;

        public TemasController(TemaService temaService, ProyectoService proyectoService)
        {
            _temaService = temaService;
            _proyectoService = proyectoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var temas = await _temaService.GetAllAsync(0);
            return Ok(temas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var tema = await _temaService.GetByIdAsync(id);
            if (tema == null)
            {
                return NotFound();
            }

            return Ok(tema);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Tema tema)
        {
            var proyecto = await _proyectoService.GetByIdAsync(tema.idProyecto ?? 0);
            if (proyecto == null)
            {
                return NotFound(new { Message = "Proyecto no encontrado." });
            }

            if (proyecto.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            var created = await _temaService.CreateAsync(tema);
            return CreatedAtAction(nameof(Get), new { id = created.idTema }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Tema tema)
        {
            var existing = await _temaService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (existing.Proyecto?.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            if (tema.idProyecto != existing.idProyecto)
            {
                var proyecto = await _proyectoService.GetByIdAsync(tema.idProyecto ?? 0);
                if (proyecto == null || proyecto.creadoPor != AuthenticatedUserId)
                {
                    return Unauthorized();
                }
            }

            existing.nombre = tema.nombre;
            existing.descripcion = tema.descripcion;
            existing.idProyecto = tema.idProyecto;
            await _temaService.UpdateAsync(existing);

            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _temaService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (existing.Proyecto?.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            await _temaService.DeleteAsync(existing);
            return Ok();
        }
    }
}

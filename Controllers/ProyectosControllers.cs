using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectosController : ControllerBase
    {
        private const int AuthenticatedUserId = 1;
        private readonly ProyectoService _proyectoService;

        public ProyectosController(ProyectoService proyectoService)
        {
            _proyectoService = proyectoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var proyectos = await _proyectoService.GetAllAsync(AuthenticatedUserId);
            return Ok(proyectos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var proyecto = await _proyectoService.GetByIdAsync(id);
            if (proyecto == null)
            {
                return NotFound();
            }

            if (proyecto.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            return Ok(proyecto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Proyecto proyecto)
        {
            proyecto.creadoPor = AuthenticatedUserId;
            var created = await _proyectoService.CreateAsync(proyecto);
            return CreatedAtAction(nameof(Get), new { id = created.idProyecto }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Proyecto proyecto)
        {
            var existing = await _proyectoService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (existing.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            existing.nombre = proyecto.nombre;
            existing.descripcion = proyecto.descripcion;
            existing.grado = proyecto.grado;
            existing.idCampo = proyecto.idCampo;
            await _proyectoService.UpdateAsync(existing);

            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _proyectoService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (existing.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            await _proyectoService.DeleteAsync(existing);
            return Ok();
        }
    }
}

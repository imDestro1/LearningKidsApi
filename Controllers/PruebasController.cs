using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PruebasController : ControllerBase
    {
        private const int AuthenticatedUserId = 1;
        private readonly PruebaService _pruebaService;
        private readonly TemaService _temaService;

        public PruebasController(PruebaService pruebaService, TemaService temaService)
        {
            _pruebaService = pruebaService;
            _temaService = temaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pruebas = await _pruebaService.GetAllAsync(0);
            return Ok(pruebas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var prueba = await _pruebaService.GetByIdAsync(id);
            if (prueba == null)
            {
                return NotFound();
            }

            return Ok(prueba);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Prueba prueba)
        {
            var tema = await _temaService.GetByIdAsync(prueba.idTema ?? 0);
            if (tema == null)
            {
                return NotFound(new { Message = "Tema no encontrado." });
            }

            if (tema.Proyecto?.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            prueba.creadoPor = AuthenticatedUserId;
            var created = await _pruebaService.CreateAsync(prueba);
            return CreatedAtAction(nameof(Get), new { id = created.idPrueba }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Prueba prueba)
        {
            var existing = await _pruebaService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (existing.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            if (prueba.idTema != existing.idTema)
            {
                var tema = await _temaService.GetByIdAsync(prueba.idTema ?? 0);
                if (tema == null || tema.Proyecto?.creadoPor != AuthenticatedUserId)
                {
                    return Unauthorized();
                }
            }

            existing.titulo = prueba.titulo;
            existing.idTema = prueba.idTema;
            await _pruebaService.UpdateAsync(existing);

            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _pruebaService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (existing.creadoPor != AuthenticatedUserId)
            {
                return Unauthorized();
            }

            await _pruebaService.DeleteAsync(existing);
            return Ok();
        }
    }
}

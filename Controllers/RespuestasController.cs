using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RespuestasController : ControllerBase
    {
        private readonly RespuestaService _respuestaService;

        public RespuestasController(RespuestaService respuestaService)
        {
            _respuestaService = respuestaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var respuestas = await _respuestaService.GetAllAsync();
            return Ok(respuestas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var respuesta = await _respuestaService.GetByIdAsync(id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return Ok(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Respuesta respuesta)
        {
            var created = await _respuestaService.CreateAsync(respuesta);
            return CreatedAtAction(nameof(Get), new { id = created.idRespuesta }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Respuesta respuesta)
        {
            var existing = await _respuestaService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            respuesta.idRespuesta = id;
            await _respuestaService.UpdateAsync(respuesta);

            return Ok(respuesta);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _respuestaService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _respuestaService.DeleteAsync(existing);
            return Ok();
        }
    }
}
using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreguntasController : ControllerBase
    {
        private readonly PreguntaService _preguntaService;

        public PreguntasController(PreguntaService preguntaService)
        {
            _preguntaService = preguntaService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var preguntas = await _preguntaService.GetAllAsync();
            return Ok(preguntas);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var pregunta = await _preguntaService.GetByIdAsync(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return Ok(pregunta);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Pregunta pregunta)
        {
            var created = await _preguntaService.CreateAsync(pregunta);
            return CreatedAtAction(nameof(Get), new { id = created.idPregunta }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Pregunta pregunta)
        {
            var existing = await _preguntaService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            pregunta.idPregunta = id;
            await _preguntaService.UpdateAsync(pregunta);

            return Ok(pregunta);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _preguntaService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _preguntaService.DeleteAsync(existing);
            return Ok();
        }
    }
}
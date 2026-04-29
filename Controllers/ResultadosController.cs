using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultadosController : ControllerBase
    {
        private readonly ResultadoService _resultadoService;

        public ResultadosController(ResultadoService resultadoService)
        {
            _resultadoService = resultadoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resultados = await _resultadoService.GetAllAsync();
            return Ok(resultados);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var resultado = await _resultadoService.GetByIdAsync(id);
            if (resultado == null)
            {
                return NotFound();
            }

            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Resultado resultado)
        {
            var created = await _resultadoService.CreateAsync(resultado);
            return CreatedAtAction(nameof(Get), new { id = created.idResultado }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Resultado resultado)
        {
            var existing = await _resultadoService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            resultado.idResultado = id;
            await _resultadoService.UpdateAsync(resultado);

            return Ok(resultado);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _resultadoService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _resultadoService.DeleteAsync(existing);
            return Ok();
        }
    }
}
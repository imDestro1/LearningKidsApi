using LearningKidsAPI.DTOs;
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

        [HttpGet("alumno/{idAlumno:int}")]
        public async Task<IActionResult> GetByAlumno(int idAlumno)
        {
            var resultados = await _resultadoService.GetByAlumnoIdAsync(idAlumno);
            return Ok(resultados);
        }

        [HttpGet("alumno/{idAlumno:int}/prueba/{idPrueba:int}")]
        public async Task<IActionResult> GetByAlumnoAndPrueba(int idAlumno, int idPrueba)
        {
            var resultado = await _resultadoService.GetByAlumnoAndPruebaAsync(idAlumno, idPrueba);
            if (resultado == null)
                return NotFound();

            return Ok(new
            {
                resultado.idResultado,
                resultado.idAlumno,
                resultado.idPrueba,
                resultado.calificacion,
                resultado.fecha,
                tituloPrueba = resultado.Prueba != null ? resultado.Prueba.titulo : null
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Resultado resultado)
        {
            var created = await _resultadoService.CreateAsync(resultado);
            return CreatedAtAction(nameof(Get), new { id = created.idResultado }, created);
        }

        /// <summary>
        /// Envía las respuestas del alumno, calcula los aciertos y guarda el resultado.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody] SubmitPruebaRequest request)
        {
            try
            {
                var (resultado, aciertos, totalPreguntas) = await _resultadoService.SubmitPruebaAsync(request);
                return CreatedAtAction(nameof(Get), new { id = resultado.idResultado }, new
                {
                    resultado.idResultado,
                    resultado.idAlumno,
                    resultado.idPrueba,
                    resultado.calificacion,
                    resultado.fecha,
                    aciertos,
                    totalPreguntas
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
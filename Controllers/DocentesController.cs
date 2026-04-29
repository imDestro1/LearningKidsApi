using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocentesController : ControllerBase
    {
        private readonly DocenteService _docenteService;

        public DocentesController(DocenteService docenteService)
        {
            _docenteService = docenteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var docentes = await _docenteService.GetAllAsync();
            return Ok(docentes);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var docente = await _docenteService.GetByIdAsync(id);
            if (docente == null)
            {
                return NotFound();
            }

            return Ok(docente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario docente)
        {
            if (!await _docenteService.IsDocenteRoleAsync(docente.idRol))
            {
                return BadRequest("El usuario debe tener el rol de docente.");
            }

            var created = await _docenteService.CreateAsync(docente);
            return CreatedAtAction(nameof(Get), new { id = created.idUsuario }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario docente)
        {
            var existing = await _docenteService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            if (!await _docenteService.IsDocenteRoleAsync(docente.idRol))
            {
                return BadRequest("El usuario debe tener el rol de docente.");
            }

            docente.idUsuario = id;
            await _docenteService.UpdateAsync(docente);
            return Ok(docente);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _docenteService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            var toDelete = new Usuario { idUsuario = id };
            await _docenteService.DeleteAsync(toDelete);
            return Ok();
        }
    }
}
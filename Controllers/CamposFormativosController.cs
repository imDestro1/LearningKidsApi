using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CamposFormativosController : ControllerBase
    {
        private readonly CampoFormativoService _campoFormativoService;

        public CamposFormativosController(CampoFormativoService campoFormativoService)
        {
            _campoFormativoService = campoFormativoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var camposFormativos = await _campoFormativoService.GetAllAsync();
            return Ok(camposFormativos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var campoFormativo = await _campoFormativoService.GetByIdAsync(id);
            if (campoFormativo == null)
            {
                return NotFound();
            }

            return Ok(campoFormativo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CampoFormativo campoFormativo)
        {
            var created = await _campoFormativoService.CreateAsync(campoFormativo);
            return CreatedAtAction(nameof(Get), new { id = created.idCampo }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] CampoFormativo campoFormativo)
        {
            var existing = await _campoFormativoService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            campoFormativo.idCampo = id;
            await _campoFormativoService.UpdateAsync(campoFormativo);

            return Ok(campoFormativo);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _campoFormativoService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _campoFormativoService.DeleteAsync(existing);
            return Ok();
        }
    }
}
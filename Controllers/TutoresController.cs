using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutoresController : ControllerBase
    {
        private readonly TutorService _tutorService;

        public TutoresController(TutorService tutorService)
        {
            _tutorService = tutorService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tutores = await _tutorService.GetAllAsync();
            return Ok(tutores);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var tutor = await _tutorService.GetByIdAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }

            return Ok(tutor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Tutor tutor)
        {
            var created = await _tutorService.CreateAsync(tutor);
            return CreatedAtAction(nameof(Get), new { id = created.idTutor }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Tutor tutor)
        {
            var existing = await _tutorService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            // 🔥 actualizar SOLO campos necesarios
            existing.nombre = tutor.nombre;
            existing.correo = tutor.correo;

            await _tutorService.UpdateAsync(existing);

            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _tutorService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _tutorService.DeleteAsync(existing);
            return Ok();
        }
    }
}
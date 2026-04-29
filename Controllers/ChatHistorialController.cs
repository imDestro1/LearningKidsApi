using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatHistorialController : ControllerBase
    {
        private readonly ChatHistorialService _chatHistorialService;

        public ChatHistorialController(ChatHistorialService chatHistorialService)
        {
            _chatHistorialService = chatHistorialService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var chatHistorial = await _chatHistorialService.GetAllAsync();
            return Ok(chatHistorial);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var chat = await _chatHistorialService.GetByIdAsync(id);
            if (chat == null)
            {
                return NotFound();
            }

            return Ok(chat);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatHistorial chatHistorial)
        {
            var created = await _chatHistorialService.CreateAsync(chatHistorial);
            return CreatedAtAction(nameof(Get), new { id = created.idChat }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ChatHistorial chatHistorial)
        {
            var existing = await _chatHistorialService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            chatHistorial.idChat = id;
            await _chatHistorialService.UpdateAsync(chatHistorial);

            return Ok(chatHistorial);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _chatHistorialService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _chatHistorialService.DeleteAsync(existing);
            return Ok();
        }
    }
}
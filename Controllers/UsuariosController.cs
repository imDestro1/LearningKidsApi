using LearningKidsAPI.Models;
using LearningKidsAPI.Services;
using LearningKidsAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LearningKidsAPI.Controllers
{
    public record LoginAlumnoRequest(string? username, string? password);

    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            var created = await _usuarioService.CreateAsync(usuario);
            return CreatedAtAction(nameof(Get), new { id = created.idUsuario }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
        {
            var existing = await _usuarioService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            usuario.idUsuario = id;
            await _usuarioService.UpdateAsync(usuario);

            return Ok(usuario);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _usuarioService.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _usuarioService.DeleteAsync(existing);
            return Ok();
        }

        [HttpPost("login/alumnos")]
<<<<<<< HEAD
        public async Task<IActionResult> LoginAlumnos([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.username) || string.IsNullOrEmpty(loginRequest.password))
            {
                return BadRequest(new { Message = "Usuario y contraseña son requeridos." });
            }

            var usuario = await _usuarioService.ValidateAlumnoCredentialsAsync(loginRequest.username, loginRequest.password);
            if (usuario == null)
            {
                return Unauthorized(new { Message = "Credenciales inválidas para acceso de alumno." });
            }

            return Ok(new { Message = "Login de alumno exitoso.", usuario });
        }

        [HttpPost("login/staff")]
        public async Task<IActionResult> LoginStaff([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.username) || string.IsNullOrEmpty(loginRequest.password))
            {
                return BadRequest(new { Message = "Usuario y contraseña son requeridos." });
            }

            var usuario = await _usuarioService.ValidateDocenteOrAdminCredentialsAsync(loginRequest.username, loginRequest.password);
            if (usuario == null)
            {
                return Unauthorized(new { Message = "Credenciales inválidas para acceso de docentes/administradores." });
            }

            return Ok(new { Message = "Login de staff exitoso.", usuario });
=======
        public async Task<IActionResult> LoginAlumnos([FromBody] LoginAlumnoRequest request)
        {
            var username = request.username?.Trim() ?? string.Empty;
            var password = request.password?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest(new { message = "Ingresa tu username y tu contraseña." });
            }

            var usuario = await _usuarioService.LoginAlumnoAsync(username, password);
            if (usuario == null)
            {
                return Unauthorized(new { message = "Usuario o contraseña incorrectos." });
            }

            return Ok(new
            {
                message = "Inicio de sesión exitoso.",
                usuario = new
                {
                    usuario.idUsuario,
                    usuario.nombre,
                    usuario.username,
                    usuario.idRol
                }
            });
>>>>>>> origin/featureAI
        }
    }
}
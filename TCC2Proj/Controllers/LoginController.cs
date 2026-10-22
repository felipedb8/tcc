using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TCC2Proj.Data;
using TCC2Proj.Models;

namespace TCC2Proj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Logar([FromBody] LoginRequest dadosLogin)
        {
            if (dadosLogin == null || string.IsNullOrEmpty(dadosLogin.Email) || string.IsNullOrEmpty(dadosLogin.Senha))
            {
                return BadRequest("E-mail e senha são obrigatórios.");
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dadosLogin.Email);

            // Se o usuário não existir
            if (usuario == null)
            {
                return Unauthorized("E-mail ou senha incorretos.");
            }

            if (usuario.Senha != dadosLogin.Senha)
            {
                return Unauthorized("E-mail ou senha incorretos.");
            }

            return Ok(new
            {
                id = usuario.Id,
                nome = usuario.Nome,
                email = usuario.Email
            });
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] Usuarios novoUsuario)
        {
            if (novoUsuario == null || string.IsNullOrEmpty(novoUsuario.Email) || string.IsNullOrEmpty(novoUsuario.Senha))
            {
                return BadRequest("Dados inválidos.");
            }

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Email == novoUsuario.Email);
            if (usuarioExiste)
            {
                return BadRequest("Este e-mail já está cadastrado.");
            }

            // Salva o novo usuário no banco de dados
            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return Ok("Usuário cadastrado com sucesso!");
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}

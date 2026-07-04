using Microsoft.AspNetCore.Mvc;
using ProjetoTeste.Model;

namespace ProjetoTeste.Controllers
{
    [ApiController]
    [Route("api/Home")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        public IActionResult Teste([FromBody] DistanciaModel dados)
        {
            return Ok(dados);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProjetoTeste.Model;

namespace ProjetoTeste.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistanciaController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceberDistancia([FromBody] DistanciaModel dados)
        {
            Console.WriteLine($"[API] Distância recebida do ESP32: {dados.Distancia} cm");

            return Ok(new { status = "sucesso", mensagem = "Dado recebido corretamente!" });
        }
    }
}

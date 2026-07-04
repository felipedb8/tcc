using Microsoft.AspNetCore.Mvc;
using ProjetoTeste.Model;

namespace ProjetoTeste.Controllers
{
    [ApiController]
    [Route("api/Distancia")]
    public class DistanciaController : ControllerBase
    {
        [HttpPost]
        public IActionResult ReceberDistancia([FromBody] DistanciaModel dados)
        {
            Console.WriteLine($"Distância recebida: {dados.Distancia}");

            return Ok(new
            {
                status = "sucesso",
                recebido = dados.Distancia,
                mensagem = "OK do servidor"
            });
        }
    }
}

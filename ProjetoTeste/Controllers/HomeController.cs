using Microsoft.AspNetCore.Mvc;

namespace ProjetoTeste.Controllers
{
    [ApiController]
    [Route("api/Distancia")]
    public class DistanciaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] float valor)
        {
            bool estaOcupado = valor > 0 && valor <= 200.0f;

            // Limpa uma linha e joga o status bem destacado no Console Log do Servidor
            Console.WriteLine("\n=============================================");
            if (estaOcupado)
            {
                Console.WriteLine($"[STATUS] 🔴 OCUPADO! (Carro detectado a {valor} cm)");
            }
            else
            {
                Console.WriteLine($"[STATUS] 🟢 LIVRE! (Distância atual: {valor} cm)");
            }
            Console.WriteLine("=============================================");

            return Ok(new
            {
                status = "Sucesso",
                distanciaRegistrada = valor,
                vagaOcupada = estaOcupado
            });
        }
    }
}

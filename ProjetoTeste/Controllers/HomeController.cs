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
            Console.WriteLine($"[API] GET chamado pelo ESP32. Distância recebida: {valor} cm");

            return Ok(new
            {
                status = "Sucesso",
                distanciaRegistrada = valor
            });
        }
    }
}

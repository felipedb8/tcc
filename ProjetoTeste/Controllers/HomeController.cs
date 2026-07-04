using Microsoft.AspNetCore.Mvc;

namespace ProjetoTeste.Controllers
{
    [ApiController]
    [Route("api/Distancia")]
    public class DistanciaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine("[API] GET chamado pelo ESP32");

            return Ok(new
            {
                distancia = 10
            });
        }
    }
}

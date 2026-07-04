[ApiController]
[Route("api/Distancia")]
public class DistanciaController : ControllerBase
{
    [HttpPost]
    public IActionResult ReceberDistancia([FromBody] DistanciaModel dados)
    {
        return Ok(new
        {
            status = "sucesso",
            recebido = dados.Distancia
        });
    }
}

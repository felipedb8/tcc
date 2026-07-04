using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ProjetoTeste.Controllers
{
    [ApiController]
    [Route("api/Distancia")]
    public class DistanciaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DistanciaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] float valor)
        {
            bool estaOcupado = valor > 0 && valor <= 200.0f;
            try
            {
                var connectionString = _configuration.GetConnectionString("PostgresConnection");

                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();

                var sql = @"INSERT INTO leituras (distancia, vaga_ocupada, data_hora)
                            VALUES (@distancia, @vaga, NOW())";

                using var cmd = new NpgsqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("distancia", valor);
                cmd.Parameters.AddWithValue("vaga", estaOcupado);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = ex.Message
                });
            }

            return Ok(new
            {
                status = "Sucesso",
                distanciaRegistrada = valor,
                vagaOcupada = estaOcupado
            });
        }
    }
}

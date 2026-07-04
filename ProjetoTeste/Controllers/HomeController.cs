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
                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection");

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

        [HttpGet("init")]
        public IActionResult Init()
        {
            try
            {
                var connString = _configuration.GetConnectionString("PostgresConnection");

                using var conn = new NpgsqlConnection(connString);
                conn.Open();

                var sql = @"
                    CREATE TABLE IF NOT EXISTS leituras (
                        id SERIAL PRIMARY KEY,
                        distancia REAL NOT NULL,
                        vaga_ocupada BOOLEAN NOT NULL,
                        data_hora TIMESTAMP NOT NULL
                    );
                ";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                return Ok("Tabela criada/verificada");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    erro = ex.Message
                });
            }
        }
    }
}

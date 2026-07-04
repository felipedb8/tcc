using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ProjetoTeste.Controllers
{
    [ApiController]
    [Route("api/Distancia")]
    public class DistanciaController : ControllerBase
    {
        private string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection");
        }

        [HttpGet]
        public IActionResult Get([FromQuery] float valor)
        {
            bool estaOcupado = valor > 0 && valor <= 200.0f;

            try
            {
                using var conn = new NpgsqlConnection(GetConnectionString());
                conn.Open();

                var sql = @"
                    INSERT INTO leituras (distancia, vaga_ocupada, data_hora)
                    VALUES (@distancia, @vaga, NOW());
                ";

                using var cmd = new NpgsqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("distancia", valor);
                cmd.Parameters.AddWithValue("vaga", estaOcupado);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }

            return Ok(new
            {
                status = "Sucesso",
                distanciaRegistrada = valor,
                vagaOcupada = estaOcupado
            });
        }

        [HttpGet("log")]
        public IActionResult GetLog()
        {
            try
            {
                using var conn = new NpgsqlConnection(GetConnectionString());
                conn.Open();

                var sql = @"
                    SELECT id, distancia, vaga_ocupada, data_hora
                    FROM leituras
                    ORDER BY data_hora DESC;
                ";

                using var cmd = new NpgsqlCommand(sql, conn);
                using var reader = cmd.ExecuteReader();

                var lista = new List<object>();

                while (reader.Read())
                {
                    lista.Add(new
                    {
                        id = reader.GetInt32(0),
                        distancia = reader.GetFloat(1),
                        vagaOcupada = reader.GetBoolean(2),
                        dataHora = reader.GetDateTime(3)
                    });
                }

                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        [HttpGet("init")]
        public IActionResult Init()
        {
            try
            {
                using var conn = new NpgsqlConnection(GetConnectionString());
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
                return StatusCode(500, new { erro = ex.Message });
            }
        }
    }
}

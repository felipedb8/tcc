
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TCC2Proj.Data;
using TCC2Proj.Models;

namespace TCC2Proj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistanciaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DistanciaController> _logger;

        public DistanciaController(
            AppDbContext context,
            ILogger<DistanciaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(
            [FromBody] Leitura leitura)
        {
            if (leitura == null)
            {
                _logger.LogWarning("Leitura invalida recebida.");
                return BadRequest("Leitura inválida.");
            }

            _logger.LogInformation(
                "Requisicao recebida do ESP32. Distancia: {Distancia} cm",
                leitura.Distancia);

            leitura.DataHora = DateTime.UtcNow;

            _context.Leituras.Add(leitura);

            var vaga = await _context.VagasLivres
                .FirstOrDefaultAsync(v => v.Id == 1);

            if (vaga == null)
            {
                vaga = new VagasLivres
                {
                    Id = 1,
                    CodigoVaga = "VAGA_001",
                    Localizacao = "Estacionamento Principal",
                    Status = "livre",
                    AtualizadoEm = leitura.DataHora
                };

                _context.VagasLivres.Add(vaga);
            }

            if (!leitura.VagaOcupada)
            {
                vaga.Status = "livre";
            }
            else if (vaga.Status != "ocupada")
            {
                vaga.Status = "aguardando_confirmacao";
            }

            vaga.AtualizadoEm = leitura.DataHora;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Dados salvos no PostgreSQL. Vaga: {Id}, Status: {Status}",
                vaga.Id,
                vaga.Status);

            return Ok(new
            {
                sucesso = true,
                mensagem = "Dados recebidos e processados com sucesso.",
                leitura.Distancia,
                leitura.VagaOcupada,
                vaga.Status
            });
        }

        [HttpPost("confirmar/{id}")]
        public async Task<IActionResult> ConfirmarChegada(int id)
        {
            var vaga = await _context.VagasLivres
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vaga == null)
                return NotFound("Vaga não encontrada.");

            var ultimaLeitura = await _context.Leituras
                .OrderByDescending(l => l.DataHora)
                .FirstOrDefaultAsync();

            if (ultimaLeitura == null || !ultimaLeitura.VagaOcupada)
            {
                return BadRequest(
                    "O sensor não detectou um objeto na vaga.");
            }

            vaga.Status = "ocupada";
            vaga.AtualizadoEm = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Chegada confirmada!",
                vaga.Id,
                vaga.Status
            });
        }

        [HttpGet("vagas")]
        public async Task<IActionResult> ListarVagas()
        {
            var vagas = await _context.VagasLivres.ToArrayAsync();

            return Ok(vagas);
        }
    }
}

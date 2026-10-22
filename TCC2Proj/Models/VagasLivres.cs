using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TCC2Proj.Models
{
    [Table("vagas_livres")] // Aponta para a tabela da imagem
    public class VagasLivres
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("codigo_vaga")]
        public string CodigoVaga { get; set; }

        [Required]
        [Column("status")]
        public string Status { get; set; }

        [Column("localizacao")]
        public string Localizacao { get; set; }

        [Column("atualizado_em")]
        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
    }
}

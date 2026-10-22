using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TCC2Proj.Models
{
    [Table("leituras")]
    public class Leitura
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("distancia")]
        [JsonPropertyName("distancia")]
        public double Distancia { get; set; }

        [Column("vaga_ocupada")]
        [JsonPropertyName("vagaOcupada")]
        public bool VagaOcupada { get; set; }

        [Column("data_hora")]
        public DateTime DataHora { get; set; }
    }
}

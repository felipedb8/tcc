using Microsoft.EntityFrameworkCore;
using TCC2Proj.Models;

namespace TCC2Proj.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Leitura> Leituras { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }

        public DbSet<VagasLivres> VagasLivres { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPI.Model;

namespace WebAPI.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Estado> Estados { get; set; }

        public DbSet<Cidade> Cidades { get; set; }

        public DbSet<EstadoCivil> EstadoCivils { get; set; }
        public DbSet<Login> Logins { get; set; }
        
        



    }
}

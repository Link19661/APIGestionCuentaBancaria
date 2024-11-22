using Microsoft.EntityFrameworkCore;
using ApiGestionBancaria.Models;
using System.Security.Cryptography.X509Certificates;

namespace ApiGestionBancaria.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<CuentaBancaria> CuentaBancarias { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }

        //Definir Id como autoincrementable
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CuentaBancaria>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CuentaBancaria>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Transaccion>()
             .HasKey(t => t.Id);

            modelBuilder.Entity<Transaccion>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
        }

    }
}

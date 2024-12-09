using CamposDealer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CamposDealer.DB
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Venda> Vendas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define a chave primária, pois estava dando erro ao inserir os dados da API na base, por vir com ID préfixado. 

            modelBuilder.Entity<Cliente>()
                .HasKey(c => c.idCliente); 

            modelBuilder.Entity<Produto>()
                .HasKey(p => p.idProduto); 

            modelBuilder.Entity<Venda>()
                .HasKey(v => v.idVenda); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

    }
}
